using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RickAndMorty.DataAccess.Models
{
    public class Character
    {
        public int CharacterId { get; set; }
        [MaxLength(200)]
        public string Name { get; set; }
        [MaxLength(200)]
        public string Status { get; set; }
        [MaxLength(200)]
        public string Species { get; set; }
        [MaxLength(200)]
        public string Type { get; set; }
        [MaxLength(200)]
        public string Gender { get; set; }
        public Planet Origin { get; set; }
        public Planet Location { get; set; }
        [MaxLength(200)]
        public string Image { get; set; }
        public ICollection<CharacterEpisode> CharacterEpisodes { get; set; }
        [MaxLength(200)]
        public string Url { get; set; }
        public DateTime Created { get; set; }
    }
}
