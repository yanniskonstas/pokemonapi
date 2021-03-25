using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using pokemonapi.Services;
using pokemonapi.Models;

namespace pokemonapi.Controllers
{
    [Produces("application/json")]    
    [Route("pokemon")]
    [ApiController]
    public class PokemonController : ControllerBase
    {
        private readonly IPokemonService _pokemonService;
        public PokemonController(IPokemonService pokemonService) {
            _pokemonService = pokemonService ??
                throw new ArgumentNullException(nameof(pokemonService));            
        }

        [HttpGet("{name}")]
        public ActionResult<PokemonDto> GetPokemon(string name) {
            var pokemon = _pokemonService.GetPokemon(name).Result;
            if (pokemon == null || string.IsNullOrWhiteSpace(pokemon.Name)) return NotFound();
            return Ok(pokemon);
        }

        [HttpGet("translated/{name}")]        
        public ActionResult<PokemonDto> GetPokemonTranslated(string name) {
            var pokemon = _pokemonService.GetPokemon(name, true).Result;
            if (pokemon == null || string.IsNullOrWhiteSpace(pokemon.Name)) return NotFound();
            return Ok(pokemon);
        }

    }
}