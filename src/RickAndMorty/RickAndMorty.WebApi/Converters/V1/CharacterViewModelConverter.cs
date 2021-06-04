using RickAndMorty.Domain.Models;
using RickAndMorty.WebApi.ViewModels;
using System.Linq;

namespace RickAndMorty.WebApi.Converters.V1
{
    public interface ICharacterViewModelConverter
    {
        public CharacterViewModel Convert(Character character);
    }

    public class CharacterViewModelConverter : ICharacterViewModelConverter
    {
        public CharacterViewModel Convert(Character character)
        {
            var characterViewModel = new CharacterViewModel
            {
                Id = character.Id,
                Name = character.Name,
                Status = character.Status,
                Species = character.Species,
                Type = character.Type,
                Gender = character.Gender,
                Origin = character.Origin == null ?
                    null :
                    new OriginViewModel
                    {
                        Id = character.Origin.Id,
                        Name = character.Origin.Name,
                        Url = character.Origin.Url,
                    },
                Location = character.Location == null ?
                    null :
                    new LocationViewModel
                    {
                        Id = character.Location.Id,
                        Name = character.Location.Name,
                        Url = character.Location.Url,
                    },
                Image = character.Image,
                Episodes = character.Episodes.Select(x =>
                    new EpisodeViewModel
                    {
                        Id = x.Id,
                        FullName = x.FullName
                    }
                ).ToList(),
                Url = character.Url,
                Created = character.Created,
            };
            return characterViewModel;
        }
    }
}
