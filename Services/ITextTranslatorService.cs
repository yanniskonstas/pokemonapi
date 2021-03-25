using System;
using  System.Threading.Tasks;

namespace pokemonapi.Clients {
    public interface ITextTranslatorService {          
         public Task<string> TranslateText(string text, TranslationKind translationKind);
    }

    public enum TranslationKind {
        Yoda,
        Shakespeare,
        Default
    }
}