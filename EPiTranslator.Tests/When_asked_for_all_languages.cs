using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EPiTranslator.Services;
using Machine.Specifications;
using NSubstitute;

namespace EPiTranslator.Tests
{
    [Subject("Using language service")]
    public class when_asked_for_all_languages
    {
        static LanguagesController languages;
        static Get factory;
        static IEnumerable<Language> result;

        Establish context = () =>
        {
            languages = new LanguagesController();

            factory = SubstituteAll.For<Get>();
            Get.The = factory;

            var language1 = new Language { Id = "en", Name = "English" };
            var language2 = new Language { Id = "da", Name = "Danish" };

            factory.Translator.GetAllLanguages().Returns(new[] { language1, language2 });
        };

        Because of = () => result = languages.GetAll();

        It should_not_be_null = () => result.ShouldNotBeNull();

        It should_have_2_languages = () => result.ShouldMatch(x => x.Count() == 2);
    }
}
