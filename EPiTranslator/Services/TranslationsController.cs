using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http;

namespace EPiTranslator.Services
{
    public class TranslationsController : ApiController
    {
        public IDictionary<string, IEnumerable<Translation>> GetAll()
        {
            return Get.The.Translator.GetAllTranslations();
        }
    }
}
