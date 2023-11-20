using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;

namespace ItemQuickSwitchMod
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class ItemQuickSwitchMod : BaseUnityPlugin
    {
        private Harmony _harmony;
        public static ItemQuickSwitchMod Instance;
        internal ManualLogSource mls;

        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
            }

            mls = BepInEx.Logging.Logger.CreateLogSource(PluginInfo.PLUGIN_GUID);
            // Creating configurable bindings:
            mls.LogInfo($"{PluginInfo.PLUGIN_NAME} is creating binds!");
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

            _harmony = new Harmony(PluginInfo.PLUGIN_GUID);
            _harmony.PatchAll();
            mls.LogInfo($"Plugin {PluginInfo.PLUGIN_NAME} is loaded!");
        }
    }
}