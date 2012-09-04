using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using EPiTranslator.Common;

namespace EPiTranslator.Xml
{
    /// <summary>
    /// The testable wrapper for <see cref="XElement"/> class.
    /// </summary>
    public class XElementWrapper : WrapperBase<XElement>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="XElementWrapper"/> class.
        /// </summary>
        public XElementWrapper() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="XElementWrapper"/> class
        /// using specified XML element.
        /// </summary>
        /// <param name="element">The element.</param>
        public XElementWrapper(XElement element) : base(element) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="XElementWrapper"/> class
        /// with the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        public XElementWrapper(string name)
        {
            Wrapped = new XElement(name);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XElementWrapper"/> class
        /// with the specified name and inner content.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="content">The content.</param>
        public XElementWrapper(string name, params object[] content)
        {
            Wrapped = new XElement(name, content.UnwrapAll().ToArray());
        }

        /// <summary>
        /// Gets or sets the name of this element.
        /// </summary>
        /// <value>The name of this element.</value>
        public virtual string Name
        {
            get
            {
                return Wrapped.Name.ToString();
            }
            set
            {
                Wrapped.Name = value;
            }
        }

        /// <summary>
        /// Gets or sets the value of this element.
        /// </summary>
        /// <value>The value of this element.</value>
        public virtual string Value
        {
            get
            {
                return Wrapped.Value;
            }
            set
            {
                Wrapped.Value = value;
            }
        }

        /// <summary>
        /// Gets the parent element for this element.
        /// </summary>
        /// <value>
        /// The parent element for this element.
        /// </value>
        public virtual XElementWrapper Parent
        {
            get
            {
                var parent = Wrapped.Parent;
                return parent != null ? new XElementWrapper(parent) : null;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this element has at least one child element.
        /// </summary>
        /// <value>
        /// <c>true</c> if this element has at least one child element; otherwise, <c>false</c>.
        /// </value>
        public virtual bool HasElements
        {
            get
            {
                return Wrapped.HasElements;
            }
        }

        /// <summary>
        /// Gets the first child for this element with the specified name.
        /// </summary>
        /// <param name="name">The name of child element to search.</param>
        /// <returns>The first child for this element with the specified name.</returns>
        public virtual XElementWrapper Child(string name)
        {
            var rawElement = Wrapped.Element(name);
            return rawElement != null ? new XElementWrapper(rawElement) : null;
        }

        /// <summary>
        /// Returns a collection of the child elements for this element.
        /// </summary>
        /// <returns>Collection of the child elements for this element.</returns>
        public virtual IEnumerable<XElementWrapper> Children()
        {
            return Wrapped.Elements().Select(child => new XElementWrapper(child));
        }

        /// <summary>
        /// Returns a collection of all descendant elements for this element.
        /// </summary>
        /// <returns>Collection of all descendant elements for this element.</returns>
        public virtual IEnumerable<XElementWrapper> Descendants()
        {
            return Wrapped.Descendants().Select(element => new XElementWrapper(element));
        }

        /// <summary>
        /// Returns element attribute for the specified name.
        /// </summary>
        /// <param name="name">Attribute name.</param>
        /// <returns>Element attribute for the specified name.</returns>
        public virtual XAttribute Attribute(string name)
        {
            return Wrapped.Attribute(name);
        }

        /// <summary>
        /// Adds the specified element as a child to this node.
        /// </summary>
        /// <param name="element">The element to add as a child to this node.</param>
        public virtual void Add(XElementWrapper element)
        {
            Wrapped.Add(element.Wrapped);
        }

        /// <summary>
        /// Adds the specified element immediately before this node.
        /// </summary>
        /// <param name="element">The element to add before this node.</param>
        public virtual void AddBeforeSelf(XElementWrapper element)
        {
            Wrapped.AddBeforeSelf(element.Wrapped);
        }

        /// <summary>
        /// Adds the specified element immediately after this node.
        /// </summary>
        /// <param name="element">The element to add after this node.</param>
        public virtual void AddAfterSelf(XElementWrapper element)
        {
            Wrapped.AddAfterSelf(element.Wrapped);
        }

        /// <summary>
        /// Sets the value of an attribute, adds an attribute, or removes an attribute.
        /// </summary>
        /// <param name="name">Existing or new attribute name.</param>
        /// <param name="value">Attribute value or empty to remove.</param>
        public virtual void SetAttributeValue(string name, object value)
        {
            Wrapped.SetAttributeValue(name, value);
        }
    }
}
