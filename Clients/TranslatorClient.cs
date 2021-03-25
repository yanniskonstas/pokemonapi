using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace pokemonapi.Clients
{
    public class TranslatorClient : ITextTranslatorService {
        private static HttpClient _httpClient;

        public TranslatorClient() {
            if (_httpClient == null) {
                _httpClient = new HttpClient();
                _httpClient.BaseAddress = new Uri("https://api.funtranslations.com/translate/");
                _httpClient.Timeout = new TimeSpan(0, 0, 10);
                _httpClient.DefaultRequestHeaders.Clear();
            }
        }

        public async Task<string> TranslateText(string text, TranslationKind translationKind) {
            var translatorJson = GetTranslatorJson(translationKind);
            var  response = await _httpClient.GetAsync($"{translatorJson}?text={text}");
            //response.EnsureSuccessStatusCode();
            if (!response.IsSuccessStatusCode) return text;
            var content = await response.Content.ReadAsStringAsync();
            return DeserializeContent(content);
        }

        private static string GetTranslatorJson(TranslationKind translationKind) => 
            translationKind switch {
                TranslationKind.Yoda => "yoda.json",
                TranslationKind.Shakespeare => "shakespeare.json",
                _ => "shakeshpeare.json"
            };

        private string DeserializeContent(string content) {
            var data = (JObject)JsonConvert.DeserializeObject(content); 
            return data["contents"]["translated"].ToString();
        } 
    }
}