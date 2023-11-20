using BepInEx.Configuration;
using UnityEngine.InputSystem;

namespace ItemQuickSwitchMod
{
    public sealed class CustomAction
    {
        public static readonly ActionItem Emote1 = new ActionItem("Emote1", Key.F1, "Dance emote", 0);
        public static readonly ActionItem Emote2 = new ActionItem("Emote2", Key.F2, "Point emote", 0);
        public static readonly ActionItem Slot1 = new ActionItem("Slot1", Key.Digit1, "Equip Slot 1", 0);
        public static readonly ActionItem Slot2 = new ActionItem("Slot2", Key.Digit2, "Equip Slot 2", 1);
        public static readonly ActionItem Slot3 = new ActionItem("Slot3", Key.Digit3, "Equip Slot 3", 2);
        public static readonly ActionItem Slot4 = new ActionItem("Slot4", Key.Digit4, "Equip Slot 4", 3);

        public static readonly ActionItem[] AllActions = { Emote1, Emote2, Slot1, Slot2, Slot3, Slot4 };

        public class ActionItem
        {
            public string Id { get; }
            public Key Shortcut { get; }
            public string Description { get; }
            public int SlotNumber { get; }

            public ConfigEntry<Key> ConfigEntry { get; set; }

            public ActionItem(string id, Key config, string description, int slotNumber)
                => (Id, Shortcut, Description, SlotNumber) = (id, config, description, slotNumber);
        }
    }
}
