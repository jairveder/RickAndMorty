using RickAndMorty.ConsoleApp.Models;
using RickAndMorty.DataAccess.Models;
using System.Collections.Generic;
using System.Linq;

namespace RickAndMorty.ConsoleApp
{
    public interface ICharacterContextConverter
    {
        public Character Convert(Result result);
    }

    public class CharacterContextConverter : ICharacterContextConverter
    {
        private readonly List<Episode> _episodes;
        private readonly List<DataAccess.Models.Location> _locations;
        private readonly List<DataAccess.Models.Origin> _origins;

        public CharacterContextConverter()
        {
            _episodes = new List<Episode>();
            _locations = new List<DataAccess.Models.Location>();
            _origins = new List<DataAccess.Models.Origin>();
        }

        public Character Convert(Result result)
        {
            if (result == null)
                return null;

            var character = new Character
            {
                Id = result.Id,
                Name = result.Name,
                Status = result.Status,
                Species = result.Species,
                Type = result.Type,
                Gender = result.Gender,
                Origin = Convert(result.Origin),
                Location = Convert(result.Location),
                Image = result.Image,
                Url = result.Url,
                Created = result.Created
            };

            character.CharacterEpisodes = result.Episode.Select(x => Convert(x, character)).ToList();

            return character;
        }

        private DataAccess.Models.Origin Convert(Models.Origin origin)
        {
            if (origin == null || origin.Name == "unknown")
                return null;

            var selectedOrigin = _origins.SingleOrDefault(x => x.Name.Equals(origin.Name) && x.Url.Equals(origin.Url));
            if (selectedOrigin != null)
                return selectedOrigin;
            
            selectedOrigin = new DataAccess.Models.Origin
            {
                Name = origin.Name,
                Url = origin.Url
            };
            _origins.Add(selectedOrigin);

            return selectedOrigin;
        }

        private DataAccess.Models.Location Convert(Models.Location location)
        {
            if (location == null || location.Name == "unknown")
                return null;

            var selectedLocation = _locations.SingleOrDefault(x => x.Name.Equals(location.Name) && x.Url.Equals(location.Url));
            if (selectedLocation != null)
                return selectedLocation;

            selectedLocation = new DataAccess.Models.Location
            {
                Name = location.Name,
                Url = location.Url
            };
            _locations.Add(selectedLocation);

            return selectedLocation;
        }

        private CharacterEpisode Convert(string episode, Character character)
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
