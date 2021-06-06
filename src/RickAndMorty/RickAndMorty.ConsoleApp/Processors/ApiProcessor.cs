using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using RickAndMorty.ConsoleApp.Models;
using Serilog;

namespace RickAndMorty.ConsoleApp.Processors
{
    public interface IApiProcessor
    {
        public Task<List<Character>> GetCharactersAsync();
    }

    public class ApiProcessor : IApiProcessor
    {
        private readonly IHttpClientFactory _clientFactory;
        private static readonly string RickAndMortyUrl = "https://rickandmortyapi.com/api/character/";

        public ApiProcessor(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<List<Character>> GetCharactersAsync()
        {
            var characters = new List<Character>();
            var nextUrl = RickAndMortyUrl;
            var httpClient = _clientFactory.CreateClient();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };


            Log.Information("Initiating loading of rick and morty characters.\n");

            var counter = 1;

            do
            {
                await httpClient.GetAsync(nextUrl)
                    .ContinueWith(async (jobSearchTask) =>
                    {
                        var response = await jobSearchTask;
                        if (response.IsSuccessStatusCode)
                        {
                            var jsonString = await response.Content.ReadAsStringAsync();
                            var result = JsonSerializer.Deserialize<Root>(jsonString, options);
                            if (result != null)
                            {
                                // Build the full list to return later after the loop.
                                if (result.Characters != null && result.Characters.Any())
                                    characters.AddRange(result.Characters.ToList());

                                Log.Information($"Loading completed for page {counter} out of {result.Info?.Pages}.");
            
                                // Get the URL for the next page
                                nextUrl = result.Info?.Next ?? string.Empty;

                                // increment counter;
                                counter++;
                            }
                            
                        }
                        else
                        {
                            // End loop if we get an error response.
                            nextUrl = string.Empty;
                            Log.Error($"Rick and morty api has failed. StatusCode {response.StatusCode} and reason {response.ReasonPhrase}");
                            throw new System.Exception($"Rick and morty api has failed. StatusCode {response.StatusCode} and reason {response.ReasonPhrase}");
                        }
                    });

            } while (!string.IsNullOrEmpty(nextUrl));

            Log.Information($"\nLoading of rick and morty completed.Total characters {characters.Count}");

            return characters;
        }
    }
}
