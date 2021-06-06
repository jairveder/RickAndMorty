using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RickAndMorty.DataAccess.Contexts;
using RickAndMorty.DataAccess.Models;
using Character = RickAndMorty.Domain.Models.Character;
using Episode = RickAndMorty.Domain.Models.Episode;
using Planet = RickAndMorty.Domain.Models.Planet;

namespace RickAndMorty.Domain.Converters
{
    public interface IDataAccessCharacterConverter
    {
        public Character Convert(DataAccess.Models.Character dataAccessCharacter);
        public Task<DataAccess.Models.Character> ConvertAsync(Character character);
    }

    public class DataAccessCharacterConverter : IDataAccessCharacterConverter
    {
        private readonly CharacterContext _characterContext;

        public DataAccessCharacterConverter(CharacterContext characterContext)
        {
            _characterContext = characterContext;
        }

        public Character Convert(DataAccess.Models.Character dataAccessCharacter)
        {
            var domainCharacter = new Character
            {
                CharacterId = dataAccessCharacter.CharacterId,
                Name = dataAccessCharacter.Name,
                Status = dataAccessCharacter.Status,
                Species = dataAccessCharacter.Species,
                Type = dataAccessCharacter.Type,
                Gender = dataAccessCharacter.Gender,
                Origin = dataAccessCharacter.Origin == null ?
                    null :
                    new Planet
                    {
                        PlanetId = dataAccessCharacter.Origin.PlanetId,
                        Name = dataAccessCharacter.Origin.Name,
                        Url = dataAccessCharacter.Origin.Url,
                    },
                Location = dataAccessCharacter.Location == null ?
                    null :
                    new Planet
                    {
                        PlanetId = dataAccessCharacter.Location.PlanetId,
                        Name = dataAccessCharacter.Location.Name,
                        Url = dataAccessCharacter.Location.Url,
                    },
                Image = dataAccessCharacter.Image,
                Episodes = dataAccessCharacter.CharacterEpisodes.Select(x =>
                    new Episode
                    {
                        EpisodeId = x.EpisodeId,
                        FullName = x.Episode.FullName
                    }
                ).ToList(),
                Url = dataAccessCharacter.Url,
                Created = dataAccessCharacter.Created,
            };
            return domainCharacter;
        }

        public async Task<DataAccess.Models.Character> ConvertAsync(Character character)
        {
            var dataAccessCharacter = new DataAccess.Models.Character
            {
                CharacterId = character.CharacterId ?? 0,
                Name = character.Name,
                Status = character.Status,
                Species = character.Species,
                Type = character.Type,
                Gender = character.Gender,
                Origin = character.Origin == null ?
                    null :
                    await _characterContext.Planet.FirstOrDefaultAsync(x => x.Name.Equals(character.Origin.Name) && x.Url.Equals(character.Origin.Url)),
                Location = character.Location == null ?
                    null :
                    await _characterContext.Planet.FirstOrDefaultAsync(x => x.Name.Equals(character.Location.Name) && x.Url.Equals(character.Location.Url)),
                Image = character.Image,
                CharacterEpisodes = new List<CharacterEpisode>(),
                Url = character.Url,
                Created = DateTime.Now,
            };

            foreach (var episode in character.Episodes ?? new List<Episode>())
            {
                var dataAccessEpisode = await _characterContext.Episode
                                            .Include(e => e.CharacterEpisodes)
                                            .FirstOrDefaultAsync(x => x.FullName.Equals(episode.FullName)) ??
                                        new DataAccess.Models.Episode
                                        {
                                            FullName = episode.FullName, 
                                            CharacterEpisodes = new List<CharacterEpisode>()
                                        };

                var characterEpisode = new CharacterEpisode
                {
                    Character = dataAccessCharacter,
                    CharacterId = dataAccessCharacter.CharacterId,
                    Episode = dataAccessEpisode,
                    EpisodeId = dataAccessEpisode.EpisodeId
                };

                dataAccessCharacter.CharacterEpisodes.Add(characterEpisode);
                dataAccessEpisode.CharacterEpisodes.Add(characterEpisode);
            }

            return dataAccessCharacter;
        }
    }
}
