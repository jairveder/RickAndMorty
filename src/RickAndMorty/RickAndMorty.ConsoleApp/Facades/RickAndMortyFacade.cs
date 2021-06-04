using Serilog;
using System.Linq;
using System.Threading.Tasks;

namespace RickAndMorty.ConsoleApp
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

            var aliveCharacters = results.Where(x => x.Status == "Alive").ToList();

            Log.Information($"Filtering out characters that are no longer alive. Remaining characters: {aliveCharacters.Count}");
            var characters = aliveCharacters.Select(_characterConverter.Convert).ToList();

            Log.Information($"Clearing out tables of all previous characters.");
            await _rickAndMortyDataProcessor.RemoveAllCharacters();

            Log.Information($"Storing all loaded characters characters.");
            await _rickAndMortyDataProcessor.StoreAsync(characters);
        }
    }
}
