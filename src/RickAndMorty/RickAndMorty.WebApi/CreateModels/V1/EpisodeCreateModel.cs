using System.ComponentModel.DataAnnotations;

namespace RickAndMorty.WebApi.CreateModels.V1
{
    public class EpisodeCreateModel
    {
        [MaxLength(200)]
        [Required]
        public string? FullName { get; set; }
    }
}
