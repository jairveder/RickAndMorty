using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.IO;
using System.Threading.Tasks;
using RickAndMorty.ConsoleApp.Converters;
using RickAndMorty.ConsoleApp.Facades;
using RickAndMorty.ConsoleApp.Processors;

namespace RickAndMorty.ConsoleApp
{
    class Program
    {
        private static IConfigurationRoot? _configuration;

        static int Main()
        {
            Log.Logger = new LoggerConfiguration()
                 .WriteTo.Console(Serilog.Events.LogEventLevel.Information)
                 .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
                 .MinimumLevel.Debug()
                 .Enrich.FromLogContext()
                 .CreateLogger();

            try
            {
                MainAsync().Wait();
                return 0;
            }
            catch
            {
                return 1;
            }
        }

        static async Task MainAsync()
        {
            Log.Information("Creating service collection");
            ServiceCollection serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            Log.Information("Building service provider");
            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            Log.Information($"ConnectionString is: {_configuration.GetConnectionString("Default")}");

            try
            {
                Log.Information("Starting service");
                var rickAndMortyFacade = serviceProvider.GetRequiredService<IRickAndMortyFacade>();
                await rickAndMortyFacade.ProcessAsync();
                Log.Information("Ending service");
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, $"Error running service {ex.Message}");
                throw;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient();
            
            services.AddSingleton(LoggerFactory.Create(builder =>
            {
                builder.AddSerilog(dispose: true);
            }));

            services.AddLogging();

            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddJsonFile("appsettings.json", false)
                .Build();

            services.AddSingleton(_configuration);

            #region converters
            services.AddTransient<ICharacterContextConverter, CharacterContextConverter>();
            #endregion
            #region facades
            services.AddTransient<IRickAndMortyFacade, RickAndMortyFacade>();
            #endregion
            #region processors
            services.AddTransient<IApiProcessor, ApiProcessor>();
            services.AddTransient<IRickAndMortyDataProcessor, RickAndMortyDataProcessor>();
            #endregion

            services.AddDataAccessModule(_configuration);
        }
    }
}
