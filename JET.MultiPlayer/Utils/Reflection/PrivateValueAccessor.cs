using System;
using System.Linq;
using System.Reflection;

namespace ServerLib.Utils.Reflection
{
    public static class PrivateValueAccessor
    {
        public const BindingFlags Flags =
            BindingFlags.GetProperty | BindingFlags.SetProperty |
            BindingFlags.GetField | BindingFlags.SetField |
            BindingFlags.NonPublic | BindingFlags.Public |
            BindingFlags.FlattenHierarchy | BindingFlags.IgnoreCase;

        public static PropertyInfo GetPrivatePropertyInfo(Type type, string propertyName)
        {
            var props = type.GetProperties(BindingFlags.Instance | Flags);
            return props.FirstOrDefault(propInfo => propInfo.Name == propertyName);
        }

        public static PropertyInfo GetStaticPropertyInfo(Type type, string propertyName)
        {
            var props = type.GetProperties(BindingFlags.Static | Flags);
            return props.FirstOrDefault(propInfo => propInfo.Name == propertyName);
        }

        public static object GetStaticPropertyValue(Type type, string propertyName)
        {
            return GetStaticPropertyInfo(type, propertyName).GetValue(null);
        }

        public static object GetPrivatePropertyValue(Type type, string propertyName, object o)
        {
            return GetPrivatePropertyInfo(type, propertyName).GetValue(o);
        }

        public static FieldInfo GetPrivateFieldInfo(Type type, string fieldName)
        {
            FieldInfo field;

            do
            {
                field = type.GetField(fieldName, BindingFlags.Instance | Flags);
                type = type.BaseType;
            } while (field == null && type != null);

            return field;
        }

        public static object GetPrivateFieldValue(Type type, string fieldName, object o)
        {
            return GetPrivateFieldInfo(type, fieldName).GetValue(o);
        }
    }
}
