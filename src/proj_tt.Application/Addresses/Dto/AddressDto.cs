using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using proj_tt.Addresses;

namespace proj_tt.Addresses.Dto
{
    [AutoMapFrom(typeof(Address))]
    public class AddressDto : AuditedEntityDto
    {
        public string Name { get; set; }
        public string AddressLine { get; set; }
        public string Phone { get; set; }
        public string ProvinceName { get; set; }
        public string DistrictName { get; set; }
        public string WardName { get; set; }
        public long UserId { get; set; }
        public bool IsDefault { get; set; }
    }

    public class CreateAddressInput
    {
        public string Name { get; set; }
        public string AddressLine { get; set; }
        public string Phone { get; set; }
        public string ProvinceName { get; set; }
        public string DistrictName { get; set; }
        public string WardName { get; set; }
        public bool IsDefault { get; set; }
    }

    public class UpdateAddressInput
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string AddressLine { get; set; }
        public string Phone { get; set; }
        public string ProvinceName { get; set; }
        public string DistrictName { get; set; }
        public string WardName { get; set; }
        public bool IsDefault { get; set; }
    }
} 