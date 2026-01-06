using UnityEngine;

namespace AtMycelia.Common
{
    public static class GameObjectExtensions 
    {
        /// <summary>
        /// Like regular GetComponent, except that if the component is not found, an instance
        /// of it is added and then returned.
        /// </summary>
        public static T GetOrAddComponent<T>(this GameObject gameObject) where T: Component
        {
            if (!gameObject.TryGetComponent(out T result))
            {
                result = gameObject.AddComponent<T>();
            }

            return result;
        }

        public static T GetOrAddComponent<T>(this Component comp) where T : Component
        {
            if (!comp.gameObject.TryGetComponent(out T result))
            {
                result = comp.gameObject.AddComponent<T>();
            }

            return result;
        }
    }
}