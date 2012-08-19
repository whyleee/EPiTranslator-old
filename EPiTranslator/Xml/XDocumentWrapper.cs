using System.Linq;
using System.Xml.Linq;
using EPiTranslator.Common;

namespace EPiTranslator.Xml
{
    /// <summary>
    /// The testable wrapper for <see cref="XDocument"/> class.
    /// </summary>
    public class XDocumentWrapper : WrapperBase<XDocument>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="XDocumentWrapper"/> class.
        /// </summary>
        public XDocumentWrapper() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="XDocumentWrapper"/> class
        /// using specified XML document.
        /// </summary>
        /// <param name="document">The document.</param>
        public XDocumentWrapper(XDocument document) : base(document) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="XDocumentWrapper"/> class
        /// using specified inner content.
        /// </summary>
        /// <param name="content">Inner content.</param>
        public XDocumentWrapper(params object[] content)
        {
            Wrapped = new XDocument(content.UnwrapAll().ToArray());
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XDocumentWrapper"/> class
        /// using specified XML declaration and inner content.
        /// </summary>
        /// <param name="declaration">XML declaration.</param>
        /// <param name="content">Inner content.</param>
        public XDocumentWrapper(XDeclaration declaration, params object[] content)
        {
            Wrapped = new XDocument(declaration, content.UnwrapAll().ToArray());
        }

        /// <summary>
        /// Gets the root element of the XML tree of this document.
        /// </summary>
        /// <value>The root element of the XML tree of this document.</value>
        public virtual XElementWrapper Root
        {
            get
            {
                var rawRoot = Wrapped.Root;
                return rawRoot != null ? new XElementWrapper(rawRoot) : null;
            }
        }

        /// <summary>
        /// Saves the document to the file.
        /// </summary>
        /// <param name="path">The path to the file to save XML.</param>
        public virtual void Save(string path)
        {
            Wrapped.Save(path);
        }
    }
}
