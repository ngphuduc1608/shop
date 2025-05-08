using Abp.Domain.Entities.Auditing;
using proj_tt.Authorization.Users;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace proj_tt.Addresses
{
    [Table("AppAddresses")]
    public class Address : AuditedEntity
    {
        public const int MaxAddressLineLength = 512;
        public const int MaxPhoneLength = 20;
        public const int MaxNameLength = 256;
        public const int MaxLocationLength = 256;

        [Required]
        [StringLength(MaxNameLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(MaxAddressLineLength)]
        public string AddressLine { get; set; }

        [Required]
        [StringLength(MaxPhoneLength)]
        public string Phone { get; set; }

        [Required]
        [StringLength(MaxLocationLength)]
        public string ProvinceName { get; set; }

        [Required]
        [StringLength(MaxLocationLength)]
        public string DistrictName { get; set; }

        [Required]
        [StringLength(MaxLocationLength)]
        public string WardName { get; set; }

        public long UserId { get; set; }

        public bool IsDefault { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }

        public Address(string name, string addressLine, string phone, string provinceName, string districtName, string wardName, long userId, bool isDefault = false)
        {
            Name = name;
            AddressLine = addressLine;
            Phone = phone;
            ProvinceName = provinceName;
            DistrictName = districtName;
            WardName = wardName;
            UserId = userId;
            IsDefault = isDefault;
        }

        protected Address()
        {
        }
    }
}