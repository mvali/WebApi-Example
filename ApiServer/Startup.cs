using Entities.Contracts;
using LoggerService;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using NLog;
using Repository;
using Repository.DbData;
using System;

namespace ApiServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            // configure NLog
            LogManager.LoadConfiguration(System.String.Concat(System.IO.Directory.GetCurrentDirectory(), "/nlog.config"));
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.

        // Register service lifetime:
        //  - AddSingleton - same for every request - All components are sharing the same service every time they need it.
        //  - AddScoped    - created once per client request - Whenever we send the HTTP request towards the application, a new instance of the service is created
        //  - Transient    - new instance created every time - If during one request on our application, 
        //                              multiple components need the service, this service will be created again for every single component
        public void ConfigureServices(IServiceCollection services)
        {
            //2. use sql server with EntityFramework
            services.AddDbContext<SqlContext>(opt => opt.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            //2.1 Map the connection configuration - without EntityFramework
            //services.Configure<ConnectionStrings>(Configuration.GetSection("ConnectionStrings"));

            // configure NLOg
            StartupConfig.ConfigureLoggerService(services);

            //4. Add Newtonsoft Json serialization .core V5
            services.AddControllers().AddNewtonsoftJson(s=>{s.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();});

            //1. register data dependency injection - MockCommanderRepo contains fake data, replace MockCommanderRepo with data from db and will be fine
            //1. services.AddScoped<IAlimentRepository, AlimentMoq>();

            //3. used to map SqlModel with ViewModel
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            //2. register data dependency injection - use Sql database data
            services.AddScoped<IAlimentRepository, AlimentRepository>();
            services.AddScoped<IAlimentRepositoryAsync, AlimentRepository>();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ApiServer", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiServer v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

    }
}
