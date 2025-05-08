using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using Microsoft.EntityFrameworkCore;
using proj_tt.Addresses.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace proj_tt.Addresses
{
    public class AddressAppService : ApplicationService, IAddressAppService
    {
        private readonly IRepository<Address> _addressRepository;
        private readonly IAbpSession _abpSession;

        public AddressAppService(
            IRepository<Address> addressRepository,
            IAbpSession abpSession)
        {
            _addressRepository = addressRepository;
            _abpSession = abpSession;
        }

        public async Task<AddressDto> GetAddress(int id)
        {
            var address = await _addressRepository.GetAsync(id);
            return ObjectMapper.Map<AddressDto>(address);
        }

        public async Task<List<AddressDto>> GetUserAddresses()
        {
            var userId = _abpSession.UserId;
            if (userId == null)
            {
                throw new ApplicationException("User not logged in");
            }

            var addresses = await _addressRepository.GetAll()
                .Where(a => a.UserId == userId)
                .OrderByDescending(a => a.IsDefault)
                .ThenByDescending(a => a.CreationTime)
                .ToListAsync();

            return ObjectMapper.Map<List<AddressDto>>(addresses);
        }

        public async Task<AddressDto> CreateAddress(CreateAddressInput input)
        {
            var userId = _abpSession.UserId;
            if (userId == null)
            {
                throw new ApplicationException("User not logged in");
            }

            if (input.IsDefault)
            {
                // Set all other addresses to non-default
                var existingAddresses = await _addressRepository.GetAll()
                    .Where(a => a.UserId == userId)
                    .ToListAsync();

                foreach (var existingAddress in existingAddresses)
                {
                    existingAddress.IsDefault = false;
                    await _addressRepository.UpdateAsync(existingAddress);
                }
            }

            var address = new Address(
                input.Name,
                input.AddressLine,
                input.Phone,
                input.ProvinceName,
                input.DistrictName,
                input.WardName,
                userId.Value,
                input.IsDefault
            );

            await _addressRepository.InsertAsync(address);
            await CurrentUnitOfWork.SaveChangesAsync();

            return ObjectMapper.Map<AddressDto>(address);
        }

        public async Task<AddressDto> UpdateAddress(UpdateAddressInput input)
        {
            var address = await _addressRepository.GetAsync(input.Id);
            var userId = _abpSession.UserId;

            if (userId == null || address.UserId != userId)
            {
                throw new ApplicationException("Unauthorized");
            }

            if (input.IsDefault && !address.IsDefault)
            {
                // Set all other addresses to non-default
                var existingAddresses = await _addressRepository.GetAll()
                    .Where(a => a.UserId == userId && a.Id != input.Id)
                    .ToListAsync();

                foreach (var existingAddress in existingAddresses)
                {
                    existingAddress.IsDefault = false;
                    await _addressRepository.UpdateAsync(existingAddress);
                }
            }

            address.Name = input.Name;
            address.AddressLine = input.AddressLine;
            address.Phone = input.Phone;
            address.ProvinceName = input.ProvinceName;
            address.DistrictName = input.DistrictName;
            address.WardName = input.WardName;
            address.IsDefault = input.IsDefault;

            await _addressRepository.UpdateAsync(address);
            await CurrentUnitOfWork.SaveChangesAsync();

            return ObjectMapper.Map<AddressDto>(address);
        }

        public async Task DeleteAddress(int id)
        {
            var address = await _addressRepository.GetAsync(id);
            var userId = _abpSession.UserId;

            if (userId == null || address.UserId != userId)
            {
                throw new ApplicationException("Unauthorized");
            }

            await _addressRepository.DeleteAsync(address);
            await CurrentUnitOfWork.SaveChangesAsync();
        }

        public async Task SetDefaultAddress(int id)
        {
            var address = await _addressRepository.GetAsync(id);
            var userId = _abpSession.UserId;

            if (userId == null || address.UserId != userId)
            {
                throw new ApplicationException("Unauthorized");
            }

            if (!address.IsDefault)
            {
                // Set all other addresses to non-default
                var existingAddresses = await _addressRepository.GetAll()
                    .Where(a => a.UserId == userId && a.Id != id)
                    .ToListAsync();

                foreach (var existingAddress in existingAddresses)
                {
                    existingAddress.IsDefault = false;
                    await _addressRepository.UpdateAsync(existingAddress);
                }

                address.IsDefault = true;
                await _addressRepository.UpdateAsync(address);
                await CurrentUnitOfWork.SaveChangesAsync();
            }
        }
    }
}