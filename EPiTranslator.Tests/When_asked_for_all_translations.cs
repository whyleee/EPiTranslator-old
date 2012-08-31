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
        static IDictionary<string, IEnumerable<Translation>> result;

        Establish context = () =>
        {
            translations = new TranslationsController();

            factory = SubstituteAll.For<Get>();
            Get.The = factory;

            //Get.The.Translator.Returns(Substitute.For<Translator>());

            var enTranslation1 = new Translation
                {
                    Keyword = "Name",
                    Value = "Email",
                    Language = "en",
                    Category = "Dictionary"
                };
            var enTranslation2 = new Translation
                {
                    Keyword = "Email",
                    Value = "Email",
                    Language = "en",
                    Category = "Dictionary"
                };
            var daTranslation1 = new Translation
                {
                    Keyword = "Name",
                    Value = "Navn",
                    Language = "da",
                    Category = "Dictionary"
                };

            var translationsData = new Dictionary<string, IEnumerable<Translation>>
                {
                    {"en", new[] {enTranslation1, enTranslation2}},
                    {"da", new[] {daTranslation1}}
                };

            factory.Translator.GetAllTranslations().Returns(translationsData);
        };

        Because of = () => result = translations.GetAll();

        It should_not_be_null = () => result.ShouldNotBeNull();

        It should_have_2_languages = () => result.ShouldMatch(x => x.Keys.Count == 2);
    }
}