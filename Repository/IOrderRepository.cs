using IBSRAStore.DTOs;

namespace IBSRAStore.Repository
{
    public interface IOrderRepository
    {
        Task<int> CreateOrderAsync(string orderId, int productId, string customerEmail, 
            string customerName, decimal totalAmount, decimal taxAmount, decimal subtotalAmount,
            string cardNumberLast4, string expiryDate, string address, string city, string zipCode);
        Task<OrderDetailsDto?> GetOrderDetailsAsync(string orderId);
    }
}
