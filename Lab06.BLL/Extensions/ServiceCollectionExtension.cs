using Lab06.BLL.Services;
using Lab06.BLL.Services.Interfaces;
using Lab06.DAL.Repositories;
using Lab06.DAL.Repositories.Interfaces;
using Lab06.DAL.Uow;
using Microsoft.Extensions.DependencyInjection;

namespace Lab06.BLL.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddBllServices(this IServiceCollection services)
        {
            services.AddScoped<IParkAttractionsService, ParkAttractionsService>();
            services.AddScoped<IUserTicketService, UserTicketService>();
            services.AddScoped<IUserService, UserService>();
            return services;
        }

        public static IServiceCollection AddDalServices(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            return services;
        }
    }
}
