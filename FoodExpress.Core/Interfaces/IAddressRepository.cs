using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FoodExpress.Core.Entities;

namespace FoodExpress.Core.Interfaces
{
    public interface IAddressRepository
    {
        Task<IEnumerable<Address>> GetByUserIdAsync(int userId);

        Task<Address?> GetByIdAsync(int id);

        Task<Address> AddAsync(Address address);

        Task UpdateAsync(Address address);

        Task DeleteAsync(int id);
    }
}
