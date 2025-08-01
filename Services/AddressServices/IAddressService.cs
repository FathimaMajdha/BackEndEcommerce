using BackendProject.ApiResponse;
using BackendProject.Dto;

public interface IAddressService
{
    Task<ApiResponse<string>> AddnewAddress(int userId, AddNewAddressDto dto);
    Task<ApiResponse<List<GetAddressDto>>> GetAddress(int userId);
    Task<ApiResponse<string>> RemoveAddress(int addressId);
}
