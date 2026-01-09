using Microsoft.EntityFrameworkCore;
using Mercora.Infrastructure.Persistence;
using Mercora.Application.Products;
using Mercora.Infrastructure.Services.Products;
using Mercora.Application.Orders;
using Mercora.Infrastructure.Services.Orders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IOrderService, OrderService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<MercoraDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("MercoraDB"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
app.MapControllers();
app.Run();
