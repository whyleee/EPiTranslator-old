
namespace EPiTranslator
{
    /// <summary>
    /// Short localization exceptions, usefull everywhere.
    /// </summary>
    public static class LocalizationExtensions
    {
        /// <summary>
        /// Translates the text using specified key (the path of translation element in EPiServer language file).
        /// </summary>
        /// <param name="o">Any object can use this extension.</param>
        /// <param name="key">The key.</param>
        /// <returns>Translated text if found or specified key otherwise.</returns>
        /// <example>Example of key: "/UserControls/MyControl/Header".</example>
        public static string L(this object o, string key)
        {
            return Get.The.Translator.Translate(key);
        }

        /// <summary>
        /// Translates the text using specified key (the path of translation element in EPiServer language file),
        /// replacing format items with the string representation of a corresponding object in a specified array.
        /// </summary>
        /// <param name="o">Any object can use this extension.</param>
        /// <param name="key">The key.</param>
        /// <param name="arguments">The object array with arguments for replacement items.</param>
        /// <returns>Translated and formatted text if found or [Missing text] otherwise.</returns>
        /// <example>
        /// Example of key: "/UserControls/MyControl/Header".
        /// Example of formatted value: "Minimum length is {0}".
        /// </example>
        public static string L(this object o, string key, params object[] arguments)
        {
            return Get.The.Translator.Translate(key, arguments);
        }

        /// <summary>
        /// Translates the text using specified key (the path of translation element in EPiServer language file).
        /// In case when translated text wasn't found - returns provided fallback.
        /// </summary>
        /// <param name="o">Any object can use this extension.</param>
        /// <param name="key">The key.</param>
        /// <param name="fallback">The fallback.</param>
        /// <returns>Translated text if found or fallback text otherwise.</returns>
        /// <remarks>If translation wasn't found for the specified key, this method will create all needed elements
        /// in EPiServer language files for each site locale, inserting fallback text as a value.</remarks>
        public static string L(this object o, string key, string fallback)
        {
            return Get.The.Translator.Translate(key, fallback);
        }

        /// <summary>
        /// Translates the text using specified key (the path of translation element in EPiServer language file),
        /// replacing format items with the string representation of a corresponding object in a specified array.
        /// In case when translated text wasn't found - returns provided fallback.
        /// </summary>
        /// <param name="o">Any object can use this extension.</param>
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
        public static string L(this object o, string key, string fallback, params object[] arguments)
        {
            return Get.The.Translator.Translate(key, fallback, arguments);
        }
    }
}
