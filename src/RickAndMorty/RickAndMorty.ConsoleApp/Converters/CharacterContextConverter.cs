using System;
using System.Collections.Generic;
using System.Linq;
using RickAndMorty.DataAccess.Models;
using Character = RickAndMorty.ConsoleApp.Models.Character;

namespace RickAndMorty.ConsoleApp.Converters
{
    public interface ICharacterContextConverter
    {
        public DataAccess.Models.Character Convert(Character character);
    }

    public class CharacterContextConverter : ICharacterContextConverter
    {
        private readonly List<Episode> _episodes;
        private readonly List<Planet> _planets;

        public CharacterContextConverter()
        {
            _episodes = new List<Episode>();
            _planets = new List<Planet>();
        }

        public DataAccess.Models.Character Convert(Character character)
        {
            var dataAccessCharacter = new DataAccess.Models.Character
            {
                CharacterId = character.Id ?? 0,
                Name = character.Name,
                Status = character.Status,
                Species = character.Species,
                Type = character.Type,
                Gender = character.Gender,
                Origin = character.Origin == null ? null : Convert(character.Origin.Name, character.Origin.Url),
                Location = character.Location == null ? null : Convert(character.Location.Name, character.Location.Url),
                Image = character.Image,
                Url = character.Url,
                Created = character.Created ?? DateTime.Now
            };

            dataAccessCharacter.CharacterEpisodes = character.Episode?.Select(x => Convert(x, dataAccessCharacter)).ToList();

            return dataAccessCharacter;
        }

        private Planet? Convert(string? name, string? url)
        {
            if (name == "unknown")
                return null;

            var selectedPlanet = _planets.SingleOrDefault(x => x.Name.Equals(name) && x.Url.Equals(url));
            if (selectedPlanet != null)
                return selectedPlanet;
            
            selectedPlanet = new Planet
            {
                Name = name,
                Url = url
            };
            _planets.Add(selectedPlanet);

            return selectedPlanet;
        }
        
        private CharacterEpisode Convert(string episode, DataAccess.Models.Character character)
        {
            var selectedEpisode = _episodes.SingleOrDefault(x => x.FullName.Equals(episode));
            if (selectedEpisode == null)
            {
                selectedEpisode = new Episode
                {
                    FullName = episode,
                    CharacterEpisodes = new List<CharacterEpisode>()
                };
                _episodes.Add(selectedEpisode);
            }

            var characterEpisode = new CharacterEpisode
            {
                Character = character,
                Episode = selectedEpisode
            };

            selectedEpisode.CharacterEpisodes.Add(characterEpisode);

            return characterEpisode;
        }
    }
}
