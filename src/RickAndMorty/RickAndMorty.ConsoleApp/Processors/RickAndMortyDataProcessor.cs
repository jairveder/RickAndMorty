using Microsoft.EntityFrameworkCore;
using RickAndMorty.DataAccess;
using RickAndMorty.DataAccess.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RickAndMorty.ConsoleApp
{
    public interface IRickAndMortyDataProcessor
    {
        public Task RemoveAllCharacters();
        public Task StoreAsync(List<Character> characters);
    }

    public class RickAndMortyDataProcessor : IRickAndMortyDataProcessor
    {
        private readonly CharacterContext _characterContext;

        public RickAndMortyDataProcessor(CharacterContext characterContext)
        {
            _characterContext = characterContext;
        }

        public async Task StoreAsync(List<Character> characters)
        {
            await _characterContext.AddRangeAsync(characters);
            await _characterContext.SaveChangesAsync();
        }

        public async Task RemoveAllCharacters()
        {
            using var transactionContext = await _characterContext.Database.BeginTransactionAsync();
            await _characterContext.Database.ExecuteSqlRawAsync("SET FOREIGN_KEY_CHECKS = 0;");
            foreach (var tableName in CharacterContext.Tables)
            {
                await _characterContext.Database.ExecuteSqlRawAsync($"TRUNCATE TABLE `{tableName}`;");
            }
            await _characterContext.Database.ExecuteSqlRawAsync("SET FOREIGN_KEY_CHECKS = 1;");
            await transactionContext.CommitAsync();
        }
    }

}
