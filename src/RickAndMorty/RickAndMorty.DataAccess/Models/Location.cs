using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace RickAndMorty.DataAccess.Models
{
    [Index(nameof(Name))]
    public class Location
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }
        [Required]
        [MaxLength(200)]
        public string Url { get; set; }
    }
}
