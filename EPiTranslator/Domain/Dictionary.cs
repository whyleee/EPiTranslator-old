using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EPiTranslator
{
    /// <summary>
    /// Represents a self-contained collection of translations for single language.
    /// </summary>
    public class Dictionary
    {
        /// <summary>
        /// Gets or sets dictionary language.
        /// </summary>
        /// <value>
        /// Dictionary language.
        /// </value>
        /// <remarks>Refers to <see cref="EPiTranslator.Language.Id"/> property.</remarks>
        public string Language { get; set; }

        /// <summary>
        /// Gets or sets all entries of the dictionary.
        /// </summary>
        /// <value>
        /// All entries of the dictionary.
        /// </value>
        public IEnumerable<Translation> Entries { get; set; }
    }
}
