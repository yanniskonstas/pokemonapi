using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;
using pokemonapi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static System.Console;

namespace pokemonapi.Clients {
    public class PokemonClient {
        private static HttpClient _httpClient;
        private readonly ITextTranslatorService _textTranslator;

        public PokemonClient(ITextTranslatorService textTranslator) {
            _textTranslator = textTranslator;
            if (_httpClient == null) {
                _httpClient = new HttpClient();
                _httpClient.BaseAddress = new Uri("https://pokeapi.co/api/v2/");
                _httpClient.Timeout = new TimeSpan(0, 0, 10);
                _httpClient.DefaultRequestHeaders.Clear();
            }
        }

        public async Task<PokemonDto> GetPokemonUsingStreams(string name, bool translate) {
            var request = new HttpRequestMessage( HttpMethod.Get, $"pokemon-species/{name.ToLowerInvariant()}/"); 
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            using ( var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead)) {
                var stream = await response.Content.ReadAsStreamAsync();
                //response.EnsureSuccessStatusCode();
                if (!response.IsSuccessStatusCode) return new PokemonDto();
                return translate ? TranslateText(DeserializeStream(stream)) : DeserializeStream(stream);
            }           
        }

        private PokemonDto DeserializeStream(Stream stream) {
            var reader = new StreamReader(stream);
            return DeserializeContent(reader.ReadToEnd());
        }

        private PokemonDto DeserializeContent(string content) {
            var data = (JObject)JsonConvert.DeserializeObject(content);       

            JArray flavorTextEntriesArray = JArray.Parse(data["flavor_text_entries"].ToString());
            var flavorTextEntries =  new List<FlavorText>();
            try {
                flavorTextEntries = flavorTextEntriesArray.Select(f => new FlavorText {
                    Flavor_Text = f["flavor_text"].ToString(),
                    Language = new Language { 
                        Name = f["language"]["name"].ToString()
                    }
                }).ToList();
            } catch {
                WriteLine("Desciption not found");
            }

            var pokemonDescription = String.Empty;
            if (flavorTextEntries.Any()) {
                var enFlavorTextEntries = flavorTextEntries.Where(f => f.Language.Name == "en").FirstOrDefault();
                pokemonDescription = enFlavorTextEntries != null ? enFlavorTextEntries.Flavor_Text : String.Empty;
            }

            JToken nameJToken, isLegendaryJToken;  
            var pokemonDto = new PokemonDto() {
                Name = data.TryGetValue("name", out nameJToken) ? (string)nameJToken : string.Empty,
                IsLegendary = data.TryGetValue("is_legendary", out isLegendaryJToken) ? (bool)isLegendaryJToken : false,
                Habitat  = data.SelectToken("habitat.name") != null ? data["habitat"]["name"].ToString() : string.Empty,
                Description = RemoveTextFormating(pokemonDescription)
            };

            return pokemonDto;
        } 

        private PokemonDto TranslateText(PokemonDto pokemonDto) {
            pokemonDto.Description = 
                pokemonDto.Habitat.ToLowerInvariant() == "cave" || pokemonDto.IsLegendary 
                ? _textTranslator.TranslateText(pokemonDto.Description, TranslationKind.Yoda).Result
                : _textTranslator.TranslateText(pokemonDto.Description, TranslationKind.Shakespeare).Result;
            return pokemonDto;                
        }

        private string RemoveTextFormating(string text) => Regex.Replace(text, @"\t|\n|\r|\f", " ");
    }
}