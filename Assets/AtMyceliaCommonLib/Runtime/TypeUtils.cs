using System;
using System.Collections.Generic;
using System.Reflection;
using Type = System.Type;

namespace AtMycelia
{
    public static class TypeUtils
    {
        public static bool TypesCompatible(Type firstType, Type secondType)
        {
            bool regularAssignability = firstType.IsAssignableFrom(secondType);
            bool castableNumericTypes = ArrayContains(_basicNumericTypes, firstType) &&
                ArrayContains(_basicNumericTypes, secondType);
            bool castableVectorTypes = ArrayContains(_vectorTypes, firstType) &&
                ArrayContains(_vectorTypes, secondType);
            bool result = regularAssignability || castableNumericTypes || castableVectorTypes;
            return result;
        }

        private static bool ArrayContains(Type[] types, Type toFind)
        {
            for (int i = 0; i < types.Length; i++)
            {
                if (types[i] == toFind)
                {
                    return true;
                }
            }

            return false;
        }

        // Compatible with each other the way the numeric types are: a value of one can be
        // turned into another (the components that don't exist in the target are dropped, and
        // the ones that don't exist in the source are zero). Converting is up to the caller.
        private static readonly Type[] _vectorTypes = new Type[]
        {
            typeof(UnityEngine.Vector2),
            typeof(UnityEngine.Vector3),
            typeof(UnityEngine.Vector4)
        };

        private static readonly Type[] _basicNumericTypes = new Type[]
        {
            typeof(sbyte), typeof(byte),
            typeof(short), typeof(ushort),
            typeof(int), typeof(uint),
            typeof(long), typeof(ulong),
            typeof(char), typeof(float),
            typeof(double), typeof(decimal)
        };

        public static IList<Type> GetInstantiatableTypes(Type baseType)
        {
            var result = new List<Type>();
            Assembly[] allAssemblies = AppDomain.CurrentDomain.GetAssemblies();

            for (int i = 0; i < allAssemblies.Length; i++)
            {
                IList<Type> typesInAssembly = SafeGetTypes(allAssemblies[i]);
                for (int j = 0; j < typesInAssembly.Count; j++)
                {
                    Type candidate = typesInAssembly[j];
                    if (IsInstantiatableType(candidate, baseType))
                    {
                        result.Add(candidate);
                    }
                }
            }

            return result;
        }

        private static IList<Type> SafeGetTypes(Assembly toGetTypesFrom)
        {
            try
            {
                return toGetTypesFrom.GetTypes();
            }
            catch (ReflectionTypeLoadException ex)
            {
                // The types that failed to load come back as nulls
                var loaded = new List<Type>(ex.Types.Length);
                for (int i = 0; i < ex.Types.Length; i++)
                {
                    if (ex.Types[i] != null)
                    {
                        loaded.Add(ex.Types[i]);
                    }
                }

                return loaded;
            }
        }

        public static bool IsInstantiatableType(Type typeToCheck, Type baseType)
        {
            return baseType.IsAssignableFrom(typeToCheck) && typeToCheck.IsConcrete();
        }
    }
}
