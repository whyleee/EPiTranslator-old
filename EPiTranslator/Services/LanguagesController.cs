using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http;

namespace EPiTranslator.Services
{
    /// <summary>
    /// Service to retrieve available languages for translations.
    /// </summary>
    public class LanguagesController : ApiController
    {
        /// <summary>
        /// Gets all available languages for translations.
        /// </summary>
        /// <returns>
        /// Collection of <see cref="Language"/> objects representing available languages for translations.
        /// </returns>
        public IEnumerable<Language> GetAll()
        {
            return Get.The.Translator.GetAllLanguages();
        }
    }
}
