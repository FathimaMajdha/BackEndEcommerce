using BackendProject.Dto;
using BackendProject.Services.AddressServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BackendProject.ApiResponse;

namespace BackendProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _addressService;

        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        [HttpPost("add/{userId}")]
        public async Task<IActionResult> AddAddress(int userId, [FromBody] AddNewAddressDto address)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);

                return BadRequest(ApiResponse<string>.FailureResponse(
                    "Invalid address data: " + string.Join(", ", errors)));
            }

            var response = await _addressService.AddnewAddress(userId, address);
            return response.Success ? Ok(response) : StatusCode(500, response);
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetAddresses(int userId)
        {
            var response = await _addressService.GetAddress(userId);
            return Ok(response); 
        }

        [HttpDelete("{addressId}")]
        public async Task<IActionResult> DeleteAddress(int addressId)
        {
            var response = await _addressService.RemoveAddress(addressId);
            return response.Success ? Ok(response) : NotFound(response);
        }
    }
}
