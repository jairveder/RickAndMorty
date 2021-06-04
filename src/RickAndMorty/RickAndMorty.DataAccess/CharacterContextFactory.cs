using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using RickAndMorty.DataAccess;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public class CharacterContextFactory : IDesignTimeDbContextFactory<CharacterContext>
    {
        public CharacterContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CharacterContext>();
            optionsBuilder.UseMySql("database=rickAndMorty;server=localhost;user id=newuser;Password=password", new MySqlServerVersion(new Version(10, 1, 40)), mySqlOptions => mySqlOptions.CharSetBehavior(CharSetBehavior.NeverAppend));

            return new CharacterContext(optionsBuilder.Options);
        }
    }

}
