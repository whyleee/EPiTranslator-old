using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EPiTranslator
{
    /// <summary>
    /// Represents a single entry (word or phrase) in dictionary.
    /// </summary>
    public class DictionaryEntry
    {
        /// <summary>
        /// Gets or sets the keyword used for search the translation.
        /// </summary>
        /// <value>
        /// The keyword used for search the translation.
        /// </value>
        public string Keyword { get; set; }

        /// <summary>
        /// Gets or sets the category for the entry.
        /// </summary>
        /// <value>
        /// The category for the entry.
        /// </value>
        public string Category { get; set; }

        /// <summary>
        /// Gets or sets all translations of the keyword from all dictionaries.
        /// </summary>
        /// <value>
        /// All translations of the keyword from all dictionaries.
        /// </value>
        /// <remarks>
        /// This collection has translations for all registered languages in <see cref="Translator"/>.
        /// For words without translation for specific language here will be a <see cref="Translation"/> objects with empty <see cref="Translation.Value"/> property.
        /// This will simplify processing translations by clients (for example, showing them in the table).
        /// </remarks>
        public IEnumerable<Translation> Translations { get; set; }
    }
}