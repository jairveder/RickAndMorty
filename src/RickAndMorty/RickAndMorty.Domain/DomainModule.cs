using Microsoft.Extensions.Configuration;
using RickAndMorty.Domain.Caches;
using RickAndMorty.Domain.Converters;
using RickAndMorty.Domain.Processors;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DomainModule
    {
        public static void AddDomainModule(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMemoryCache();
            services.AddDataAccessModule(configuration);

            services.AddSingleton<IFlushableMemoryCache, FlushableMemoryCache>();

            services.AddTransient<IRickAndMortyProcessor, RickAndMortyProcessor>();

            services.AddTransient<IDataAccessCharacterConverter, DataAccessCharacterConverter>();
            
        }
    }
}
