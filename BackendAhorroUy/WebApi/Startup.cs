using BusinessLogic;
using BusinessLogic.Interface;
using BusinessLogicForPushNotification;
using BusinessLogicForPushNotification.Interface;
using DataAccess;
using DataAccessInterface;
using Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Diagnostics.CodeAnalysis;

namespace WebApi
{
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApi", Version = "v1" });
            });

            services.AddDbContext<DbContext, ContextObl>(options => {
                options.UseSqlServer(
                    Configuration.GetConnectionString("AhorroUyDB"));
            }, ServiceLifetime.Scoped);


            services.AddScoped(typeof(IRepository<Category>), typeof(BaseRepository<Category>));
            services.AddScoped(typeof(IRepository<Market>), typeof(BaseRepository<Market>));
            services.AddScoped(typeof(IRepository<Product>), typeof(BaseRepository<Product>));
            services.AddScoped(typeof(IRepository<ProductMarket>), typeof(BaseRepository<ProductMarket>));
            services.AddScoped(typeof(IRepository<User>), typeof(BaseRepository<User>));
            services.AddScoped(typeof(IRepository<UserSession>), typeof(BaseRepository<UserSession>));
            services.AddScoped(typeof(IRepository<Token>), typeof(BaseRepository<Token>));
            services.AddScoped(typeof(IRepository<Coupon>), typeof(BaseRepository<Coupon>));
            services.AddScoped(typeof(IRepository<Purchase>), typeof(BaseRepository<Purchase>));


            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserSessionRepository, UserSessionRepository>();
            services.AddScoped<IProductMarketManagemenet, ProductMarketManagement>();
            services.AddScoped<ICouponManagement, CouponManagement>();
            services.AddScoped<IFavoritesManagement, FavoritesManagement>();
            services.AddScoped<IProductMarketRepository, ProductMarketRepository>();
            services.AddScoped<IUserManagement, UserManagement>();
            services.AddScoped<IUserSessionManagement, UserSessionManagement>();
            services.AddScoped<ICategoryManagement, CategoryManagement>();
            services.AddScoped<IPushNotificationManagement, PushNotificationManagement>();
            services.AddScoped<IPurchaseManagement, PurchaseManagement>();


        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApi v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
