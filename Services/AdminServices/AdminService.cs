using BackendProject.Data;
using BackendProject.Dto;
using BackendProject.Models;
using BackendProject.Services.CloudinaryServices;
using Microsoft.EntityFrameworkCore;
using BackendProject.ApiResponse;
using System.Linq;
using AutoMapper.Internal;


namespace BackendProject.Services.AdminServices
{
    public class AdminService : IAdminService
    {
        private readonly AppDbContext _context;
        private readonly ICloudinaryService _cloudinaryService;

        public AdminService(AppDbContext context, ICloudinaryService cloudinaryService)
        {
            _context = context;
            _cloudinaryService = cloudinaryService;
        }

        public async Task<ApiResponse<object>> GetDashboardOverviewAsync()
        {
            var users = await _context.Users
                .Include(u => u.Orders)
                .Where(u => u.Role != "Admin" && !u.IsBlocked)
                .ToListAsync();

            var allOrders = await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .Where(o => users.Select(u => u.Id).Contains(o.UserId))
                .ToListAsync();

            var totalRevenue = allOrders.Sum(o => o.TotalAmount);

            // Recent activities (users and orders)
            var recentUserRegistrations = await _context.Users
      .Where(u => u.Role != "Admin" && !u.IsBlocked)
      .OrderByDescending(u => u.CreatedAt)
      .Select(u => new RecentActivity
      {
          Timestamp = (DateTime)u.CreatedAt,
          Message = $"New user registered: {u.Name}"
      })
      .Take(5)
      .ToListAsync();

            var recentOrders = await _context.Orders
                .OrderByDescending(o => o.OrderDate)
                .Select(o => new RecentActivity
                {
                    Timestamp = o.OrderDate,
                    Message = $"New order placed by user ID {o.UserId}, Amount: ₹{o.TotalAmount:F2}"
                })
                .Take(5)
                .ToListAsync();


            var recentActivities = recentUserRegistrations
    .Concat(recentOrders)
    .OrderByDescending(a => a.Timestamp)
    .Take(10)
    .ToList();



            var overview = new
            {
                totalUsers = users.Count,
                totalOrders = allOrders.Count,
                totalRevenue = totalRevenue,
                ordersOverTime = allOrders
                    .Where(o => o.OrderDate != DateTime.MinValue)
                    .GroupBy(o => o.OrderDate.Date)
                    .Select(g => new {
                        date = g.Key.ToString("dd-MM-yyyy"),
                        count = g.Count()
                    }).ToList(),
                revenueByUser = users
                    .Where(u => u.Orders != null && u.Orders.Any())
                    .Select(u => new {
                        name = u.Name,
                        value = u.Orders.Sum(o => o.TotalAmount)
                    }).ToList(),
                topProducts = allOrders
                    .SelectMany(o => o.OrderItems)
                    .Where(oi => oi.Product != null)
                    .GroupBy(oi => oi.Product.ProductName)
                    .Select(g => new {
                        title = g.Key,
                        qty = g.Sum(oi => oi.Quantity)
                    })
                    .OrderByDescending(p => p.qty)
                    .Take(5)
                    .ToList(),
                ordersPerUser = users
                    .Select(u => new {
                        name = u.Name,
                        orders = u.Orders?.Count ?? 0
                    }).ToList(),
                dailyRevenueTrend = allOrders
                    .Where(o => o.OrderDate != DateTime.MinValue)
                    .GroupBy(o => o.OrderDate.Date)
                    .Select(g => new {
                        date = g.Key.ToString("dd-MM-yyyy"),
                        total = g.Sum(o => o.TotalAmount)
                    }).ToList(),
                recentActivities
            };

            return ApiResponse<object>.SuccessResponse(overview);
        }


        public async Task<ApiResponse<List<UserOrderDto>>> GetAllUsersAsync()
        {
            var users = await _context.Users
                .Where(u => u.Role != "admin")
                .Include(u => u.Orders)
                    .ThenInclude(o => o.OrderItems)
                        .ThenInclude(oi => oi.Product)
                .ToListAsync();

            var result = users.Select(user => new UserOrderDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Password = user.Password,
                IsBlocked = user.IsBlocked, 
                Orders = user.Orders?.Select(order => new OrderViewDto
                {
                    OrderId = order.Id,
                    TotalAmount = order.TotalAmount,
                    DeliveryStatus = order.DeliveryStatus,
                    PaymentStatus = order.PaymentStatus,
                    Items = order.OrderItems?.Select(item => new OrderItemDto
                    {
                        ProductId = item.ProductId,
                        ProductName = (string)(item.Product?.ProductName ?? "Unknown"),
                        Description = item.Product?.Description ?? string.Empty,
                        ImageUrl = item.Product?.ImageUrl,
                        Quantity = item.Quantity
                    }).ToList() ?? new List<OrderItemDto>()
                }).ToList() ?? new List<OrderViewDto>()
            }).ToList();


