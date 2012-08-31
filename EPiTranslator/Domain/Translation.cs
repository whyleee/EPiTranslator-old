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
        /// Gets or sets registered fallback for translation.
        /// </summary>
        /// <value>
        /// Registered fallback for translation.
        /// </value>
        public string Fallback { get; set; }

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
