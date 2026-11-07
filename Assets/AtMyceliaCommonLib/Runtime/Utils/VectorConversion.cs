using System;
using UnityEngine;

namespace AtMycelia
{
    /// <summary>
    /// Turns Vector2s, Vector3s and Vector4s into one another, which is what
    /// <see cref="TypeUtils.TypesCompatible"/> promises is possible. Components the target
    /// doesn't have are dropped, and ones the source doesn't have are zero.
    /// </summary>
    public static class VectorConversion
    {
        /// <summary>
        /// False if either side isn't a vector type, in which case the caller falls back
        /// on whatever else it does. A value that's already of the target type is returned
        /// as it is.
        /// </summary>
        public static bool TryConvert(object value, Type targetType, out object converted)
        {
            converted = null;
            if (value == null || targetType == null)
            {
                return false;
            }

            if (targetType == typeof(Vector4))
            {
                return TryToVector4(value, out converted);
            }

            if (targetType == typeof(Vector3))
            {
                return TryToVector3(value, out converted);
            }

            if (targetType == typeof(Vector2))
            {
                return TryToVector2(value, out converted);
            }

            return false;
        }

        private static bool TryToVector4(object value, out object converted)
        {
            switch (value)
            {
                case Vector4 four: converted = four; return true;
                case Vector3 three: converted = new Vector4(three.x, three.y, three.z, 0f); return true;
                case Vector2 two: converted = new Vector4(two.x, two.y, 0f, 0f); return true;
                default: converted = null; return false;
            }
        }

        private static bool TryToVector3(object value, out object converted)
        {
            switch (value)
            {
                case Vector3 three: converted = three; return true;
                case Vector4 four: converted = new Vector3(four.x, four.y, four.z); return true;
                case Vector2 two: converted = new Vector3(two.x, two.y, 0f); return true;
                default: converted = null; return false;
            }
        }

        private static bool TryToVector2(object value, out object converted)
        {
            switch (value)
            {
                case Vector2 two: converted = two; return true;
                case Vector3 three: converted = new Vector2(three.x, three.y); return true;
                case Vector4 four: converted = new Vector2(four.x, four.y); return true;
                default: converted = null; return false;
            }
        }
    }
}
