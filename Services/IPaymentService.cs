using IBSRAStore.DTOs;

namespace IBSRAStore.Services
{
    public interface IPaymentService
    {
        Task<PaymentResponse> ProcessPaymentAsync(PaymentRequest request);
        Task<OrderDetailsDto?> GetOrderDetailsAsync(string orderId);
    }
}
