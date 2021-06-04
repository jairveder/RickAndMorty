using Microsoft.EntityFrameworkCore;
using RickAndMorty.DataAccess;
using RickAndMorty.Domain.Caches;
using RickAndMorty.Domain.Converters;
using RickAndMorty.Domain.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RickAndMorty.Domain.Processors
{
    public interface IRickAndMortyProcessor
    {
        public Task<GetCharacters> GetAllCharactersAsync();
        public Task<GetCharacters> GetCharactersByPlanetAsync(string planet);
    }


    public class RickAndMortyProcessor : IRickAndMortyProcessor
    {
        private readonly CharacterContext _characterContext;
        private readonly IFlushableMemoryCache _memoryCache;
        private readonly IDataAccessCharacterConverter _dataAccessCharacterConverter;

        private static string GetAllCacheKey = "GetAll";
        private static string GetByPlanetCacheKey = "GetByPlanet";

        public RickAndMortyProcessor(CharacterContext characterContext, IFlushableMemoryCache memoryCache, IDataAccessCharacterConverter dataAccessCharacterConverter)
        {
            _characterContext = characterContext;
            _memoryCache = memoryCache;
            _dataAccessCharacterConverter = dataAccessCharacterConverter;
        }

        public async Task<GetCharacters> GetAllCharactersAsync()
        {
            if (_memoryCache.TryGetValue(GetAllCacheKey, out List<Character> characters))
            {
                return new GetCharacters
                {
                    FromDatabase = false,
                    Characters = characters
                };
            }

            var dataAccessCharacters = await _characterContext.Character.ToListAsync();
            characters = dataAccessCharacters.Select(_dataAccessCharacterConverter.Convert).ToList();

            _memoryCache.Set(GetAllCacheKey, characters);

            return new GetCharacters
            {
                FromDatabase = true,
                Characters = characters
            };
        }

        public async Task<GetCharacters> GetCharactersByPlanetAsync(string planet)
        {
            var response = new GetCharacters();
            List<Character> characters;

            if (string.IsNullOrEmpty(planet))
                return null;

            var planetCacheKey = $"{GetByPlanetCacheKey}-{planet}";
            if (_memoryCache.TryGetValue(planetCacheKey, out characters))
            {
                return new GetCharacters
                {
                    FromDatabase = false,
                    Characters = characters
                };
            }

            if (_memoryCache.TryGetValue(GetAllCacheKey, out characters))
            {
                var filtered = characters.Where(x => planet.Equals(x.Origin.Name)).ToList();
                return new GetCharacters
                {
                    FromDatabase = false,
                    Characters = filtered
                };
            }

            var dataAccessCharacters = await _characterContext.Character.Where(x => x.Origin.Name.Equals(planet)).ToListAsync();
            characters = dataAccessCharacters.Select(_dataAccessCharacterConverter.Convert).ToList();

            _memoryCache.Set(planetCacheKey, characters);

            return new GetCharacters
            {
                FromDatabase = true,
                Characters = characters
            };
        }
    }
}
