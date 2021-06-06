using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RickAndMorty.ConsoleApp.Models
{
    public class Root
    {
        public Info? Info { get; set; }
        [JsonPropertyName("Results")]
        public List<Character>? Characters { get; set; }
    }
}
