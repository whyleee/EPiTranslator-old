using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http;

namespace EPiTranslator.Services
{
    public class TranslationsController : ApiController
    {
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

        public void Update(Translation translation)
        {
            Get.The.Translator.UpdateTranslation(translation);
        }

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
