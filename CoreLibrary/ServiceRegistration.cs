
using CoreLibrary.Features;
using CoreLibrary.Interface.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Microservice.core;

public static class ServiceRegistration
{
    public static void AddCoreLayer(this IServiceCollection services)
    {
        services.AddTransient<IAuthService, AuthService>();
        services.AddTransient<ICategoryService, CategoryService>();
        services.AddTransient<ICustomerService, CustomerService>();
        services.AddTransient<IEmployeeService, EmployeeService>();
        services.AddTransient<IOrderService, OrderService>();
        services.AddTransient<ISupplierService, SupplierService>();
        services.AddTransient<IProductService, ProductService>();

        services.AddAutoMapper((cfg) => { }, AppDomain.CurrentDomain.GetAssemblies());
    }
}