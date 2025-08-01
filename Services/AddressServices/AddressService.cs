using Microsoft.EntityFrameworkCore;
using BackendProject.Data;
using BackendProject.Dto;
using BackendProject.Models;
using BackendProject.ApiResponse;

namespace BackendProject.Services.AddressServices
{
    public class AddressService : IAddressService
    {
        private readonly AppDbContext _context;

        public AddressService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<string>> AddnewAddress(int userId, AddNewAddressDto dto)
        {
            var newAddress = new UserAddress
            {
                UserId = userId,
                StreetName = dto.StreetName,
                City = dto.City,
                HomeAddress = dto.HomeAddress,
                CustomerPhone = dto.CustomerPhone,
                PostalCode = dto.PostalCode
            };

            await _context.UserAddresses.AddAsync(newAddress);
            var result = await _context.SaveChangesAsync();
            return result > 0
                ? ApiResponse<string>.SuccessResponse("Address added successfully")
                : ApiResponse<string>.FailureResponse("Failed to add address");
        }

        public async Task<ApiResponse<List<GetAddressDto>>> GetAddress(int userId)
        {
            var addresses = await _context.UserAddresses
                .Where(a => a.UserId == userId)
                .Select(a => new GetAddressDto
                {
                    Id = a.Id,
                    StreetName = a.StreetName,
                    City = a.City,
                    HomeAddress = a.HomeAddress,
                    CustomerPhone = a.CustomerPhone,
                    PostalCode = a.PostalCode
                }).ToListAsync();

            return ApiResponse<List<GetAddressDto>>.SuccessResponse(addresses, "Addresses fetched successfully");
        }

        public async Task<ApiResponse<string>> RemoveAddress(int addressId)
        {
            var address = await _context.UserAddresses.FindAsync(addressId);
            if (address == null)
                return ApiResponse<string>.FailureResponse("Address not found");

            _context.UserAddresses.Remove(address);
            var result = await _context.SaveChangesAsync();

            return result > 0
                ? ApiResponse<string>.SuccessResponse("Address removed successfully")
                : ApiResponse<string>.FailureResponse("Failed to remove address");
        }
    }
}
