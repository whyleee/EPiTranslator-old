using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http;

namespace EPiTranslator.Services
{
    public class LanguagesController : ApiController
    {
        public IEnumerable<Language> GetAll()
        {
            return Get.The.Translator.GetAllLanguages();
        }
    }
}
