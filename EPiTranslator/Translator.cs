using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Xml.Linq;
using EPiServer.Configuration;
using EPiServer.Core;
using EPiTranslator.Xml;

namespace EPiTranslator
{
    /// <summary>
    /// Provides high-level API for translating text using EPiServer language files.
    /// </summary>
    public class Translator
    {
        public const string TranslationsFileName = "Auto_{0}.xml";

        private static IEnumerable<string> _siteLanguages;

        /// <summary>
        /// Gets all registered site languages.
        /// </summary>
        /// <value>Collection of all registered site languages.</value>
        public IEnumerable<string> SiteLanguages
        {
            get
            {
                // If site languages were determined earlier, return them.
                if (_siteLanguages != null)
                {
                    return _siteLanguages;
                }

                // Get languages for all sites and return them.
                var allLanguages = new List<string>();

                foreach (var settings in Settings.All)
                {
                    var currentSiteLanguages = Get.The.EPiServer.GetPage(new PageReference(settings.Value.PageStartId)).PageLanguages;
                    allLanguages.AddRange(currentSiteLanguages.Except(allLanguages));
                }

                _siteLanguages = allLanguages;

                return allLanguages;
            }
        }

        /// <summary>
        /// Gets the current language.
        /// </summary>
        /// <value>The current language.</value>
        public string CurrentLanguage
        {
            get
            {
                return Thread.CurrentThread.CurrentUICulture.Name;
            }
        }

        /// <summary>
        /// Translates the text using specified key (the path of translation element in EPiServer language file).
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>Translated text if found or [Missing text] otherwise.</returns>
        /// <example>Example of key: "/UserControls/MyControl/Header".</example>
        public virtual string Translate(string key)
        {
            var text = Get.The.LanguageManager.TranslateRaw(key);

            if (text == null)
            {
                // Add translation to language files for languages without translation.
                foreach (var locale in SiteLanguages)
                {
                    var textForLanguage = Get.The.LanguageManager.TranslateRaw(key, locale);

                    if (textForLanguage == null)
                    {
                        AddFallbackTranslation(locale, key, GetDefaultTextForMissingTranslation(key, locale));
                    }
                }

                return GetDefaultTextForMissingTranslation(key, CurrentLanguage);
            }

            return text;
        }

        /// <summary>
        /// Translates the text using specified key (the path of translation element in EPiServer language file),
        /// replacing format items with the string representation of a corresponding object in a specified array.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="arguments">The object array with arguments for replacement items.</param>
        /// <returns>Translated and formatted text if found or [Missing text] otherwise.</returns>
        /// <example>
        /// Example of key: "/UserControls/MyControl/Header".
        /// Example of formatted value: "Minimum length is {0}".
        /// </example>
        public virtual string Translate(string key, params object[] arguments)
        {
            var translated = Translate(key);

            for (int i = 0; i < arguments.Length; ++i)
            {
                if (!ContainsPlaceholder(translated, i))
                {
                    return GetMissingPlaceholderString(translated, i);
                }
            }

            return string.Format(translated, arguments);
        }

        /// <summary>
        /// Translates the text using specified key (the path of translation element in EPiServer language file).
        /// In case when translated text wasn't found - returns provided fallback.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="fallback">The fallback.</param>
        /// <returns>Translated text if found or fallback text otherwise.</returns>
        /// <remarks>If translation wasn't found for the specified key, this method will create all needed elements
        /// in EPiServer language files for each site locale, inserting fallback text as a value.</remarks>
        public virtual string Translate(string key, string fallback)
        {
            var result = Get.The.LanguageManager.TranslateRaw(key);

            if (result == null)
            {
                // Add translation to language files for languages without translation.
                foreach (var locale in SiteLanguages)
                {
                    var textForLanguage = Get.The.LanguageManager.TranslateRaw(key, locale);

                    if (textForLanguage == null)
                    {
                        AddFallbackTranslation(locale, key, fallback);
                    }
                }

                return fallback;
            }

            return result;
        }

