using System.Linq;
using System.Threading.Tasks;
using RickAndMorty.ConsoleApp.Converters;
using RickAndMorty.ConsoleApp.Processors;
using Serilog;

namespace RickAndMorty.ConsoleApp.Facades
{
    public interface IRickAndMortyFacade
    {
        public Task ProcessAsync();
    }

    public class RickAndMortyFacade : IRickAndMortyFacade
    {
        private readonly IApiProcessor _apiProcessor;
        private readonly ICharacterContextConverter _characterConverter;
        private readonly IRickAndMortyDataProcessor _rickAndMortyDataProcessor;

        public RickAndMortyFacade(IApiProcessor apiProcessor, ICharacterContextConverter characterConverter, IRickAndMortyDataProcessor rickAndMortyDataProcessor)
        {
            _apiProcessor = apiProcessor;
            _characterConverter = characterConverter;
            _rickAndMortyDataProcessor = rickAndMortyDataProcessor;
        }

        public async Task ProcessAsync()
        {
            var results = await _apiProcessor.GetCharactersAsync();

            Log.Information($"Filtering out characters that are no longer alive.");
            var aliveCharacters = results.Where(x => x.Status == "Alive").ToList();
            Log.Information($"Remaining characters: {aliveCharacters.Count}.");

            Log.Information($"Converting characters to database models.");
            var characters = aliveCharacters.Select(_characterConverter.Convert).ToList();

            Log.Information($"Clearing out tables of all previous characters.");
            await _rickAndMortyDataProcessor.RemoveAllCharacters();

            Log.Information($"Storing all loaded characters characters.");
            await _rickAndMortyDataProcessor.StoreAsync(characters);
        }
    }
}
