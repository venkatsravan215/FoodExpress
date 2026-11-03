using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FoodExpress.Core.Entities;

namespace FoodExpress.Core.Interfaces
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAllAsync();

        Task<Order?> GetByIdAsync(int id);

        Task<IEnumerable<Order>> GetByUserIdAsync(int userId);

        Task<Order> AddAsync(Order order);

        Task UpdateAsync(Order order);

        Task DeleteAsync(int id);
    }
}
