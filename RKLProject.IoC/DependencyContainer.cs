using Microsoft.Extensions.DependencyInjection;
using RKLProject.Core.IServices;
using RKLProject.Core.MongoDbServices.IServices;
using RKLProject.Core.MongoDbServices.Services;
using RKLProject.Core.Services;
using RKLProject.DataLayer.Implementations;
using RKLProject.DataLayer.Interfaces;


namespace RKLProject.IoC
{
    public class DependencyContainer
    {
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddTransient<IUserService , UserService>();
            services.AddTransient<IFormService , FormService>();
            services.AddTransient<IOrganizationService , OrganizationService>();
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddTransient(typeof(IMongoFormService<>), typeof(MongoFormService<>));
            services.AddTransient<IUnitOfWork , UnitOfWork>();
        }
    }
}