            return ApiResponse<List<UserOrderDto>>.SuccessResponse(result);
        }


        public async Task<ApiResponse<string>> ToggleBlockUserAsync(int userId, bool isBlocked)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return ApiResponse<string>.FailureResponse("User not found");

            user.IsBlocked = isBlocked;
            await _context.SaveChangesAsync();
            return ApiResponse<string>.SuccessResponse("User status updated");
        }

        public async Task<ApiResponse<string>> DeleteUserAsync(int userId)
        {
            var user = await _context.Users
                .Include(u => u.Orders)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                return ApiResponse<string>.FailureResponse("User not found");

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return ApiResponse<string>.SuccessResponse("User deleted successfully");
        }

        public async Task<ApiResponse<List<ProductWithCategoryDto>>> GetAllProductsAsync()
        {
            var products = await _context.Products
                .Include(p => p.Category)
                .Select(p => new ProductWithCategoryDto
                {
                    Id = p.ProductId,
                    ProductName = p.ProductName,
                    Description = p.Description,
                    Price = p.Price,
                    ImageUrl = p.ImageUrl,
                    CategoryId = p.CategoryId,
                    CategoryName = p.Category != null ? p.Category.CategoryName : null
                })
                .ToListAsync();

            return ApiResponse<List<ProductWithCategoryDto>>.SuccessResponse(products);
        }

        public async Task<ApiResponse<ProductWithCategoryDto>> GetProductByIdAsync(int id)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .Where(p => p.ProductId == id)
                .Select(p => new ProductWithCategoryDto
                {
                    Id = p.ProductId,
                    ProductName = p.ProductName,
                    Description = p.Description,
                    Price = p.Price,
                    ImageUrl = p.ImageUrl,
                    CategoryId = p.CategoryId,
                    CategoryName = p.Category != null ? p.Category.CategoryName : null
                })
                .FirstOrDefaultAsync();

            if (product == null)
                return ApiResponse<ProductWithCategoryDto>.FailureResponse("Product not found");

            return ApiResponse<ProductWithCategoryDto>.SuccessResponse(product);
        }


        public async Task<ApiResponse<ProductWithCategoryDto>> CreateProductAsync(CreateProductDto productDto)
        {
            try
            {
                string imageUrl = "";
                if (productDto.Image != null)
                {
                    imageUrl = await _cloudinaryService.UploadImageAsync(productDto.Image);
                }

                var category = await _context.Categories
                    .FirstOrDefaultAsync(c => c.CategoryName == productDto.CategoryName);

                if (category == null)
                    return ApiResponse<ProductWithCategoryDto>.FailureResponse("Category not found");

                var product = new Product
                {
                    ProductName = productDto.ProductName,
                    Description = productDto.Description,
                    Price = productDto.Price,
                    CategoryId = category.Id,
                    ImageUrl = imageUrl
                };

                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                var createdProduct = await _context.Products
                    .Include(p => p.Category)
                    .Where(p => p.ProductId == product.ProductId)
                    .Select(p => new ProductWithCategoryDto
                    {
                        Id = p.ProductId,
                        ProductName = p.ProductName,
                        Description = p.Description,
                        Price = p.Price,
                        ImageUrl = p.ImageUrl,
                        CategoryId = p.CategoryId,
                        CategoryName = p.Category.CategoryName
                    })
                    .FirstOrDefaultAsync();

                return ApiResponse<ProductWithCategoryDto>.SuccessResponse(createdProduct, "Product created successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<ProductWithCategoryDto>.FailureResponse($"Failed to create product: {ex.Message}");
            }
        }



        public async Task<ApiResponse<ProductWithCategoryDto>> UpdateProductAsync(UpdateNewProductDto dto)
        {
            var existing = await _context.Products.FindAsync(dto.Id);
            if (existing == null)
                return ApiResponse<ProductWithCategoryDto>.FailureResponse("Product not found");

            if (dto.Image != null)
            {
                var imageUrl = await _cloudinaryService.UploadImageAsync(dto.Image);
                existing.ImageUrl = imageUrl;
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.CategoryName == dto.CategoryName);

            if (category == null)
                return ApiResponse<ProductWithCategoryDto>.FailureResponse("Category not found");

            existing.ProductName = dto.ProductName;
            existing.Description = dto.Description;
            existing.Price = dto.Price;
            existing.CategoryId = category.Id;

            await _context.SaveChangesAsync();

           
            var updatedProduct = await _context.Products
                .Include(p => p.Category)
                .Where(p => p.ProductId == existing.ProductId)
                .Select(p => new ProductWithCategoryDto
                {
                    Id = p.ProductId,
                    ProductName = p.ProductName,
                    Description = p.Description,
                    Price = p.Price,
                    ImageUrl = p.ImageUrl,
                    CategoryId = p.CategoryId,
                    CategoryName = p.Category.CategoryName
                })
                .FirstOrDefaultAsync();

            return ApiResponse<ProductWithCategoryDto>.SuccessResponse(updatedProduct!, "Product updated successfully");
        }



        public async Task<ApiResponse<string>> DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return ApiResponse<string>.FailureResponse("Product not found");

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return ApiResponse<string>.SuccessResponse("Product deleted successfully");
        }

        public async Task<ApiResponse<List<ProductWithCategoryDto>>> SearchProductsAsync(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return ApiResponse<List<ProductWithCategoryDto>>.FailureResponse("Search keyword is required");

            var query = await _context.Products
                .Include(p => p.Category)
                .Where(p => p.ProductName != null &&
                            p.ProductName.ToLower().StartsWith(keyword.ToLower()))
                .ToListAsync();

            var result = query.Select(p => new ProductWithCategoryDto
            {
                Id = p.ProductId,
                ProductName = p.ProductName,
                Description = p.Description,
                Price = p.Price,
                ImageUrl = p.ImageUrl,
                CategoryId = p.CategoryId,
                CategoryName = p.Category != null ? p.Category.CategoryName : null
            }).ToList();

            return ApiResponse<List<ProductWithCategoryDto>>.SuccessResponse(result);
        }

        public async Task<ApiResponse<PaginatedResponse<ProductWithCategoryDto>>> GetProductsPaginatedAsync(int page, int pageSize, string category)
        {
            var query = _context.Products.Include(p => p.Category).AsQueryable();

            if (!string.IsNullOrWhiteSpace(category))
            {
                var lc = category.ToLower();
                if (lc == "dogfoodall")
                {
                    query = query.Where(p => new[] { "dogfoodall", "drydogfood", "wetdogfood" }
                        .Contains(p.Category.CategoryName.ToLower()));
                }
                else if (lc == "catfoodall")
                {
                    query = query.Where(p => new[] { "catfoodall", "drycatfood", "wetcatfood" }
                        .Contains(p.Category.CategoryName.ToLower()));
                }
                else
                {
                    query = query.Where(p => p.Category.CategoryName.ToLower() == lc);
                }
            }

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderByDescending(p => p.ProductId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new ProductWithCategoryDto
                {
                    Id = p.ProductId,
                    ProductName = p.ProductName,
                    Description = p.Description,
                    Price = p.Price,
                    ImageUrl = p.ImageUrl,
                    CategoryId = p.CategoryId,
                    CategoryName = p.Category.CategoryName
                })
                .ToListAsync();

            var result = new PaginatedResponse<ProductWithCategoryDto>
            {
                Items = items,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };

            return ApiResponse<PaginatedResponse<ProductWithCategoryDto>>.SuccessResponse(result);
        }


        public async Task<ApiResponse<PaginatedResponse<UserOrderDto>>> GetUsersPaginatedAsync(int page, int pageSize, string keyword)
        {
            var query = _context.Users
                .Include(u => u.Orders)
                    .ThenInclude(o => o.OrderItems)
                        .ThenInclude(oi => oi.Product)
                .Where(u => u.Role != "admin");

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                keyword = keyword.ToLower();
                query = query.Where(u => u.Name.ToLower().StartsWith(keyword));
            }

            var totalCount = await query.CountAsync();

            var users = await query
                .OrderByDescending(u => u.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var userDtos = users.Select(user => new UserOrderDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Password = user.Password,
                IsBlocked = user.IsBlocked,
                Orders = user.Orders?.Select(order => new OrderViewDto
                {
                    OrderId = order.Id,
                    TotalAmount = order.TotalAmount,
                    DeliveryStatus = order.DeliveryStatus,
                    PaymentStatus = order.PaymentStatus,
                    Items = order.OrderItems?.Select(item => new OrderItemDto
                    {
                        ProductId = item.ProductId,
                        ProductName = (string)(item.Product?.ProductName ?? "Unknown"),
                        Description = item.Product?.Description ?? string.Empty,
                        ImageUrl = item.Product?.ImageUrl,
                        Quantity = item.Quantity
                    }).ToList() ?? new List<OrderItemDto>()
                }).ToList() ?? new List<OrderViewDto>()
            }).ToList();

            var result = new PaginatedResponse<UserOrderDto>
            {
                Items = userDtos,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };

            return ApiResponse<PaginatedResponse<UserOrderDto>>.SuccessResponse(result);
        }



    }
}
