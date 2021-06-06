using Microsoft.EntityFrameworkCore;
using RickAndMorty.Domain.Caches;
using RickAndMorty.Domain.Converters;
using RickAndMorty.Domain.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RickAndMorty.DataAccess.Contexts;
using RickAndMorty.Domain.Extensions;
using Serilog;

namespace RickAndMorty.Domain.Processors
{
    public interface IRickAndMortyProcessor
    {
        public Task<GetCharacters> GetAllCharactersAsync();
        public Task<GetCharacters?> GetCharactersByPlanetAsync(string planet);
        public Task<bool> CreateAsync(Character character);
    }


    public class RickAndMortyProcessor : IRickAndMortyProcessor
    {
        private readonly CharacterContext _characterContext;
        private readonly IFlushableMemoryCache _memoryCache;
        private readonly IDataAccessCharacterConverter _dataAccessCharacterConverter;

        private const string GetAllCacheKey = "GetAll";
        private const string GetByPlanetCacheKey = "GetByPlanet-";

        public RickAndMortyProcessor(CharacterContext characterContext, 
            IFlushableMemoryCache memoryCache, 
            IDataAccessCharacterConverter dataAccessCharacterConverter)
        {
            _characterContext = characterContext;
            _memoryCache = memoryCache;
            _dataAccessCharacterConverter = dataAccessCharacterConverter;
        }

        public async Task<GetCharacters> GetAllCharactersAsync()
        {
            Log.Information("Attempting to load characters from general cache.");
            if (_memoryCache.TryGetValue(GetAllCacheKey, out List<Character> characters))
            {
                Log.Information("Characters found in cache. Returning characters.");
                return new GetCharacters
                {
                    FromDatabase = false,
                    Characters = characters
                };
            }

            Log.Information("Loading characters from the database.");
            var dataAccessCharacters = await _characterContext.GetCharacterQueryable().ToListAsync();
            characters = dataAccessCharacters.Select(_dataAccessCharacterConverter.Convert).ToList();

            Log.Information("Storing characters in the general cache.");
            _memoryCache.Set(GetAllCacheKey, characters);

            return new GetCharacters
            {
                FromDatabase = true,
                Characters = characters
            };
        }

        public async Task<GetCharacters?> GetCharactersByPlanetAsync(string planet)
        {
            if (string.IsNullOrEmpty(planet))
            {
                Log.Error("Planet is null or empty. Not able to search.");
                return null;
            }

            Log.Information($"Attempting to load planet {planet} from the planet cache.");
            var planetCacheKey = $"{GetByPlanetCacheKey}{planet}";
            if (_memoryCache.TryGetValue(planetCacheKey, out List<Character> characters))
            {
                Log.Information($"Planet {planet} found in the planet cache.");
                return new GetCharacters
                {
                    FromDatabase = false,
                    Characters = characters
                };
            }

            Log.Information("Attempting to load characters from general cache.");
            if (_memoryCache.TryGetValue(GetAllCacheKey, out characters))
            {
                Log.Information("Characters found in cache. Returning characters.");
                var filtered = characters.Where(x => x.Location != null && planet.Equals(x.Location.Name)).ToList();
                return new GetCharacters
                {
                    FromDatabase = false,
                    Characters = filtered
                };
            }

            Log.Information($"Loading characters for planet {planet} from the database.");
            var dataAccessCharacters = await _characterContext.GetCharacterQueryable().Where(x => x.Location.Name.Equals(planet)).ToListAsync();
            characters = dataAccessCharacters.Select(_dataAccessCharacterConverter.Convert).ToList();

            Log.Information($"Storing characters for planet {planet} in the planet cache.");
            _memoryCache.Set(planetCacheKey, characters);

            return new GetCharacters
            {
                FromDatabase = true,
                Characters = characters
            };
        }

        public async Task<bool> CreateAsync(Character character)
        {
            Log.Information("Converting characters to database entities.");
            var dataAccessCharacter = await _dataAccessCharacterConverter.ConvertAsync(character);

            await _characterContext.AddAsync(dataAccessCharacter);
            await _characterContext.SaveChangesAsync();

            _memoryCache.Flush();

            return true;
        }
    }
}
