using ApiServer.Controllers;
using Entities.Contracts;
using LoggerService;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using NLog;
using Repository;
using Repository.DbData;
using Repository.Moq;
using Swashbuckle.AspNetCore.SwaggerGen;
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
            // does not behave like normal relational Db
            //services.AddDbContext<SqlContext>(opt => opt.UseInMemoryDatabase(databaseName: "ApiServerDb"), ServiceLifetime.Scoped, ServiceLifetime.Scoped);

            /*// SqlLite InMemory database - you must recreate tables, fill data each time as Db gets distroyed when app is closed
            var connectionString = "DataSource=myshareddb;mode=memory;cache=shared";
            var keepAliveConnection = new SqliteConnection(connectionString);
            keepAliveConnection.Open();
            services.AddDbContext<SqlContext>(options => { options.UseSqlite(connectionString); });*/

            //2.1 Map the connection configuration - without EntityFramework
            //services.Configure<ConnectionStrings>(Configuration.GetSection("ConnectionStrings"));

            // configure NLOg
            StartupConfig.ConfigureLoggerService(services);

            //4. Add Newtonsoft Json serialization .core V5
            //services.AddControllers().AddNewtonsoftJson(s=>{s.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();});

            //1. register data dependency injection - MockCommanderRepo contains fake data, replace MockCommanderRepo with data from db and will be fine
            //1. services.AddScoped<IAlimentRepository, AlimentMoq>();

            //3. used to map SqlModel with ViewModel
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            //2. register data dependency injection - use Sql database data
            services.AddScoped<IAlimentRepository, AlimentRepository>();
            services.AddScoped<IAlimentRepositoryAsync, AlimentRepository>();
            services.AddScoped<ICartService, CartServiceMoq>();
            services.AddScoped<IPaymentService, PaymentServiceMoq>();
            services.AddScoped<IShipmentService, ShipmentServiceMoq>();

            // add ApiExplorer for more detailing the api description (like swagger but more restrictive)
            ApiDescription.ApiExplorer.AddApiExplorerServices(services);

            services.AddControllers().AddNewtonsoftJson(s => { s.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver(); });
            //services.AddControllers();
            services.AddMvc();
            
            // for creating different support for versioning
            services.AddApiVersioning(options => {
                options.AssumeDefaultVersionWhenUnspecified = true;
                //options.DefaultApiVersion = new ApiVersion(1, 1); // translate to 1.1 = major:1 minor:1
                options.DefaultApiVersion = ApiVersion.Default;
                
                //options.ApiVersionReader = new MediaTypeApiVersionReader("version"); // mapped to "version" word in headed "Accept" value  (key:Accept value:application/json;version=2.0)
                //options.ApiVersionReader = new HeaderApiVersionReader("X-Version"); // Asks for key:X-Value and value:2.0
                options.ApiVersionReader = ApiVersionReader.Combine(
                    new MediaTypeApiVersionReader("version"),
                    new HeaderApiVersionReader("X-Version")
                ); // use both methods at the same time */

                // without decorating the controller "CartController"
                options.Conventions.Controller<CartController>().HasDeprecatedApiVersion(new ApiVersion(1, 0));
                options.Conventions.Controller<CartController>().HasApiVersion(new ApiVersion(2, 0));

                options.ReportApiVersions = true;
            });


            services.AddVersionedApiExplorer(options =>
            {
                // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service  
                // note: the specified format code will format the version as "'v'major[.minor][-status]"  
                options.GroupNameFormat = "'v'VVV";

                // note: this option is only necessary when versioning by url segment. the SubstitutionFormat  
                // can also be used to control the format of the API version in route templates  
                options.SubstituteApiVersionInUrl = true;
            });
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, Logic.SwaggerOptionsConfigure>();

            services.AddSwaggerGen(options => {
                options.OperationFilter<Logic.SwaggerDefaultValues>();
                options.SchemaFilter<Logic.SwaggerExcludePropertySchemaFilter>();
            });

            /*services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ApiServer", Version = "v1" });
            });*/
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();               // swagger it's based on mvc component: ApiExplorer
                                                //app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiServer v1"));
                app.UseSwaggerUI(options =>
                {// build a swagger endpoint for each discovered API version  
                    foreach (var description in provider.ApiVersionDescriptions)
                    {
                        options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                        options.DefaultModelExpandDepth(1);
                        options.DefaultModelsExpandDepth(-1);
                        options.DefaultModelRendering(Swashbuckle.AspNetCore.SwaggerUI.ModelRendering.Example);
                        options.EnableFilter();
                    }
                });

                /*
                // seeding InMemory data - Only allowed for Singleton services
                // Singleton services can be resolved by constructor injection in Middleware
                //var context = app.ApplicationServices.GetService<SqlContext>();
                //new AlimentMoq().AddTestData(context);

                using (var scope = app.ApplicationServices.CreateScope())
                {
                    var _sqlContext = scope.ServiceProvider.GetRequiredService<SqlContext>();
                    new AlimentMoq().AddTestData(_sqlContext);
                }*/
            }
            else
            {
                //app.UseSwagger();               // swagger it's based on mvc component: ApiExplorer
                //app.UseSwaggerUI(c => c.SwaggerEndpoint("/documentation", "ApiServer v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                // for MVC part of ApiExplorer
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Documentation}/{action=Index}/{id?}");
            });
        }

    }
}
