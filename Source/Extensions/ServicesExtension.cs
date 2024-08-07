namespace Comanda.WebApi.Extensions;

public static class ServicesExtension
{
    public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers()
            .AddJsonOptions(options =>
            {
                /*
                    The following line is added to prevent issues related to circular references
                    during JSON serialization. It instructs the JsonSerializer to ignore cycles.
                */
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });

        services.ConfigureSwagger();

        services.AddDataPersistence(configuration);
        services.ConfigureIdentity();

        services.AddMediator();
        services.AddValidation();
        services.AddMapping();

        services.AddJwtBearer(configuration);
        services.AddApplicationServices(configuration);
    }
}