using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RickAndMorty.DataAccess.Models
{
    public class Character
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }
        [Required]
        [MaxLength(200)]
        public string Status { get; set; }
        [Required]
        [MaxLength(200)]
        public string Species { get; set; }
        public string Type { get; set; }
        [Required]
        [MaxLength(200)]
        public string Gender { get; set; }
        public Origin Origin { get; set; }
        public Location Location { get; set; }
        [Required]
        [MaxLength(200)]
        public string Image { get; set; }
        public ICollection<CharacterEpisode> CharacterEpisodes { get; set; }
        [Required]
        [MaxLength(200)]
        public string Url { get; set; }
        [Required]
        [MaxLength(200)]
        public DateTime Created { get; set; }
    }
}
