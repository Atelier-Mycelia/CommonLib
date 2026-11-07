using System.Collections.Generic;
using Type = System.Type;

namespace AtMycelia
{
    public static class TypeExtensions
    {
        public static bool IsConcrete(this Type type)
        {
            return !(type.IsAbstract || type.IsInterface);
        }

        public static IList<Type> GetInstantiatableTypes(this Type baseType)
        {
            return TypeUtils.GetInstantiatableTypes(baseType);
        }
    }
}
