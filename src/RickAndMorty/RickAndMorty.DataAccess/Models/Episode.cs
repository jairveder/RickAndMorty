using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RickAndMorty.DataAccess.Models
{
    public class Episode
    {
        public int EpisodeId { get; set; }
        [MaxLength(200)]
        public string FullName { get; set; }
        public ICollection<CharacterEpisode> CharacterEpisodes { get; set; }
    }
}
