using IBSRAStore.Models;
using Microsoft.Data.SqlClient;
using System.Text.Json;
using System.Data;

namespace IBSRAStore.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly string _connectionString;

        public ProductRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") 
                ?? throw new ArgumentException("DefaultConnection string is required");
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            var products = new List<Product>();

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("GetAllProducts", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                products.Add(new Product
                {
                    Id = reader.GetInt32("Id"),
                    Name = reader.GetString("Name"),
                    Price = reader.GetDecimal("Price"),
                    OriginalPrice = reader.GetDecimal("OriginalPrice"),
                    ImageUrl = reader.GetString("ImageUrl"),
                    Rating = reader.GetDecimal("Rating"),
                    ReviewCount = reader.GetInt32("ReviewCount"),
                    Features = reader.GetString("Features"),
                    CreatedAt = reader.GetDateTime("CreatedAt"),
                    UpdatedAt = reader.GetDateTime("UpdatedAt")
                });
            }

            return products;
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("GetProductById", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@ProductId", id);

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new Product
                {
                    Id = reader.GetInt32("Id"),
                    Name = reader.GetString("Name"),
                    Price = reader.GetDecimal("Price"),
                    OriginalPrice = reader.GetDecimal("OriginalPrice"),
                    ImageUrl = reader.GetString("ImageUrl"),
                    Rating = reader.GetDecimal("Rating"),
                    ReviewCount = reader.GetInt32("ReviewCount"),
                    Features = reader.GetString("Features"),
                    CreatedAt = reader.GetDateTime("CreatedAt"),
                    UpdatedAt = reader.GetDateTime("UpdatedAt")
                };
            }

            return null;
        }
    }
}
