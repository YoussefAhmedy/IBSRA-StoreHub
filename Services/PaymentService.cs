using IBSRAStore.DTOs;
using IBSRAStore.Repository;

namespace IBSRAStore.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;

        public PaymentService(IOrderRepository orderRepository, IProductRepository productRepository)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
        }

        public async Task<PaymentResponse> ProcessPaymentAsync(PaymentRequest request)
        {
            try
            {
                // Validate product exists
                var product = await _productRepository.GetProductByIdAsync(request.ProductId);
                if (product == null)
                {
                    return new PaymentResponse
                    {
                        Success = false,
                        Message = "Product not found"
                    };
                }

                // Generate unique order ID
                var orderId = $"ORD-{DateTime.UtcNow.Ticks}";

                // Calculate amounts
                var subtotal = request.Amount;
                var tax = subtotal * 0.1m;
                var total = subtotal + tax;

                // Get last 4 digits of card
                var cardLast4 = request.PaymentMethod.CardNumber.Replace(" ", "").Substring(
                    request.PaymentMethod.CardNumber.Replace(" ", "").Length - 4);

                // Simulate payment processing delay
                await Task.Delay(2000);

                // Create order
                var orderDbId = await _orderRepository.CreateOrderAsync(
                    orderId,
                    request.ProductId,
                    request.BillingAddress.Email,
                    request.PaymentMethod.HolderName,
                    total,
                    tax,
                    subtotal,
                    cardLast4,
                    request.PaymentMethod.ExpiryDate,
                    request.BillingAddress.Address,
                    request.BillingAddress.City,
                    request.BillingAddress.ZipCode
                );

                return new PaymentResponse
                {
                    Success = true,
                    Message = "Payment processed successfully",
                    OrderId = orderId,
                    TransactionId = Guid.NewGuid().ToString(),
                    Amount = total
                };
            }
            catch (Exception ex)
            {
                return new PaymentResponse
                {
                    Success = false,
                    Message = $"Payment processing failed: {ex.Message}"
                };
            }
        }

        public async Task<OrderDetailsDto?> GetOrderDetailsAsync(string orderId)
        {
            return await _orderRepository.GetOrderDetailsAsync(orderId);
        }
    }
}
