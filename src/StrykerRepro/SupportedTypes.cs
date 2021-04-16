namespace StrykerRepro
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    using Newtonsoft.Json.Linq;

    internal sealed partial class SupportedTypes : ReadOnlyDictionary<Type, string>
    {
        public static readonly SupportedTypes Map = new SupportedTypes();

        public static bool IsSupported(Type type)
        {
            return Map.ContainsKey(type);
        }

        public static bool IsSupported(object value)
        {
            var valueToCheck = value;
            if (valueToCheck == null)
            {
                return true;
            }

            var valueType = valueToCheck.GetType();
            if (valueType.IsArray)
            {
                return IsSupported(valueType.GetElementType());
            }

            if (valueToCheck is JArray jarray)
            {
                valueToCheck = jarray.ToObject<IEnumerable<object>>();
            }

            if (valueToCheck is IEnumerable enumerable)
            {
                return enumerable.OfType<object>().All(x => IsSupported(x.GetType()));
            }

            return IsSupported(valueType);
        }
    }
}
