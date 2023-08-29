using CustomerOrder.Cache;
using CustomerOrder.Data;
using CustomerOrder.Models;
using CustomerOrder.Repositories;
using CustomerOrder.Services;
using CustomerOrder.Services.YourAspNetCoreMVCApp.Services;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
builder.Services.AddDbContext<CustomerOrderDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseSqlServer(connectionString);
});
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IValidator<Customer>, CustomerValidator>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IOrderService, OrderService>(); 
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IValidator<Order>, OrderValidator>();
builder.Services.AddSingleton<IMemoryCache,MyMemoryCache>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
