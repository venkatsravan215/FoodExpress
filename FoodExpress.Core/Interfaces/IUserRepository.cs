using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoodExpress.Core.Entities;

namespace FoodExpress.Core.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(int id);

        Task<User?> GetByEmailAsync(string email);

        Task<IEnumerable<User>> GetAllAsync();

        Task<User> AddAsync(User user);

        Task UpdateAsync(User user);

        Task DeleteAsync(int id);
    }
}