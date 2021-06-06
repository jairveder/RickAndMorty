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

            #region caches
            services.AddSingleton<IFlushableMemoryCache, FlushableMemoryCache>();
            #endregion
            #region converters
            services.AddTransient<IDataAccessCharacterConverter, DataAccessCharacterConverter>();
            #endregion
            #region processors
            services.AddTransient<IRickAndMortyProcessor, RickAndMortyProcessor>();
            #endregion
        }
    }
}
