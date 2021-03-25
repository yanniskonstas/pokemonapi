using System;
using System.Threading.Tasks;
using pokemonapi.Models;

namespace pokemonapi.Services {

    public interface IPokemonService {
        Task<PokemonDto> GetPokemon(string name, bool traslate = false);
    }

}