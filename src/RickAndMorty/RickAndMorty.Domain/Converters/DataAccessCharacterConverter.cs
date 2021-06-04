using RickAndMorty.Domain.Models;
using System.Linq;

namespace RickAndMorty.Domain.Converters
{
    public interface IDataAccessCharacterConverter
    {
        public Character Convert(DataAccess.Models.Character dataAccessCharacter);
    }

    public class DataAccessCharacterConverter : IDataAccessCharacterConverter
    {
        public Character Convert(DataAccess.Models.Character dataAccessCharacter)
        {
            var domainCharacter = new Character
            {
                Id = dataAccessCharacter.Id,
                Name = dataAccessCharacter.Name,
                Status = dataAccessCharacter.Status,
                Species = dataAccessCharacter.Species,
                Type = dataAccessCharacter.Type,
                Gender = dataAccessCharacter.Gender,
                Origin = dataAccessCharacter.Origin == null ?
                    null :
                    new Origin
                    {
                        Id = dataAccessCharacter.Origin.Id,
                        Name = dataAccessCharacter.Origin.Name,
                        Url = dataAccessCharacter.Origin.Url,
                    },
                Location = dataAccessCharacter.Location == null ?
                    null :
                    new Location
                    {
                        Id = dataAccessCharacter.Location.Id,
                        Name = dataAccessCharacter.Location.Name,
                        Url = dataAccessCharacter.Location.Url,
                    },
                Image = dataAccessCharacter.Image,
                Episodes = dataAccessCharacter.CharacterEpisodes.Select(x =>
                    new Episode
                    {
                        Id = x.EpisodeId,
                        FullName = x.Episode.FullName
                    }
                ).ToList(),
                Url = dataAccessCharacter.Url,
                Created = dataAccessCharacter.Created,
            };
            return domainCharacter;
        }
    }
}
