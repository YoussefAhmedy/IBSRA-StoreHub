using IBSRAStore.Repository;
using IBSRAStore.Services;

namespace IBSRAStore.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Register repositories
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();

            // Register services
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IPaymentService, PaymentService>();

            return services;
        }
    }
}
