using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http;

namespace EPiTranslator.Services
{
    /// <summary>
    /// Contains API to get/update translations in the storage.
    /// </summary>
    public class TranslationsController : ApiController
    {
        /// <summary>
        /// Gets all translations registered in the storage.
        /// </summary>
        /// <returns>
        /// Collection of <see cref="DictionaryEntry"/> representing a word or a phrase with translations to different languages.
        /// </returns>
        /// <remarks>
        /// For not translated words/phrases in particular language each returned entry
        /// anyway will contain a <see cref="Translation"/> object but with <c>null</c> as a <see cref="Translation.Value"/>.
        /// </remarks>
        public IEnumerable<DictionaryEntry> GetAll()
        {
            var languages = Get.The.Translator.GetAllLanguages();
            var dictionaries = Get.The.Translator.GetAllTranslations();

            return dictionaries
                .SelectMany(dict => dict.Entries)
                .GroupBy(entry => entry.Keyword)
                .Select(translations => new DictionaryEntry
                    {
                        Keyword = translations.Key,
                        Category = translations.First().Category,
                        Translations = MapTranslations(translations, languages)
                    });
        }

        /// <summary>
        /// Updates the specified translation in the storage.
        /// </summary>
        /// <param name="translation">The translation.</param>
        /// <remarks>
        /// If no translation was in the storage earlier - it will insert it as a new translation.
        /// </remarks>
        public void Put(Translation translation)
        {
            Get.The.Translator.UpdateTranslation(translation);
        }

        /// <summary>
        /// Maps translations to specified languages, adding <see cref="Translation"/> objects with <c>null</c>
        /// in <see cref="Translation.Value"/> for not translated words/phrases in particular language.
        /// </summary>
        /// <param name="translations">Existing translations.</param>
        /// <param name="toLanguages">All available languages.</param>
        /// <returns>Collection of translations including empty ones for all available languages.</returns>
        private IEnumerable<Translation> MapTranslations(IEnumerable<Translation> translations, IEnumerable<Language> toLanguages)
        {
            translations = translations.ToList();

            var template = translations.First();
            var translationsToAllLanguages = new List<Translation>();

            foreach (var language in toLanguages)
            {
                var existingTranslation = translations.FirstOrDefault(x => x.Language == language.Id);

                if (existingTranslation != null)
                {
                    translationsToAllLanguages.Add(existingTranslation);
                }
                else
                {
                    var emptyTranslation = new Translation
                    {
                        Key = template.Key,
                        Keyword = template.Keyword,
                        Language = language.Id,
                        Category = template.Category
                    };

                    translationsToAllLanguages.Add(emptyTranslation);
                }
            }

            return translationsToAllLanguages;
        }
    }
}
