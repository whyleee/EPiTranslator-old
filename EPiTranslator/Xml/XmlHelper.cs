using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace EPiTranslator.Xml
{
    /// <summary>
    /// Helpre class to work with XML.
    /// </summary>
    public class XmlHelper
    {
        /// <summary>
        /// Loads the XML using specified path.
        /// </summary>
        /// <param name="path">The path to the XML file to load.</param>
        /// <returns><see cref="XDocumentWrapper" /> object with loaded XML.</returns>
        public virtual XDocumentWrapper LoadXml(string path)
        {
            return new XDocumentWrapper(XDocument.Load(path));
        }

        /// <summary>
        /// Creates the writer for XML.
        /// </summary>
        /// <param name="output">The output stream.</param>
        /// <param name="settings">The settings for the writer.</param>
        /// <returns>The writer for XML.</returns>
        public virtual XmlWriter CreateWriter(Stream output, XmlWriterSettings settings)
        {
            return XmlWriter.Create(output, settings);
        }

        /// <summary>
        /// Creates the writer for XML.
        /// </summary>
        /// <param name="output">The output string.</param>
        /// <param name="settings">The settings for the writer.</param>
        /// <returns>The writer for XML.</returns>
        public virtual XmlWriter CreateWriter(StringBuilder output, XmlWriterSettings settings)
        {
            return XmlWriter.Create(output, settings);
        }

        /// <summary>
        /// Loads the XSLT file using specified path, settings and stylesheet resolver.
        /// </summary>
        /// <param name="path">The URL to the XSLT file.</param>
        /// <param name="settings">XSLT settings.</param>
        /// <param name="stylesheetResolver">The stylesheet resolver.</param>
        /// <returns><see cref="XslCompiledTransform" /> object that can transform the input.</returns>
        public virtual XslCompiledTransform LoadXslt(string path, XsltSettings settings, XmlResolver stylesheetResolver)
        {
            var xslt = new XslCompiledTransform();
            xslt.Load(path, settings, stylesheetResolver);

            return xslt;
        }

        /// <summary>
        /// Transforms the input using the specified XSLT file.
        /// </summary>
        /// <param name="xslt">The XSLT file.</param>
        /// <param name="input">The input XML to transform.</param>
        /// <param name="arguments">The arguments that will be passed to XSLT processor.</param>
        /// <param name="output">The <see cref="TextWriter" /> object to which output will be written.</param>
        public virtual void Transform(XslCompiledTransform xslt, IXPathNavigable input, XsltArgumentList arguments, TextWriter output)
        {
            xslt.Transform(input, arguments, output);
        }

        /// <summary>
        /// Transforms the input using the specified XSLT file.
        /// </summary>
        /// <param name="xslt">The XSLT file.</param>
        /// <param name="input">The input XML to transform.</param>
        /// <param name="arguments">The arguments that will be passed to XSLT processor.</param>
        /// <param name="output">The <see cref="XmlWriter" /> object to which output will be written.</param>
        public virtual void Transform(XslCompiledTransform xslt, IXPathNavigable input, XsltArgumentList arguments, XmlWriter output)
        {
            xslt.Transform(input, arguments, output);
        }

        /// <summary>
        /// Serializes the specified object.
        /// </summary>
        /// <param name="o">The object to serialize.</param>
        /// <returns>Output of the serialized object.</returns>
        public virtual XPathNodeIterator ToXPathNode(object o)
        {
            var output = new StringBuilder();
            using (var writer = CreateWriter(output, new XmlWriterSettings()))
            {
                Serialize(writer, o);
            }

            return XDocument.Parse(output.ToString()).Root.CreateNavigator().Select(".");
        }

        /// <summary>
        /// Serializes the specified object.
        /// </summary>
        /// <param name="writer">The XML writer.</param>
        /// <param name="o">The object to serialize.</param>
        public virtual void Serialize(XmlWriter writer, object o)
        {
            var serializer = new XmlSerializer(o.GetType());

            var dummyNamespace = new XmlSerializerNamespaces();
            dummyNamespace.Add(string.Empty, string.Empty);

            serializer.Serialize(writer, o, dummyNamespace);
        }
    }
}
