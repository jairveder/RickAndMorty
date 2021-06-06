using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RickAndMorty.Domain.Processors;
using RickAndMorty.WebApi.Converters.V1;
using RickAndMorty.WebApi.CreateModels.V1;
using RickAndMorty.WebApi.Definitions;
using RickAndMorty.WebApi.ViewModels.V1;

namespace RickAndMorty.WebApi.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [Route("api/[controller]")]

    public class RickAndMortyController : ControllerBase
    {
        private readonly ICharacterConverter _converter;
        private readonly IRickAndMortyProcessor _rickAndMortyProcessor;

        public RickAndMortyController(ICharacterConverter converter, IRickAndMortyProcessor rickAndMortyProcessor)
        {
            _converter = converter;
            _rickAndMortyProcessor = rickAndMortyProcessor;
        }

        /// <summary>
        /// Gets all characters
        /// </summary>
        [HttpGet(Name="GetAllAsync")]
        [ProducesResponseType(typeof(IEnumerable<CharacterViewModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllAsync()
        {
            var charactersResult = await _rickAndMortyProcessor.GetAllCharactersAsync();
            var convertedCharacters = charactersResult.Characters?.Select(_converter.Convert);

            Response.Headers.Add(Headers.FromDatabase, charactersResult.FromDatabase.ToString());

            return new ObjectResult(convertedCharacters)
            {
                StatusCode = (int)HttpStatusCode.OK
            };
        }

        /// <summary>
        /// Get all characters for a given planet (location) 
        /// </summary>
        /// <param name="planetName">Full name of the planet</param>
        [HttpGet("planets/{planetName}", Name = "GetByPlanetAsync")]
        [ProducesResponseType(typeof(IEnumerable<CharacterViewModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetByPlanetAsync(string planetName)
        {
            var charactersResult = await _rickAndMortyProcessor.GetCharactersByPlanetAsync(planetName);
            if (charactersResult == null)
                return new StatusCodeResult((int)HttpStatusCode.NotFound);

            var convertedCharacters = charactersResult.Characters?.Select(_converter.Convert);

            Response.Headers.Add(Headers.FromDatabase, charactersResult.FromDatabase.ToString());

            return new ObjectResult(convertedCharacters)
            {
                StatusCode = (int)HttpStatusCode.OK
            };
        }

        /// <summary>
        /// Creates a character
        /// </summary>
        /// <param name="characterCreateModel">Model to create a character</param>
        [HttpPost(Name = "PostAsync")]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> PostAsync([FromBody] CharacterCreateModel characterCreateModel)
        {
            var character = _converter.Convert(characterCreateModel);
            var createdSuccessfully = await _rickAndMortyProcessor.CreateAsync(character);

            return new StatusCodeResult(createdSuccessfully ? (int)HttpStatusCode.Created : (int)HttpStatusCode.BadRequest);
        }
    }
}
