using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FoodExpress.Core.DTOs;

namespace FoodExpress.Core.Interfaces
{
    public interface IPaymentService
    {
        Task<PaymentResponseDto?> GetByIdAsync(int id);

        Task<IEnumerable<PaymentResponseDto>> GetAllAsync();

        Task<IEnumerable<PaymentResponseDto>> GetByOrderIdAsync(int orderId);

        Task<PaymentResponseDto> CreateAsync(PaymentCreateDto dto);

        Task<PaymentResponseDto?> UpdateStatusAsync(
            int id, FoodExpress.Core.Enums.PaymentStatus status);

        Task<bool> DeleteAsync(int id);
    }
}

