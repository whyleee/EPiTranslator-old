using System;
using System.Collections.Generic;
using System.Linq;

namespace EPiTranslator
{
    // Internal implementation of the factory.
    // NOTE: Do not add getters here, as this file can be overwritten by the new version of the factory.
    public partial class Get
    {
        /// <summary>
        /// Shared instance for all factories. Can be changed in runtime.
        /// Do not use this field directly, use <see cref="GetFactory{TFactory}" /> method instead.
        /// </summary>
        protected static Get _factory;

        /// <summary>
        /// Gets or sets the factory instance.
        /// </summary>
        /// <value>The factory instance.</value>
        public static Get The
        {
            get { return GetFactory<Get>(); }
            set { _factory = value; }
        }

        /// <summary>
        /// Gets the factory by requested type.
        /// </summary>
        /// <typeparam name="TFactory">The type of the factory.</typeparam>
        /// <returns>
        /// Returns the factory of the requested type. If no factory was set earlier or
        /// that factory is of another type, this method creates new factory of the
        /// requested type and returns it.
        /// </returns>
        /// <remarks>
        /// Tricks with type checks needed for safe 'parent-slave' casts and polymorphism.
        /// </remarks>
        protected static TFactory GetFactory<TFactory>() where TFactory : Get, new()
        {
            if (_factory == null || !(_factory is TFactory))
            {
                _factory = new TFactory();
            }

            return (TFactory)_factory;
        }
    }
}
