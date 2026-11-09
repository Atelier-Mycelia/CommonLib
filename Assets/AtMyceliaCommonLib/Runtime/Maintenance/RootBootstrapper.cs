using UnityEngine;

namespace AtMycelia
{
    /// <summary>
    /// Prepares a persistent root GameObject for other systems by Atelier Mycelia
    /// to attach <i>their</i> roots to. A clean Hierarchy is a happy Hierarchy!
    /// </summary>
    public static class GameObjectRootBootstrapper
    {
        /// <summary>
        /// Called by CoreStartupMaintenance before the first scene loads 
        /// (courtesy of MaintenanceRunner). This is public so we don't have 
        /// to worry about whether or not other root bootstrappers will run
        /// before this one. If they do, they can just call this func before 
        /// trying to do anything with this root.
        /// </summary>
        public static void EnsureRoot()
        {
            if (Root != null)
            {
                return;
            }

            Root = new GameObject(RootObjName);
            Object.DontDestroyOnLoad(Root);
        }

        public static GameObject Root { get; private set; }

        private static readonly string RootObjName = "AtMycelia";
    }
}