        /// <summary>
        /// Translates the text using specified key (the path of translation element in EPiServer language file),
        /// replacing format items with the string representation of a corresponding object in a specified array.
        /// In case when translated text wasn't found - returns provided fallback.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="fallback">The fallback.</param>
        /// <param name="arguments">The object array with arguments for replacement items.</param>
        /// <returns>Translated and formatted text if found or fallback text otherwise.</returns>
        /// <remarks>
        /// <para>
        /// Fallback text can contain format placeholders too.
        /// </para>
        /// <para>
        /// If translation wasn't found for the specified key, this method will create all needed elements
        /// in EPiServer language files for each site locale, inserting fallback text as a value.
        /// </para>
        /// </remarks>
        public virtual string Translate(string key, string fallback, params object[] arguments)
        {
            var translated = Translate(key, fallback);

            for (int i = 0; i < arguments.Length; ++i)
            {
                if (!ContainsPlaceholder(translated, i))
                {
                    return GetMissingPlaceholderString(translated, i);
                }
            }

            return string.Format(translated, arguments);
        }

        public virtual IEnumerable<Language> GetAllLanguages()
        {
            var languages = new List<Language>();

            foreach (var language in SiteLanguages)
            {
                var physicalPath = Get.The.HttpContext.Server.MapPath("~/lang/" + string.Format(TranslationsFileName, language));

                if (!Get.The.FileManager.Exists(physicalPath))
                {
                    continue;
                }

                var doc = Get.The.XmlHelper.LoadXml(physicalPath);
                var languageName = doc.Root.Child("language").Attribute("name").Value;

                languages.Add(new Language {Id = language, Name = languageName});
            }

            return languages;
        }

        public virtual IEnumerable<Dictionary> GetAllTranslations()
        {
            var translations = new List<Dictionary>();

            foreach (var language in SiteLanguages)
            {
                var physicalPath = Get.The.HttpContext.Server.MapPath("~/lang/" + string.Format(TranslationsFileName, language));

                if (!Get.The.FileManager.Exists(physicalPath))
                {
                    continue;
                }

                var doc = Get.The.XmlHelper.LoadXml(physicalPath);
                var root = doc.Root.Child("language");

                var translationsForLanguage = root.Descendants()
                    .Where(node => node.HasElements)
                    .Select(node => new Translation
                        {
                            Keyword = node.Name,
                            Value = node.Value,
                            Fallback = null,
                            Language = language,
                            Category = GetFullCategoryName(node)
                        })
                    .ToList();

                var dictionary = new Dictionary
                    {
                        Language = language,
                        Entries = translationsForLanguage
                    };

                translations.Add(dictionary);
            }

            return translations;
        }

        private string GetFullTranslationKey(XElementWrapper node)
        {
            var parentChain = new List<string>();
            var current = node;

            while (current != null)
            {
                parentChain.Add(current.Name);
                current = current.Parent;
            }

            return "/" + string.Join("/", Enumerable.Reverse(parentChain));
        }

        private string GetFullCategoryName(XElementWrapper node)
        {
            var fullKey = GetFullTranslationKey(node);
            var namePart = fullKey.LastIndexOf("/");

            if (namePart == -1)
            {
                return null;
            }

            return fullKey.Substring(1).Remove(namePart).Replace("/", " - ");
        }

        /// <summary>
        /// Adds the fallback text to the specified path in all EPiServer language files.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="fallback">The fallback.</param>
        protected internal virtual void AddFallbackTranslation(string key, string fallback)
        {
            // Add translation to all language files.
            foreach (var locale in SiteLanguages)
            {
                AddFallbackTranslation(locale, key, fallback);
            }
        }

