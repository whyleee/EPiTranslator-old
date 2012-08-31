using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EPiTranslator
{
    /// <summary>
    /// Represents an available language for translations.
    /// </summary>
    public class Language
    {
        /// <summary>
        /// Gets or sets language ID (short culture name, like 'en', 'da').
        /// </summary>
        /// <value>
        /// Langauge ID.
        /// </value>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets language name.
        /// </summary>
        /// <value>
        /// Language name.
        /// </value>
        public string Name { get; set; }
    }
}
