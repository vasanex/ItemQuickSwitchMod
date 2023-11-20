using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;

namespace ItemQuickSwitchMod
{
    [BepInPlugin(StaticPluginInfo.GUID, StaticPluginInfo.NAME, PluginInfo.PLUGIN_VERSION)]
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

            mls = BepInEx.Logging.Logger.CreateLogSource(PluginInfo.PLUGIN_VERSION);
            // Creating configurable bindings:
            mls.LogInfo($"{StaticPluginInfo.NAME} is creating binds!");
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
            mls.LogInfo($"Plugin {StaticPluginInfo.NAME} is loaded!");
        }
    }
}