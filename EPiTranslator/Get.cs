using System;
using System.Web;
using EPiTranslator.Common;
using EPiTranslator.EPiServer;
using EPiTranslator.Xml;

namespace EPiTranslator
{
    /// <summary>
    /// Factory for common services with fluent interface.
    /// </summary>
    /// <example>
    /// <para>
    /// Use it like a native english speech:
    /// <code>
    /// var requestUrl = Get.The.HttpContext.Request.Url;
    /// </code>
    /// </para>
    /// <para>
    /// When testing, create a mock to this factory and mock entire chain:
    /// <code>
    /// <![CDATA[
    /// var fluentMocker = new Mock<Get>();
    /// fluentMocker.Setup(m => m.HttpContext.Request.Url).Returns('request.url');
    /// ]]>
    /// </code>
    /// </para>
    /// </example>
    public partial class Get
    {
        /// <summary>
        /// Gets the current time.
        /// </summary>
        /// <returns>The current time.</returns>
        public virtual DateTime Time
        {
            get { return DateTime.Now; }
        }

        /// <summary>
        /// Gets current HTTP context.
        /// </summary>
        /// <value><see cref="HttpContextBase"/> object that represents current HTTP context.</value>
        public virtual HttpContextBase HttpContext
        {
            get
            {
                return System.Web.HttpContext.Current != null ? new HttpContextWrapper(System.Web.HttpContext.Current) : null;
            }
        }

        /// <summary>
        /// Gets the translator.
        /// </summary>
        /// <value>
        /// The translator.
        /// </value>
        public virtual Translator Translator
        {
            get
            {
                return new Translator();
            }
        }

        /// <summary>
        /// Gets the language manager.
        /// </summary>
        /// <value>
        /// The language manager.
        /// </value>
        public virtual LanguageManagerWrapper LanguageManager
        {
            get
            {
                return new LanguageManagerWrapper();
            }
        }

        /// <summary>
        /// Gets the file manager.
        /// </summary>
        /// <value>
        /// The file manager.
        /// </value>
        public virtual FileManagerWrapper FileManager
        {
            get
            {
                return new FileManagerWrapper();
            }
        }

        /// <summary>
        /// Gets the XML helper.
        /// </summary>
        /// <value>
        /// The XML helper.
        /// </value>
        public virtual XmlHelper XmlHelper
        {
            get
            {
                return new XmlHelper();
            }
        }

        /// <summary>
        /// Gets the EPiServer wrapper.
        /// </summary>
        /// <value>
        /// The wrapper for EPiServer.
        /// </value>
        public virtual EPiServerWrapper EPiServer
        {
            get
            {
                return new EPiServerWrapper();
            }
        }
    }
}
