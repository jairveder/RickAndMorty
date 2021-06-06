using System;
using System.Collections.Generic;

namespace RickAndMorty.WebApi.ViewModels.V1
{
    public class CharacterViewModel
    {
        public int CharacterId { get; set; }
        public string? Name { get; set; }
        public string? Status { get; set; }
        public string? Species { get; set; }
        public string? Type { get; set; }
        public string? Gender { get; set; }
        public PlanetViewModel? Origin { get; set; }
        public PlanetViewModel? Location { get; set; }
        public string? Image { get; set; }
        public List<EpisodeViewModel>? Episodes { get; set; }
        public string? Url { get; set; }
        public DateTime Created { get; set; }
    }
}
