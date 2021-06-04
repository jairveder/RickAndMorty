using System.ComponentModel.DataAnnotations;

namespace RickAndMorty.DataAccess.Models
{
    public class Origin
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
