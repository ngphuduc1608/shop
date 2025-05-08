using Abp.Application.Services;
using Abp.Application.Services.Dto;
using proj_tt.Addresses.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace proj_tt.Addresses
{
    public interface IAddressAppService : IApplicationService
    {
        Task<AddressDto> GetAddress(int id);
        Task<List<AddressDto>> GetUserAddresses();
        Task<AddressDto> CreateAddress(CreateAddressInput input);
        Task<AddressDto> UpdateAddress(UpdateAddressInput input);
        Task DeleteAddress(int id);
        Task SetDefaultAddress(int id);
    }
} 