using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace RickAndMorty.DataAccess.Models
{
    [Index(nameof(Name))]
    public class Planet
    {
        public int PlanetId { get; set; }
        [MaxLength(200)]
        public string Name { get; set; }
        [MaxLength(200)]
        public string Url { get; set; }
    }
}
