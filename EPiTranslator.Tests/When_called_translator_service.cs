using System.Collections.Generic;
using EPiTranslator.Services;
using Machine.Specifications;

namespace EPiTranslator.Tests
{
    public class When_called_translator_service
    {
        static TranslatorController translator;
        static IEnumerable<string> result;

            Establish context = () =>
        {
            translator = new TranslatorController();
        };

        Because of = () => result = translator.Get();

        It should_contain_hello_word = () => result.ShouldContain("Hello");

        It should_contain_world_word = () => result.ShouldContain("world");
    }
}