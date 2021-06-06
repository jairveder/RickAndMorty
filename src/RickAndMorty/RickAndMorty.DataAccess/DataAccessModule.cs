using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using System;
using RickAndMorty.DataAccess.Contexts;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DataAccessModule
    {
        public static void AddDataAccessModule(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Default");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentException("ConnectionString is null or empty. Unable to proceed.");
            }

            services.AddDbContext<CharacterContext>(options =>
                options.UseMySql(connectionString, new MySqlServerVersion(new Version(10, 1, 40)), mySqlOptions => mySqlOptions.CharSetBehavior(CharSetBehavior.NeverAppend)));
        }
    }
}
