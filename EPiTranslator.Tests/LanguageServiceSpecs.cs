using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EPiTranslator.Services;
using Machine.Specifications;
using NSubstitute;

namespace EPiTranslator.Tests
{
    public class for_language_service : with_factory_mocker
    {
        protected static LanguagesController languages;

        Establish context = () =>
        {
            languages = new LanguagesController();
        };
    }

    [Subject("Language service")]
    public class when_asked_for_all_languages_and_no_data_in_translator : for_language_service
    {
        static IEnumerable<Language> result;

        Establish context = () =>
        {
            factory.Translator.GetAllLanguages().Returns(new Language[] { });
        };

        Because of = () => result = languages.GetAll();

        It should_not_be_null = () => result.ShouldNotBeNull();

        It should_be_an_empty_collection = () => result.ShouldBeEmpty();
    }

    [Subject("Language service")]
    public class when_asked_for_all_languages_and_translator_has_some_languages : for_language_service
    {
        static IEnumerable<Language> result;

        Establish context = () =>
        {
            var enLanguage = new Language {Id = "en", Name = "English"};
            var daLanguage = new Language {Id = "da", Name = "Danish"};

            factory.Translator.GetAllLanguages().Returns(new[] {enLanguage, daLanguage});
        };

        Because of = () => result = languages.GetAll();

        It should_return_same_count_of_languages =
            () => result.Count().ShouldEqual(2);

        It should_return_id_for_each_language =
            () => result.Select(x => x.Id).ShouldContain("en", "da");

        It should_return_display_name_for_each_language =
            () => result.Select(x => x.Name).ShouldContain("English", "Danish");
    }
}
