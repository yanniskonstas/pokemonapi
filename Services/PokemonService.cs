using System;
using pokemonapi.Models;
using pokemonapi.Clients;
using System.Threading.Tasks;

namespace pokemonapi.Services {
    public class PokemonService : IPokemonService  {      
        private readonly ITextTranslatorService _textTranslator;
        public PokemonService(ITextTranslatorService textTranslator) {
            _textTranslator = textTranslator ??
                throw new ArgumentNullException(nameof(textTranslator));            
        }  
        public async Task<PokemonDto> GetPokemon(string name, bool translate = false) {
            var pokemonClient = new PokemonClient(_textTranslator);
            //return await pokemonClient.GetPokemon(name);
            return await pokemonClient.GetPokemonUsingStreams(name, translate);
        }
    }
}