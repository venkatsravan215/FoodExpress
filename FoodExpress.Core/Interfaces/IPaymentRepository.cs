using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FoodExpress.Core.Entities;

namespace FoodExpress.Core.Interfaces
{
    public interface IPaymentRepository
    {
        Task<Payment?> GetByIdAsync(int id);

        Task<IEnumerable<Payment>> GetAllAsync();

        Task<IEnumerable<Payment>> GetByOrderIdAsync(int orderId);

        Task AddAsync(Payment payment);

        Task UpdateAsync(Payment payment);

        Task DeleteAsync(Payment payment);
    }
}

