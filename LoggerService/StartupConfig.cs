using Microsoft.Extensions.DependencyInjection;

namespace LoggerService
{
    public static class StartupConfig
    {
        // configure NLog
        // services.AddSingleton -> creates service on first request and then every subsequent request is calling the same instance of the service.
        //                          All components are sharing the same service every time they need it.
        // services.AddScoped -> creates service once per request. Whenever we send the HTTP request towards the application, a new instance of the service is created
        // services.AddTransient -> creates the service each time the application request it. If during one request on our application, 
        //                              multiple components need the service, this service will be created again for every single component which needs it
        public static void ConfigureLoggerService(this IServiceCollection services)
        {
            // or just add below line in: ConfigureServices(IServiceCollection services)
            //                    before: services.AddControllers()
            services.AddSingleton<ILoggerManager, LoggerManager>();
        }
    }
}
