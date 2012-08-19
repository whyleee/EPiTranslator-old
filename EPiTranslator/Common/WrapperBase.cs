using System;
using System.Collections.Generic;
using System.Linq;

namespace EPiTranslator.Common
{
    /// <summary>
    /// The base class for all wrappers that wraps objects.
    /// </summary>
    public class WrapperBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WrapperBase"/> class.
        /// </summary>
        public WrapperBase() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="WrapperBase"/> class
        /// based on the specified object.
        /// </summary>
        /// <param name="toWrap">Object to wrap.</param>
        public WrapperBase(object toWrap)
        {
            if (toWrap == null)
            {
                throw new ArgumentNullException("toWrap");
            }

            Wrapped = toWrap;
        }

        /// <summary>
        /// Gets or sets the wrapped object.
        /// </summary>
        /// <value>The wrapped object.</value>
        public object Wrapped { get; set; }
    }

    /// <summary>
    /// The base class for all strongly-typed wrappers that wraps objects.
    /// </summary>
    /// <typeparam name="T">The type of object to wrap.</typeparam>
    public class WrapperBase<T> : WrapperBase
        where T : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WrapperBase{T}"/> class.
        /// </summary>
        public WrapperBase() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="WrapperBase{T}"/> class
        /// based on the specified object.
        /// </summary>
        /// <param name="toWrap">Object to wrap.</param>
        public WrapperBase(T toWrap) : base(toWrap) { }

        /// <summary>
        /// Gets or sets the wrapped object.
        /// </summary>
        /// <value>The wrapped object.</value>
        public new T Wrapped
        {
            get
            {
                return (T) base.Wrapped;
            }
            set
            {
                base.Wrapped = value;
            }
        }
    }

    /// <summary>
    /// Extension methods for <see cref="WrapperBase"/> and <see cref="WrapperBase{T}"/> objects.
    /// </summary>
    public static class WrapperExtensions
    {
        /// <summary>
        /// Unwraps all objects in provided collection.
        /// </summary>
        /// <param name="wrappedOrNotWrappers">Collection with wrapped objects or other generic objects (not wrappers).</param>
        /// <returns>Collection of unwrapped objects.</returns>
        /// <remarks>Those objects that are not wrappers will be return without any conversion or exceptions.</remarks>
        public static IEnumerable<object> UnwrapAll(this IEnumerable<object> wrappedOrNotWrappers)
        {
            return wrappedOrNotWrappers.Select(x => x is WrapperBase ? (x as WrapperBase).Wrapped : x);
        }
    }
}
