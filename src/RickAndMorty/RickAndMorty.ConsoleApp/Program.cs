using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.IO;
using System.Threading.Tasks;

namespace RickAndMorty.ConsoleApp
{
    class Program
    {
        public static IConfigurationRoot configuration;

        static int Main(string[] args)
        {
            // Initialize serilog logger
            Log.Logger = new LoggerConfiguration()
                 .WriteTo.Console(Serilog.Events.LogEventLevel.Information)
                 .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
                 .MinimumLevel.Debug()
                 .Enrich.FromLogContext()
                 .CreateLogger();

            try
            {
                MainAsync(args).Wait();
                return 0;
            }
            catch
            {
                return 1;
            }
        }

        static async Task MainAsync(string[] args)
        {
            // Create service collection
            Log.Information("Creating service collection");
            ServiceCollection serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            // Create service provider
            Log.Information("Building service provider");
            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            // Print connection string to demonstrate configuration object is populated
            Log.Information(configuration.GetConnectionString("Default"));

            try
            {
                Log.Information("Starting service");
                var rickAndMortyFacade = serviceProvider.GetRequiredService<IRickAndMortyFacade>();
                await rickAndMortyFacade.ProcessAsync();
                Log.Information("Ending service");
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Error running service");
                throw ex;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            // Add httpClient
            services.AddHttpClient();

            // Add logging
            services.AddSingleton(LoggerFactory.Create(builder =>
            {
                builder.AddSerilog(dispose: true);
            }));

            services.AddLogging();

            // Build configuration
            configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddJsonFile("appsettings.json", false)
                .Build();

            // Add access to generic IConfigurationRoot
            services.AddSingleton(configuration);

            // Add services
            services.AddTransient<ICharacterContextConverter, CharacterContextConverter>();
            services.AddTransient<IApiProcessor, ApiProcessor>();
            services.AddTransient<IRickAndMortyFacade, RickAndMortyFacade>();
            services.AddTransient<IRickAndMortyDataProcessor, RickAndMortyDataProcessor>();
           

            // Add module
            services.AddDataAccessModule(configuration);
        }
    }
}
