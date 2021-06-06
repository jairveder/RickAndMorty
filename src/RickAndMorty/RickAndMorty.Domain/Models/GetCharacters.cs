using System.Collections.Generic;

namespace RickAndMorty.Domain.Models
{
    public class GetCharacters
    {
        public bool FromDatabase { get; set; }
        public List<Character>? Characters { get; set; }
    }
}
