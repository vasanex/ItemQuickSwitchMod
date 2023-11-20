using BepInEx;
using HarmonyLib;

namespace ItemQuickSwitchMod
{
    [BepInPlugin(StaticPluginInfo.GUID, StaticPluginInfo.NAME, PluginInfo.PLUGIN_VERSION)]
    public class ItemQuickSwitchMod : BaseUnityPlugin
    {
        private Harmony _harmony;

        private void Awake()
        {
            // Creating configurable bindings:
            Logger.LogInfo($"{StaticPluginInfo.NAME} is creating binds!");
            foreach (var action in CustomAction.AllActions)
            {
                var bind = Config.Bind(
                    "Bindings",
                    action.Id,
                    action.Shortcut,
                    action.Description
                );
                action.ConfigEntry = bind;
            }

            _harmony = new Harmony(StaticPluginInfo.GUID);
            _harmony.PatchAll();
            Logger.LogInfo($"Plugin {StaticPluginInfo.NAME} is loaded!");
        }
    }
}