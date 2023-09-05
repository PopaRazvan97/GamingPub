using BusinessLayer.Services;
using DataAccessLayer;
using DataAccessLayer.Repositories;

namespace GamingPubReservations.Settings
{
    public static class Dependencies
    {
        public static void Inject(WebApplicationBuilder applicationBuilder)
        {
            applicationBuilder.Services.AddControllers();
            applicationBuilder.Services.AddSwaggerGen();

            applicationBuilder.Services.AddDbContext<AppDbContext>();

            AddRepositories(applicationBuilder.Services);
            AddServices(applicationBuilder.Services);
        }

        private static void AddServices(IServiceCollection services)
        {
            services.AddScoped<AuthorizationService>();
            services.AddScoped<UserService>();
            services.AddScoped<ReservationService>();
            services.AddScoped<GamingPubService>();
            services.AddScoped<ScheduleService>();
            services.AddScoped<GamingPlatformService>();
        }

        private static void AddRepositories(IServiceCollection services)
        {
            services.AddScoped<UsersRepository>();
            services.AddScoped<GamingPubsRepository>();
            services.AddScoped<ReservationsRepository>();
            services.AddScoped<AddressRepository>();
            services.AddScoped<ScheduleRepository>();
            services.AddScoped<GamingPlatformRepository>();
            services.AddScoped<UnitOfWork>();
        }
    }
}