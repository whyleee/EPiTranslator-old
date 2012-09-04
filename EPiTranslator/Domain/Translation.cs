using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EPiTranslator
{
    /// <summary>
    /// Represents a single translation entity.
    /// </summary>
    public class Translation
    {
        /// <summary>
        /// Gets or sets full translation key.
        /// </summary>
        /// <value>
        /// Full translation key.
        /// </value>
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the keyword used for search the translation.
        /// </summary>
        /// <value>
        /// The keyword used for search the translation.
        /// </value>
        public string Keyword { get; set; }

        /// <summary>
        /// Gets or sets the translated phrase for translation <see cref="Language"/>.
        /// </summary>
        /// <value>
        /// The translated phrase for translation <see cref="Language"/>.
        /// </value>
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this translation is actually a fallback.
        /// </summary>
        /// <value>
        /// <c>true</c> if this translation is fallback; otherwise, <c>false</c>.
        /// </value>
        public bool IsFallback { get; set; }

        /// <summary>
        /// Gets or sets translation language.
        /// </summary>
        /// <value>
        /// Translation language.
        /// </value>
        /// <remarks>Refers to <see cref="EPiTranslator.Language.Id"/> property.</remarks>
        public string Language { get; set; }

        /// <summary>
        /// Gets or sets translation category.
        /// </summary>
        /// <value>
        /// Translation category.
        /// </value>
        public string Category { get; set; }
    }
}
