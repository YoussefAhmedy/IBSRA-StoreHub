using IBSRAStore.DTOs;
using Microsoft.Data.SqlClient;
using System.Data;

namespace IBSRAStore.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly string _connectionString;

        public OrderRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") 
                ?? throw new ArgumentException("DefaultConnection string is required");
        }

        public async Task<int> CreateOrderAsync(string orderId, int productId, string customerEmail, 
            string customerName, decimal totalAmount, decimal taxAmount, decimal subtotalAmount,
            string cardNumberLast4, string expiryDate, string address, string city, string zipCode)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("CreateOrder", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@OrderId", orderId);
            command.Parameters.AddWithValue("@ProductId", productId);
            command.Parameters.AddWithValue("@CustomerEmail", customerEmail);
            command.Parameters.AddWithValue("@CustomerName", customerName);
            command.Parameters.AddWithValue("@TotalAmount", totalAmount);
            command.Parameters.AddWithValue("@TaxAmount", taxAmount);
            command.Parameters.AddWithValue("@SubtotalAmount", subtotalAmount);
            command.Parameters.AddWithValue("@CardNumberLast4", cardNumberLast4);
            command.Parameters.AddWithValue("@ExpiryDate", expiryDate);
            command.Parameters.AddWithValue("@Address", address);
            command.Parameters.AddWithValue("@City", city);
            command.Parameters.AddWithValue("@ZipCode", zipCode);

            await connection.OpenAsync();
            var result = await command.ExecuteScalarAsync();
            return Convert.ToInt32(result);
        }

        public async Task<OrderDetailsDto?> GetOrderDetailsAsync(string orderId)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("GetOrderDetails", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@OrderId", orderId);

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new OrderDetailsDto
                {
                    OrderId = reader.GetString("OrderId"),
                    CustomerEmail = reader.GetString("CustomerEmail"),
                    CustomerName = reader.GetString("CustomerName"),
                    TotalAmount = reader.GetDecimal("TotalAmount"),
                    TaxAmount = reader.GetDecimal("TaxAmount"),
                    SubtotalAmount = reader.GetDecimal("SubtotalAmount"),
                    PaymentStatus = reader.GetString("PaymentStatus"),
                    CreatedAt = reader.GetDateTime("CreatedAt"),
                    ProductName = reader.GetString("ProductName"),
                    ProductPrice = reader.GetDecimal("ProductPrice"),
                    ProductImage = reader.GetString("ProductImage"),
                    CardNumberLast4 = reader.GetString("CardNumberLast4"),
                    TransactionId = reader.GetString("TransactionId"),
                    Address = reader.GetString("Address"),
                    City = reader.GetString("City"),
                    ZipCode = reader.GetString("ZipCode")
                };
            }

            return null;
        }
    }
}
