namespace AtMycelia
{
    /// <summary>
    /// A unit of maintenance that MaintenanceRunner discovers automatically (any concrete,
    /// non-generic class with a public parameterless constructor, in any loaded assembly)
    /// and runs in a predictable order. Packages that depend on this one can implement this
    /// instead of using their own [InitializeOnLoad]/[RuntimeInitializeOnLoadMethod] hooks.
    /// Consider deriving from MaintenanceModule so you only override what you need.
    /// </summary>
    public interface IMaintenanceModule
    {
        /// <summary>
        /// Lower values run first. Ties are broken by type name, so order is always deterministic.
        /// </summary>
        int Priority { get; }

        /// <summary>
        /// Runs once per Play Mode/player start, before the first scene loads.
        /// </summary>
        void RunStartup();

        /// <summary>
        /// Runs after assembly reloads, scene loads/changes and (in the editor) once editor
        /// initialization settles. Must be safe to call repeatedly.
        /// </summary>
        void RunSceneMaintenance();
    }
}
