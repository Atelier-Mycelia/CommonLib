using UnityEngine;

namespace AtMycelia.EditorExt
{
    /// <summary>
    /// Calls ApplyBackwardsCompatibility on all Components implementing IBackwardsCompatibilityApplier.
    /// Discovered and run by MaintenanceRunner.
    /// </summary>
    public class BackwardsCompatibilityEnsurer : MaintenanceModule
    {
        public override void RunSceneMaintenance()
        {
#if UNITY_6000_OR_NEWER
            var appliers = Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);
#else
            var appliers = Object.FindObjectsOfType<MonoBehaviour>();
#endif
            foreach (var applier in appliers)
            {
                if (applier is IBackwardsCompatibilityApplier backwardsCompatibilityApplier)
                {
                    backwardsCompatibilityApplier.ApplyBackwardsCompatibility();
                }
            }
        }
    }
}
