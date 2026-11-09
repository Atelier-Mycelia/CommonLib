namespace AtMycelia
{
    /// <summary>
    /// Core startup work that other modules may depend on, so it runs before 
    /// everything else.
    /// </summary>
    public class CoreStartupMaintenance : MaintenanceModule
    {
        public override int Priority => int.MinValue;
        // ^We want to make sure that this runs first, given how the modules
        // are run in ascending Priority order

        public override void RunStartup()
        {
            UnityThreadUtil.Init();
            GameObjectRootBootstrapper.EnsureRoot();
        }
    }
}
