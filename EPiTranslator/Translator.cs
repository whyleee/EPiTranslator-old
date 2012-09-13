using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml.Linq;
using EPiServer.Configuration;
using EPiServer.Core;
using EPiServer.DataAbstraction;
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
                return LanguageBranch.ListEnabled().Select(x => x.LanguageID);

                ////// If site languages were determined earlier, return them.
                ////if (_siteLanguages != null)
                ////{
                ////    return _siteLanguages;
                ////}

                ////// Get languages for all sites and return them.
                ////var allLanguages = new List<string>();

                ////foreach (var settings in Settings.All)
                ////{
                ////    var currentSiteLanguages = Get.The.EPiServer.GetPage(new PageReference(settings.Value.PageStartId)).PageLanguages;
                ////    allLanguages.AddRange(currentSiteLanguages.Except(allLanguages));
                ////}

                ////_siteLanguages = allLanguages;

                ////return allLanguages;
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
        /// Updates translation using specified key in specified language.
        /// </summary>
        /// <param name="key">Translation key.</param>
        /// <param name="translation">New translation.</param>
        /// <param name="language">Translation language.</param>
        public virtual void UpdateTranslation(string key, string translation, string language)
        {
            AddFallbackTranslation(language, key, translation, updateExisting: true);
        }

        /// <summary>
        /// Updates translation.
        /// </summary>
        /// <param name="translation">New translation.</param>
        public virtual void UpdateTranslation(Translation translation)
        {
            UpdateTranslation(translation.Key, translation.Value, translation.Language);
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

        /// <summary>
        /// Gets all languages registered for all sites in EPiServer.
        /// </summary>
        /// <returns>Collection of <see cref="Language"/> objects representing all languages registered for all sites in EPiServer.</returns>
        public virtual IEnumerable<Language> GetAllLanguages()
        {
            var languages = new List<Language>();

            foreach (var language in SiteLanguages)
            {
                var doc = GetTranslationsStorage(language);

                if (doc == null)
                {
                    continue;
                }

                var languageName = doc.Root.Child("language").Attribute("name").Value;

                languages.Add(new Language {Id = language, Name = languageName});
            }

            return languages;
        }

        /// <summary>
        /// Gets all translations from the storage grouped by language.
        /// </summary>
        /// <returns>Collection of <see cref="Dictionary"/> objects representing all translations from the storage grouped by language.</returns>
        public virtual IEnumerable<Dictionary> GetAllTranslations()
        {
            var translations = new List<Dictionary>();

            foreach (var language in SiteLanguages)
            {
                var doc = GetTranslationsStorage(language);

                if (doc == null)
                {
                    continue;
                }

                var root = doc.Root.Child("language");

                var translationsForLanguage = root.Descendants()
                    .Where(node => !node.HasElements)
                    .Select(node => new Translation
                        {
                            Key = GetFullTranslationKey(node),
                            Keyword = node.Name,
                            Value = node.Value != null && !node.Value.StartsWith("[Missing") ? node.Value : null,
                            IsFallback = node.Attribute("fallback") != null,
                            Language = language,
                            Category = GetCategoryName(node).ToSentenceCase()
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

        /// <summary>
        /// Gets the storage for translations for provided language.
        /// </summary>
        /// <param name="language">The language.</param>
        /// <returns><see cref="XDocumentWrapper"/> object representing translations storage.</returns>
        protected internal virtual XDocumentWrapper GetTranslationsStorage(string language)
        {
            var physicalPath = Get.The.HttpContext.Server.MapPath("~/lang/" + string.Format(TranslationsFileName, language));

            if (!Get.The.FileManager.Exists(physicalPath))
            {
                return null;
            }

            return Get.The.XmlHelper.LoadXml(physicalPath);
        }

        /// <summary>
        /// Gets full translation key for its node in the storage.
        /// </summary>
        /// <param name="node">Translation node in the storage.</param>
        /// <returns>Full translation key for its node in the storage.</returns>
        private string GetFullTranslationKey(XElementWrapper node)
        {
            var parentChain = new List<string>();
            var current = node;

            while (current != null && current.Name != "language" && current.Attribute("id") == null)
            {
                parentChain.Add(current.Name);
                current = current.Parent;
            }

            return "/" + string.Join("/", Enumerable.Reverse(parentChain));
        }

        /// <summary>
        /// Gets the full name of the category for translation represented by provided node in the storage.
        /// </summary>
        /// <param name="node">Translation node in the storage.</param>
        /// <returns>Full name of the category for translation represented by provided node in the storage.</returns>
        private string GetFullCategoryName(XElementWrapper node)
        {
            var fullKey = GetFullTranslationKey(node);
            var namePart = fullKey.LastIndexOf("/");

            if (namePart <= 0)
            {
                return null;
            }

            return fullKey.Remove(namePart).Substring(1).Replace("/", " - ");
        }

        /// <summary>
        /// Gets the name of the category for translation.
        /// </summary>
        /// <param name="node">The node representing a translation.</param>
        /// <returns>The name of the category for translation.</returns>
        private string GetCategoryName(XElementWrapper node)
        {
            return node.Parent != null ? node.Parent.Name : "";
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
        /// <param name="updateExisting">Determines whether to update existing translation if found or to skip.</param>
        protected internal virtual void AddFallbackTranslation(string locale, string key, string fallback, bool updateExisting = false)
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
                    // If last element found and we don't want to override existing translations - skip adding fallback to it.
                    if (pathElementName == lastPathElementName && !updateExisting)
                    {
                        continue;
                    }

                    currentNode = element;
                }

                // If it is a last element of the path, insert a text and break the cycle.
                if (currentNode.Name == lastPathElementName)
                {
                    currentNode.Value = fallback;
                    currentNode.SetAttributeValue("fallback", updateExisting ? null : "true");
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

    public static class StringExtensions
    {
        public static string ToSentenceCase(this string str)
        {
            return Regex.Replace(str, "[a-z][A-Z]", m => m.Value[0] + " " + char.ToLower(m.Value[1]));
        }
    }
}
