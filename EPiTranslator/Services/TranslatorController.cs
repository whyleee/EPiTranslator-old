using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http;

namespace EPiTranslator.Services
{
    public class TranslatorController : ApiController
    {
        public IEnumerable<string> Get()
        {
            return new[] {"Hello", "world"};
        }
    }
}
