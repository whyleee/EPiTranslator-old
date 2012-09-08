﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using Machine.Specifications;
using NSubstitute;

namespace EPiTranslator.Tests
{
    public class with_factory_mocker
    {
        protected static Get factory;

        Establish context = () =>
        {
            factory = SubstituteAll.For<Get>();
            Get.The = factory;
        };

        protected static IList<T> CollectionWith<T>()
        {
            return new List<T>();
        }
    }

    public static class LanguageSetups
    {
        public static void ReturnsSomeLanguages(this IEnumerable<Language> method)
        {
            method.ReturnsEnglishAndDanishLanguages();
        }

        public static void ReturnsEnglishAndDanishLanguages(this IEnumerable<Language> method)
        {
            var enLanguage = new Language { Id = "en", Name = "English" };
            var daLanguage = new Language { Id = "da", Name = "Danish" };

            method.Returns(new[] { enLanguage, daLanguage });
        }
    }

    public static class TranslationSetups
    {
        public static void ReturnsNothing(this IEnumerable<Dictionary> method)
        {
            method.Returns(new Dictionary[] { });
        }
    }

    /// <summary>
    /// Creates recursive mocks for 3-long object call chain.
    /// </summary>
    public static class SubstituteAll
    {
        private static int recursiveCounter = 0;

        public static T For<T>() where T : class
        {
            recursiveCounter = 0;
            return (T) For(typeof (T));
        }

        private static object For(Type type)
        {
            ++recursiveCounter;

            if (type == typeof(string))
            {
                return string.Empty;
            }

            if (typeof(IEnumerable).IsAssignableFrom(type))
            {
                var valueType = type.GetElementType() ?? type.GetGenericArguments().FirstOrDefault();

                if (valueType != null)
                {
                    var list = (IList) Activator.CreateInstance(typeof(List<>).MakeGenericType(valueType));

                    if (type.IsArray)
                    {
                        return typeof(Enumerable).GetMethod("ToArray").MakeGenericMethod(valueType).Invoke(null, new[] {list});
                    }
                    else
                    {
                        return list;
                    }
                }
            }

            if (type.IsSealed)
            {
                return null;
            }

            if (recursiveCounter == 4)
            {
                return null;
            }

            var sub = Substitute.For(new[] {type}, new object[0]);

            var gettableVirtualProps = type.GetProperties()
                .Where(x => x.CanRead && x.GetGetMethod().IsVirtual && x.PropertyType.IsClass)
                .Select(x => x);

            foreach (var prop in gettableVirtualProps)
            {
                if (prop.GetIndexParameters().Length > 0)
                {
                    continue;
                }

                var subForProperty = SubstituteAll.For(prop.PropertyType);
                --recursiveCounter;

                if (subForProperty == null)
                {
                    continue;
                }

                object value = null;

                try
                {
                    value = prop.GetValue(sub, null);
                }
                catch (TargetInvocationException ex)
                {
                    if (ex.InnerException != null &&
                        ex.InnerException.GetType() == typeof(ArgumentException) &&
                        ex.InnerException.Message.Contains("implements ISerializable, but failed to provide a deserialization constructor"))
                    {
                        // don't know what to do.
                    }
                }

                value.Returns(subForProperty);
            }
            return sub;
        }
    }
}