using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using RickAndMorty.DataAccess.Contexts;

namespace RickAndMorty.DataAccess
{
    public class CharacterContextFactory : IDesignTimeDbContextFactory<CharacterContext>
    {
        private const string ConnectionString = "";

        public CharacterContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CharacterContext>();
            optionsBuilder.UseMySql(ConnectionString, new MySqlServerVersion(new Version(10, 1, 40)), mySqlOptions => mySqlOptions.CharSetBehavior(CharSetBehavior.NeverAppend));
            return new CharacterContext(optionsBuilder.Options);
        }
    }

}
