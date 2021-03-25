using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace pokemonapi.Models {
    public class PokemonDto {

        [JsonProperty("name", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Name {get; set;}
        public string Description { get; set; }
        public string Habitat {get; set;}

        [JsonProperty("is_legendary", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public bool IsLegendary {get; set;}
    }

    public class FlavorText {
        [JsonProperty("flavor_text", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Flavor_Text { get; set; }

        [JsonProperty("language", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public Language Language {get; set; }
    }

    public class Language {

        [JsonProperty("name", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Name {get; set;}
    }
}