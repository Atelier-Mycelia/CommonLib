namespace AtMycelia
{
    /// <summary>
    /// Convenience base for IMaintenanceModule: override only the phases you care about.
    /// </summary>
    public abstract class MaintenanceModule : IMaintenanceModule
    {
        public const int EarlyPriority = -1000;
        public const int DefaultPriority = 0;
        public const int LatePriority = 1000;

        public virtual int Priority => DefaultPriority;
        public virtual void RunStartup() { }
        public virtual void RunSceneMaintenance() { }
    }
}
