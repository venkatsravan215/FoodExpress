using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FoodExpress.Core.DTOs;
using FoodExpress.Core.Entities;
using FoodExpress.Core.Interfaces;

namespace FoodExpress.Infrastructure.Services
{
    public class AddressService : IAddressService
    {
        private readonly IAddressRepository _addressRepository;

        public AddressService(IAddressRepository addressRepository)
        {
            _addressRepository = addressRepository;
        }

        public async Task<IEnumerable<AddressResponseDto>> GetByUserIdAsync(
            int userId)
        {
            var addresses =
                await _addressRepository.GetByUserIdAsync(userId);

            return addresses.Select(MapToResponseDto).ToList();
        }

        public async Task<AddressResponseDto?> GetByIdAsync(int id)
        {
            var address = await _addressRepository.GetByIdAsync(id);

            return address == null ? null : MapToResponseDto(address);
        }

        public async Task<AddressResponseDto> CreateAsync(
            AddressCreateDto addressDto)
        {
            var address = new Address
            {
                AddressLine = addressDto.AddressLine,
                City = addressDto.City,
                State = addressDto.State,
                PostalCode = addressDto.PostalCode,
                Country = addressDto.Country,
                Label = addressDto.Label,
                IsDefault = addressDto.IsDefault,
                UserId = addressDto.UserId
            };

            var createdAddress =
                await _addressRepository.AddAsync(address);

            return MapToResponseDto(createdAddress);
        }

        public async Task UpdateAsync(
            int id, AddressCreateDto addressDto)
        {
            var address = await _addressRepository.GetByIdAsync(id);

            if (address == null)
                throw new KeyNotFoundException("Address not found.");

            address.AddressLine = addressDto.AddressLine;
            address.City = addressDto.City;
            address.State = addressDto.State;
            address.PostalCode = addressDto.PostalCode;
            address.Country = addressDto.Country;
            address.Label = addressDto.Label;
            address.IsDefault = addressDto.IsDefault;

            await _addressRepository.UpdateAsync(address);
        }

        public async Task DeleteAsync(int id)
        {
            await _addressRepository.DeleteAsync(id);
        }

        private static AddressResponseDto MapToResponseDto(Address address)
        {
            return new AddressResponseDto
            {
                Id = address.Id,
                AddressLine = address.AddressLine,
                City = address.City,
                State = address.State,
                PostalCode = address.PostalCode,
                Country = address.Country,
                Label = address.Label,
                IsDefault = address.IsDefault,
                UserId = address.UserId
            };
        }
    }
}
