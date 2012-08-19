using EPiServer.Core;

namespace EPiTranslator.EPiServer
{
    /// <summary>
    /// The testable wrapper for <see cref="EPiServer.Core.LanguageManager" /> class.
    /// </summary>
    public class LanguageManagerWrapper
    {
        /// <summary>
        /// Translates the text using specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>Translated text if found or specified key otherwise.</returns>
        public virtual string Translate(string key)
        {
            return LanguageManager.Instance.Translate(key);
        }

        /// <summary>
        /// Translates the text for the target language using specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="language">Target language.</param>
        /// <returns>Translated text for target language if found or specified key otherwise.</returns>
        public virtual string Translate(string key, string language)
        {
            return LanguageManager.Instance.Translate(key, language);
        }

        /// <summary>
        /// Translates the text using specified key, using fallback in case of translation wasn't found.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="fallback">The fallback.</param>
        /// <returns>Translated text if found or fallback text otherwise.</returns>
        public virtual string TranslateWithFallback(string key, string fallback)
        {
            return LanguageManager.Instance.TranslateFallback(key, fallback);
        }

        /// <summary>
        /// Translates the text for the target language using specified key, using fallback in case of translation wasn't found.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="fallback">The fallback.</param>
        /// <param name="language">Target language.</param>
        /// <returns>Translated text for target language if found or fallback text otherwise.</returns>
        public virtual string TranslateWithFallback(string key, string fallback, string language)
        {
            return LanguageManager.Instance.TranslateFallback(key, fallback, language);
        }

        /// <summary>
        /// Translates the text using specified key, returning <c>null</c> if translation wasn't found.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>Translated text if found or <c>null</c> otherwise.</returns>
        public virtual string TranslateRaw(string key)
        {
            return LanguageManager.Instance.TranslateRaw(key);
        }

        /// <summary>
        /// Translates the text for the target language using specified key, returning <c>null</c> if translation wasn't found.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="language">Target language.</param>
        /// <returns>Translated text for target language if found or <c>null</c> otherwise.</returns>
        public virtual string TranslateRaw(string key, string language)
        {
            return LanguageManager.Instance.TranslateRaw(key, language);
        }
    }
}
