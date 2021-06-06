using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using RickAndMorty.DataAccess.Contexts;

namespace RickAndMorty.Domain.Extensions
{
    public static class DataAccessExtensions
    {
        public static IIncludableQueryable<DataAccess.Models.Character, DataAccess.Models.Episode> GetCharacterQueryable(this CharacterContext characterContext)
        {
            return characterContext.Character
                .Include(character => character.Location)
                .Include(character => character.Origin)
                .Include(character => character.CharacterEpisodes)
                .ThenInclude(characterEpisode => characterEpisode.Episode);
        }
    }
}
