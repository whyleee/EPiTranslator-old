using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using EPiTranslator.Tests;
using NSubstitute;

namespace EPiTranslator.UI
{
    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            RegisterRoutes(RouteTable.Routes);

            SetupTestData();
        }

        private void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new {id = RouteParameter.Optional}
            );
        }

        private void SetupTestData()
        {
            var factory = SubstituteAll.For<Get>();
            Get.The = factory;

            var language1 = new Language { Id = "en", Name = "English" };
            var language2 = new Language { Id = "da", Name = "Danish" };

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
                IsFallback = true,
                Language = "en",
                Category = "Dictionary"
            };
            var enTranslation3 = new Translation
            {
                Key = "/Header/Hello",
                Keyword = "Hello",
                Value = "Hello, world!",
                IsFallback = true,
                Language = "en",
                Category = "Header"
            };
            var daTranslation1 = new Translation
            {
                Key = "/Dictionary/Name",
                Keyword = "Name",
                Value = "Navn",
                Language = "da",
                Category = "Dictionary"
            };
            var daTranslation2 = new Translation
            {
                Key = "/Header/Hello",
                Keyword = "Hello",
                Value = "Hej, verden!",
                Language = "da",
                Category = "Header"
            };

            var translationsData = new List<Dictionary>
                {
                    new Dictionary {Language = "en", Entries = new List<Translation> {enTranslation1, enTranslation2, enTranslation3}},
                    new Dictionary {Language = "da", Entries = new List<Translation> {daTranslation1, daTranslation2}}
                };

            Application["AllTranslations"] = translationsData;

            factory.Translator.GetAllTranslations().Returns(translationsData);
            factory.Translator.UpdateTranslation(Arg.Do<Translation>(translation =>
                {
                    // Do not update shared state for integration tests.
                    if (HttpContext.Current.Request.Cookies["e2e"] != null)
                    {
                        HttpContext.Current.Response.Cookies["e2e"].Expires = DateTime.Now.AddDays(-1);
                        return;
                    }

                    var dicts = (List<Dictionary>) Application["AllTranslations"];
                    var entries = dicts.First(x => x.Language == translation.Language).Entries;
                    var existing = entries.FirstOrDefault(e => e.Key == translation.Key);

                    if (existing != null)
                    {
                        existing.Value = translation.Value;
                        existing.IsFallback = false;
                    }
                    else
                    {
                        translation.IsFallback = false;
                        ((IList<Translation>) entries).Add(translation);
                    }
                }));
        }
    }
}