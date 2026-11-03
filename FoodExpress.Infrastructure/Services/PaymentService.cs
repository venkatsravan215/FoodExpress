using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoodExpress.Core.DTOs;
using FoodExpress.Core.Entities;
using FoodExpress.Core.Enums;
using FoodExpress.Core.Interfaces;

namespace FoodExpress.Infrastructure.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IOrderRepository _orderRepository;

        public PaymentService(
            IPaymentRepository paymentRepository,
            IOrderRepository orderRepository)
        {
            _paymentRepository = paymentRepository;
            _orderRepository = orderRepository;
        }

        public async Task<PaymentResponseDto?> GetByIdAsync(int id)
        {
            var payment = await _paymentRepository.GetByIdAsync(id);

            return payment == null ? null : MapToDto(payment);
        }

        public async Task<IEnumerable<PaymentResponseDto>> GetAllAsync()
        {
            var payments = await _paymentRepository.GetAllAsync();

            return payments.Select(MapToDto);
        }

        public async Task<IEnumerable<PaymentResponseDto>> GetByOrderIdAsync(
            int orderId)
        {
            var payments =
                await _paymentRepository.GetByOrderIdAsync(orderId);

            return payments.Select(MapToDto);
        }

        public async Task<PaymentResponseDto> CreateAsync(
            PaymentCreateDto dto)
        {
            var order = await _orderRepository.GetByIdAsync(dto.OrderId);

            if (order == null)
            {
                throw new KeyNotFoundException("Order not found.");
            }

            if (!Enum.IsDefined(typeof(PaymentMethod), dto.Method))
            {
                throw new ArgumentException("Invalid payment method.");
            }

            if (dto.Amount != order.TotalAmount)
            {
                throw new ArgumentException(
                    "Payment amount must match the order total.");
            }

            var payment = new Payment
            {
                OrderId = order.Id,
                Amount = order.TotalAmount,
                Method = dto.Method,
                Status = PaymentStatus.Pending,
                TransactionReference = string.Empty,
                CreatedAt = DateTime.UtcNow
            };

            await _paymentRepository.AddAsync(payment);

            return MapToDto(payment);
        }

        public async Task<PaymentResponseDto?> UpdateStatusAsync(
            int id, PaymentStatus status)
        {
            var payment = await _paymentRepository.GetByIdAsync(id);

            if (payment == null)
            {
                return null;
            }

            if (!Enum.IsDefined(typeof(PaymentStatus), status))
            {
                throw new ArgumentException("Invalid payment status.");
            }

            payment.Status = status;

            await _paymentRepository.UpdateAsync(payment);

            return MapToDto(payment);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var payment = await _paymentRepository.GetByIdAsync(id);

            if (payment == null)
            {
                return false;
            }

            await _paymentRepository.DeleteAsync(payment);

            return true;
        }

        private static PaymentResponseDto MapToDto(Payment payment)
        {
            return new PaymentResponseDto
            {
                Id = payment.Id,
                OrderId = payment.OrderId,
                Amount = payment.Amount,
                Method = payment.Method,
                Status = payment.Status,
                TransactionReference = payment.TransactionReference,
                CreatedAt = payment.CreatedAt
            };
        }
    }
}

