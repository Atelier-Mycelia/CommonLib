using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

namespace AtMycelia
{
    /// <summary>
    /// The master maintenance module. It discovers every IMaintenanceModule in all loaded
    /// assemblies, instantiates them, sorts them by Priority (then type name), and runs them
    /// one by one. This is the only maintenance entry point, so we never rely on Unity's
    /// unspecified ordering between separate init hooks (or on frame delays).
    /// <para>
    /// Other packages hook in by implementing IMaintenanceModule (or deriving MaintenanceModule).
    /// A module that throws is logged and skipped; the rest still run.
    /// </para>
    /// </summary>
#if UNITY_EDITOR
    [InitializeOnLoad]
#endif
    public static class MaintenanceRunner
    {
        static MaintenanceRunner()
        {
            ToggleSubs(false);
            ToggleSubs(true);
        }

        
        static void ToggleSubs(bool on)
        {
            if (on)
            {
                SceneManager.activeSceneChanged += OnActiveSceneChanged;
            }
            else
            {
                SceneManager.activeSceneChanged -= OnActiveSceneChanged;
            }

#if UNITY_EDITOR
            if (on)
            {
                AssemblyReloadEvents.afterAssemblyReload += RunSceneMaintenance;
                EditorSceneManager.sceneOpened += OnSceneOpened;
                EditorApplication.delayCall += RunSceneMaintenance;
            }
            else
            {
                AssemblyReloadEvents.afterAssemblyReload -= RunSceneMaintenance;
                EditorSceneManager.sceneOpened -= OnSceneOpened;
                EditorApplication.delayCall -= RunSceneMaintenance;
            }
#endif
        }

        static void OnActiveSceneChanged(Scene previous, Scene next) => RunSceneMaintenance();

        /// <summary>
        /// Runs on scene changes and (in the editor) after assembly reloads and
        /// editor init.
        /// </summary>
        public static void RunSceneMaintenance()
        {
            EnsureModules();
            RunAll(module => module.RunSceneMaintenance(), "RunSceneMaintenance");
        }

        private static void EnsureModules()
        {
            if (_modules != null)
            {
                return;
            }

            _modules = DiscoverModules();
        }

        /// <summary>
        /// The discovered modules (sorted by execution order first, name second).
        /// </summary>
        private static List<IMaintenanceModule> _modules;

        private static List<IMaintenanceModule> DiscoverModules()
        {
            var modules = new List<IMaintenanceModule>();

            foreach (var type in FindModuleTypes())
            {
                var constr = type.GetConstructor(Type.EmptyTypes);
                if (!type.IsConcrete() || constr == null)
                {
                    continue;
                }

                try
                {
                    var newInstance = (IMaintenanceModule)Activator.CreateInstance(type);
                    modules.Add(newInstance);
                }
                catch (Exception excep)
                {
                    Debug.LogError($"[MaintenanceRunner] Could not instantiate {type.FullName}: {excep}");
                }
            }

            modules.Sort(CompareModules);
            return modules;
        }

        private static IEnumerable<Type> FindModuleTypes()
        {
            IEnumerable<Type> result = null;
            // We can't use TypeCache in player builds, hence us needing to
            // take different approaches based on whether this is running
            // in such a build or not.
            if (Application.isEditor)
            {
#if UNITY_EDITOR
                result = TypeCache.GetTypesDerivedFrom<IMaintenanceModule>();
#endif
            }
            else
            {
                result = FindModuleTypesForPlayerBuild();
            }

            return result;
        }

        private static IEnumerable<Type> FindModuleTypesForPlayerBuild()
        {
            var found = new List<Type>();
            var moduleType = typeof(IMaintenanceModule);
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assemblyEl in assemblies)
            {
                Type[] types;
                try
                {
                    types = assemblyEl.GetTypes();
                }
                catch (ReflectionTypeLoadException excep)
                {
                    types = excep.Types; // may contain nulls
                }
                catch (Exception)
                {
                    continue;
                }

                foreach (var typeEl in types)
                {
                    if (typeEl != null && moduleType.IsAssignableFrom(typeEl))
                    {
                        found.Add(typeEl);
                    }
                }
            }

            return found;
        }

        private static int CompareModules(IMaintenanceModule first, IMaintenanceModule second)
        {
            int byPriority = first.Priority.CompareTo(second.Priority);
            string firstFullName = first.GetType().FullName;
            string secondFullName = second.GetType().FullName;
            bool sameNumericPriority = byPriority == 0;
            // ^When false, we fall back to sorting by name
            return sameNumericPriority
                ? string.CompareOrdinal(firstFullName, secondFullName)
                : byPriority;
        }

        private static void RunAll(Action<IMaintenanceModule> phase, string phaseName)
        {   
            foreach (var elem in Modules)
            {
                try
                {
                    phase(elem);
                }
                catch (Exception excep)
                {
                    var modType = elem.GetType();
                    string logMessage = $"[MaintenanceRunner]{modType.FullName}.{phaseName} failed: {excep}";
                    Debug.LogError(logMessage);
                }
            }
        }

#if UNITY_EDITOR
        static void OnSceneOpened(Scene scene, OpenSceneMode mode) => RunSceneMaintenance();
#endif

        /// <summary>
        /// Runs once per Play Mode/player start, before the first scene loads.
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void RunStartupMaintenance()
        {
            RunAll(module => module.RunStartup(), "RunStartup");
            ToggleSubs(false);
            ToggleSubs(true);
        }

        public static IReadOnlyList<IMaintenanceModule> Modules
        {
            get
            {
                return _modules;
            }
        }

    }
}
