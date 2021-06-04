using Microsoft.AspNetCore.Mvc;
using RickAndMorty.Domain.Processors;
using RickAndMorty.WebApi.Converters.V1;
using RickAndMorty.WebApi.Definitions;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace RickAndMorty.WebApi.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/[controller]")]

    public class RickAndMortyController : ControllerBase
    {
        private readonly ICharacterViewModelConverter _converter;
        private readonly IRickAndMortyProcessor _rickAndMortyProcessor;

        public RickAndMortyController(ICharacterViewModelConverter converter, IRickAndMortyProcessor rickAndMortyProcessor)
        {
            _converter = converter;
            _rickAndMortyProcessor = rickAndMortyProcessor;
        }

        [HttpGet(Name="GetAllAsync")]
        public async Task<IActionResult> GetAllAsync()
        {
            var getAll = await _rickAndMortyProcessor.GetAllCharactersAsync();
            var convertedCharacters = getAll.Characters.Select(_converter.Convert);

            Response.Headers.Add(Headers.FromDatabase, getAll.FromDatabase.ToString());

            return new ObjectResult(convertedCharacters)
            {
                StatusCode = (int)HttpStatusCode.OK
            };
        }
    }
}
