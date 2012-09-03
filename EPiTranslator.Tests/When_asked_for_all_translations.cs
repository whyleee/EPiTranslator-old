using System.Collections.Generic;
using System.Linq;
using EPiTranslator.Services;
using Machine.Specifications;
using NSubstitute;

namespace EPiTranslator.Tests
{
    [Subject("Using translations service")]
    public class when_asked_for_all_translations
    {
        static TranslationsController translations;
        static Get factory;
        static IEnumerable<DictionaryEntry> result;

        Establish context = () =>
        {
            translations = new TranslationsController();

            factory = SubstituteAll.For<Get>();
            Get.The = factory;

            var language1 = new Language {Id = "en", Name = "English"};
            var language2 = new Language {Id = "da", Name = "Danish"};

            factory.Translator.GetAllLanguages().Returns(new[] {language1, language2});

            var enTranslation1 = new Translation
                {
                    Key = "/Dictionary/Name",
                    Keyword = "Name",
                    Value = "Name",
                    Language = "en",
                    Category = "Dictionary"
                };
            var enTranslation2 = new Translation
                {
                    Key = "/Dictionary/Email",
                    Keyword = "Email",
                    Value = "Email",
                    Language = "en",
                    Category = "Dictionary"
                };
            var daTranslation1 = new Translation
                {
                    Key = "/Dictionary/Name",
                    Keyword = "Name",
                    Value = "Navn",
                    Language = "da",
                    Category = "Dictionary"
                };

            var translationsData = new List<Dictionary>
                {
                    new Dictionary {Language = "en", Entries = new[] {enTranslation1, enTranslation2}},
                    new Dictionary {Language = "da", Entries = new[] {daTranslation1}}
                };

            factory.Translator.GetAllTranslations().Returns(translationsData);
        };

        Because of = () => result = translations.GetAll();

        It should_not_be_null = () => result.ShouldNotBeNull();

        It should_have_2_entries = () => result.ShouldMatch(x => x.Count() == 2);

        It should_have_first_entry_with_translations_for_all_languages =
            () => result.First().Translations.ShouldMatch(x => x.Count() == 2);

        It should_have_second_entry_with_translations_for_all_languages =
            () => result.Last().Translations.ShouldMatch(x => x.Count() == 2);
    }
}