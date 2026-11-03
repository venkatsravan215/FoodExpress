using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FoodExpress.Core.DTOs;

namespace FoodExpress.Core.Interfaces
{
    public interface IAddressService
    {
        Task<IEnumerable<AddressResponseDto>> GetByUserIdAsync(int userId);

        Task<AddressResponseDto?> GetByIdAsync(int id);

        Task<AddressResponseDto> CreateAsync(AddressCreateDto addressDto);

        Task UpdateAsync(int id, AddressCreateDto addressDto);

        Task DeleteAsync(int id);
    }
}
