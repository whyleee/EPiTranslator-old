using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EPiTranslator.Services;
using Machine.Specifications;
using NSubstitute;

namespace EPiTranslator.Tests
{
    public class for_translation_service : with_factory_mocker
    {
        protected static TranslationsController translations;

        Establish context = () =>
        {
            translations = new TranslationsController();
        };
    }

    [Subject("Translation service")]
    public class when_asked_for_all_translations_and_no_translations_in_the_storage : for_translation_service
    {
        static IEnumerable<DictionaryEntry> result;

        Establish context = () =>
        {
            factory.Translator.GetAllLanguages().ReturnsSomeLanguages();

            factory.Translator.GetAllTranslations().ReturnsNothing();
        };

        Because of = () => result = translations.GetAll();

        It should_not_be_null = () => result.ShouldNotBeNull();

        It should_be_an_empty_collection = () => result.ShouldBeEmpty();
    }

    [Subject("Translation service")]
    public class when_asked_for_all_translations_and_translator_has_translations_in_some_languages : for_translation_service
    {
        static IEnumerable<DictionaryEntry> result;

        Establish context = () =>
        {
            factory.Translator.GetAllLanguages().ReturnsEnglishAndDanishLanguages();

            var enNameTranslation = new Translation
                {
                    Key = "/Dictionary/Name", Keyword = "Name", Value = "Name", Language = "en", Category = "Dictionary"
                };
            var daNameTranslation = new Translation
                {
                    Key = "/Dictionary/Name", Keyword = "Name", Value = "Navn", Language = "da", Category = "Dictionary"
                };

            var enEmailTranslation = new Translation
                {
                    Key = "/Mails/Email", Keyword = "Email", Value = "Email", Language = "en", Category = "Mails"
                };

            var translations = new[]
                {
                    new Dictionary {Language = "en", Entries = new[] {enNameTranslation, enEmailTranslation}},
                    new Dictionary {Language = "da", Entries = new[] {daNameTranslation}}
                };

            factory.Translator.GetAllTranslations().Returns(translations);
        };

        Because of = () => result = translations.GetAll();

        It should_have_count_of_entries_equal_to_count_of_different_words =
            () => result.Count().ShouldEqual(2);

        It should_have_translations_of_word_for_each_language_if_exist =
            () => result.First(x => x.Keyword == "Name").Translations.Select(x => x.Value).ShouldContain("Name", "Navn");

        It should_have_translation_without_value_for_not_translated_words_in_particular_language =
            () => result.First(x => x.Keyword == "Email").Translations.Select(x => x.Value).ShouldContain("Email", null);

        It should_have_category_name_for_all_entries =
            () => result.Select(x => x.Category).ShouldContain("Dictionary", "Mails");

        It should_have_keys_for_all_translations =
            () => result.SelectMany(x => x.Translations).Select(x => x.Key).ShouldContain("/Dictionary/Name", "/Mails/Email");
    }

    [Subject("Translations service")]
    public class when_asked_to_update_a_translation : for_translation_service
    {
        static Translation newTranslation;

        Establish context = () =>
        {
            newTranslation = new Translation
            {
                Key = "/Dictionary/Surname", Keyword = "Surname", Value = "Efternavn", Language = "da", Category = "Dictionary"
            };
        };

        Because of = () => translations.Put(newTranslation);

        It should_update_translation_in_storage =
            () => factory.Translator.Received().UpdateTranslation(newTranslation);
    }
}