        /// <summary>
        /// Adds the fallback text to the specified path in EPiServer language file for the specified language.
        /// </summary>
        /// <param name="locale">Language code to insert a fallback.</param>
        /// <param name="key">The key.</param>
        /// <param name="fallback">The fallback.</param>
        protected internal virtual void AddFallbackTranslation(string locale, string key, string fallback)
        {
            // Open language file.
            var physicalPath = Get.The.HttpContext.Server.MapPath("~/lang/" + string.Format(TranslationsFileName, locale));

            // If language file isn't exists, create it.
            if (!Get.The.FileManager.Exists(physicalPath))
            {
                CreateLanguageFile(locale, physicalPath);
            }

            var doc = Get.The.XmlHelper.LoadXml(physicalPath);

            // Extract XML element names.
            var keyPathElements = key.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            var lastPathElementName = keyPathElements.Last();
            var currentNode = doc.Root.Child("language");
            var fallbackInserted = false;

            // If not EPiServer language file structure - throw exception.
            if (currentNode == null)
            {
                throw new ArgumentException(string.Format("'{0}' is not a language file.", physicalPath));
            }

            // For each XML element in search key.
            foreach (var pathElementName in keyPathElements)
            {
                // Try to get element by name.
                var element = currentNode.Child(pathElementName);

                // If not found, add it.
                if (element == null)
                {
                    var children = currentNode.Children();
                    XElementWrapper elementAfter = null;

                    // Try to find the alphabetical place for the element.
                    foreach (var child in children)
                    {
                        elementAfter = child;

                        if (child.Name.CompareTo(pathElementName) > 0)
                        {
                            break;
                        }
                    }

                    // If there are some children.
                    if (elementAfter != null)
                    {
                        // Add the element in alphabetical order.
                        if (elementAfter.Name.CompareTo(pathElementName) > 0)
                        {
                            elementAfter.AddBeforeSelf(new XElementWrapper(pathElementName));
                        }
                        else
                        {
                            elementAfter.AddAfterSelf(new XElementWrapper(pathElementName));
                        }
                    }
                    else
                    {
                        // Otherwise, just add the element.
                        currentNode.Add(new XElementWrapper(pathElementName));
                    }
                        
                    currentNode = currentNode.Child(pathElementName);
                }
                else
                {
                    // If last element found - skip adding fallback to it.
                    if (pathElementName == lastPathElementName)
                    {
                        continue;
                    }

                    currentNode = element;
                }

                // If it is a last element of the path, insert a text and break the cycle.
                if (currentNode.Name == lastPathElementName)
                {
                    currentNode.Value = fallback;
                    fallbackInserted = true;
                    break;
                }
            }

            // Save results.
            if (fallbackInserted)
            {
                var file = new FileInfo(physicalPath);

                if (file.IsReadOnly)
                {
                    file.IsReadOnly = false;
                }

                doc.Save(physicalPath);
            }
        }

        /// <summary>
        /// Determines whether the specified text contains placeholder with the specified index.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="placeholderIndex">Placeholder index.</param>
        /// <returns>
        /// <c>true</c> if the specified text contains placeholder with the specified index; otherwise, <c>false</c>.
        /// </returns>
        private bool ContainsPlaceholder(string text, int placeholderIndex)
        {
            return text.Contains("{" + placeholderIndex + "}");
        }

        /// <summary>
        /// Gets the [Missing format placeholder] string for specified text and placeholder index.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="index">Placeholder index.</param>
        /// <returns>[Missing format placeholder] string with the specified text and missing placeholder index.</returns>
        private string GetMissingPlaceholderString(string text, int index)
        {
            return string.Format("[Missing format placeholder {{{0}}} in '{1}', {2} language]", index, text, CurrentLanguage);
        }

        /// <summary>
        /// Gets default text for missing translation.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="locale">Target language.</param>
        /// <returns>Default text for missing translation.</returns>
        private string GetDefaultTextForMissingTranslation(string key, string locale)
        {
            return "[Missing text " + key + " for " + locale.ToLower() + "]";
        }

        /// <summary>
        /// Creates the language file for the specified language.
        /// </summary>
        /// <param name="locale">The language code.</param>
        /// <param name="physicalPath">The physical path.</param>
        private void CreateLanguageFile(string locale, string physicalPath)
        {
            new XDocumentWrapper(
                new XDeclaration(version: "1.0", encoding: "utf-8", standalone: "yes"),
                new XElementWrapper("languages",
                    new XElementWrapper("language",
                        new XAttribute("name", new CultureInfo(locale).EnglishName),
                        new XAttribute("id", locale.ToLower())))
                ).Save(physicalPath);
        }
    }
}
