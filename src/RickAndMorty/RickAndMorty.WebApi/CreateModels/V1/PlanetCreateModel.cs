using System.ComponentModel.DataAnnotations;

namespace RickAndMorty.WebApi.CreateModels.V1
{
    public class PlanetCreateModel
    {
        [Required]
        [MaxLength(200)]
        public string? Name { get; set; }
        [Required]
        [MaxLength(200)]
        public string? Url { get; set; }
    }
}
