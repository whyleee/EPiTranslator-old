using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EPiTranslator.Services;
using Machine.Specifications;
using NSubstitute;

namespace EPiTranslator.Tests
{
    [Subject("Using translations service")]
    public class when_asked_to_update_a_translation
    {
        static TranslationsController translations;
        static Get factory;
        static Translation newTranslation;

        Establish context = () =>
        {
            translations = new TranslationsController();

            factory = SubstituteAll.For<Get>();
            Get.The = factory;

            newTranslation = new Translation
                {
                    Key = "/Dictionary/Name",
                    Keyword = "Name",
                    Value = "Navn",
                    Language = "da",
                    Category = "Dictionary"
                };
        };

        Because of = () => translations.Update(newTranslation);

        It should_update_translation = () => factory.Translator.Received().UpdateTranslation(newTranslation);
    }
}
