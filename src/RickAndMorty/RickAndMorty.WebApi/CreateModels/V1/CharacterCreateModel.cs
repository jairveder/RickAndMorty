using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RickAndMorty.WebApi.CreateModels.V1
{
    public class CharacterCreateModel
    {
        [Required]
        [MaxLength(200)]
        public string? Name { get; set; }
        [MaxLength(200)]
        public string? Status { get; set; }
        [MaxLength(200)]
        public string? Species { get; set; }
        [MaxLength(200)]
        public string? Type { get; set; }
        [MaxLength(200)]
        public string? Gender { get; set; }
        public PlanetCreateModel? Origin { get; set; }
        public PlanetCreateModel? Location { get; set; }
        [MaxLength(200)]
        public string? Image { get; set; }
        public List<EpisodeCreateModel>? Episodes { get; set; }
        [MaxLength(200)]
        public string? Url { get; set; }
    }
}
