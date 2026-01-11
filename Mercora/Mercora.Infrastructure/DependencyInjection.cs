using Mercora.Application.Abstractions.Persistence;
using Mercora.Application.Orders;
using Mercora.Application.Products;
using Mercora.Infrastructure.Persistence;
using Mercora.Infrastructure.Persistence.Repositories;
using Mercora.Infrastructure.Services.Orders;
using Mercora.Infrastructure.Services.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Mercora.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddMercoraInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<MercoraDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("MercoraDb"));
            });

            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderService, OrderService>();

            return services;
        }
    }
}
