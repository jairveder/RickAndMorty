using System;
using System.Linq;
using RickAndMorty.Domain.Models;
using RickAndMorty.WebApi.CreateModels.V1;
using RickAndMorty.WebApi.ViewModels.V1;

namespace RickAndMorty.WebApi.Converters.V1
{
    public interface ICharacterConverter
    {
        public CharacterViewModel Convert(Character character);
        public Character Convert(CharacterCreateModel characterCreateModel);
    }

    public class CharacterConverter : ICharacterConverter
    {
        public CharacterViewModel Convert(Character character)
        {
            var characterViewModel = new CharacterViewModel
            {
                CharacterId = character.CharacterId ?? 0,
                Name = character.Name,
                Status = character.Status,
                Species = character.Species,
                Type = character.Type,
                Gender = character.Gender,
                Origin = character.Origin == null ?
                    null :
                    new PlanetViewModel
                    {
                        PlanetId = character.Origin.PlanetId ?? 0,
                        Name = character.Origin.Name,
                        Url = character.Origin.Url,
                    },
                Location = character.Location == null ?
                    null :
                    new PlanetViewModel
                    {
                        PlanetId = character.Location.PlanetId ?? 0,
                        Name = character.Location.Name,
                        Url = character.Location.Url,
                    },
                Image = character.Image,
                Episodes = character.Episodes?.Select(x =>
                    new EpisodeViewModel
                    {
                        EpisodeId = x.EpisodeId ?? 0,
                        FullName = x.FullName
                    }
                ).ToList(),
                Url = character.Url,
                Created = character.Created ?? DateTime.Now,
            };
            return characterViewModel;
        }

        public Character Convert(CharacterCreateModel characterCreateModel)
        {
            var characterViewModel = new Character
            {
                Name = characterCreateModel.Name,
                Status = characterCreateModel.Status,
                Species = characterCreateModel.Species,
                Type = characterCreateModel.Type,
                Gender = characterCreateModel.Gender,
                Origin = characterCreateModel.Origin == null ?
                    null :
                    new Planet
                    {
                        Name = characterCreateModel.Origin.Name,
                        Url = characterCreateModel.Origin.Url,
                    },
                Location = characterCreateModel.Location == null ?
                    null :
                    new Planet
                    {
                        Name = characterCreateModel.Location.Name,
                        Url = characterCreateModel.Location.Url,
                    },
                Image = characterCreateModel.Image,
                Episodes = characterCreateModel.Episodes?.Select(x =>
                    new Episode
                    {
                        FullName = x.FullName
                    }
                ).ToList(),
                Url = characterCreateModel.Url,
            };
            return characterViewModel;
        }
    }
}
