using BepInEx.Configuration;
using UnityEngine.InputSystem;

namespace ItemQuickSwitchMod;

public sealed class CustomAction
{
    public static readonly CustomAction Emote1 = new(
        "m_Movement_Emote1",
        UnityEngine.InputSystem.Key.F1,
        "Dance emote",
        0
    );

    public static readonly CustomAction Emote2 = new(
        "m_Movement_Emote2",
        UnityEngine.InputSystem.Key.F2,
        "Point emote",
        0
    );

    public static readonly CustomAction Slot1 = new(
        "m_Movement_Slot1",
        UnityEngine.InputSystem.Key.Digit1,
        "Equip Slot 1",
        0
    );

    public static readonly CustomAction Slot2 = new(
        "m_Movement_Slot2",
        UnityEngine.InputSystem.Key.Digit2,
        "Equip Slot 2",
        1
    );

    public static readonly CustomAction Slot3 = new(
        "m_Movement_Slot3",
        UnityEngine.InputSystem.Key.Digit3,
        "Equip Slot 3",
        2
    );

    public static readonly CustomAction Slot4 = new(
        "m_Movement_Slot4",
        UnityEngine.InputSystem.Key.Digit4,
        "Equip Slot 4",
        3
    );

    public static readonly CustomAction[] AllActions = { Emote1, Emote2, Slot1, Slot2, Slot3, Slot4 };

    public string Id { get; }
    public Key Shortcut { get; }
    public string Description { get; }
    public int SlotNumber { get; }

    public ConfigEntry<Key> ConfigEntry { get; set; }

    private CustomAction(string id, Key config, string description, int slotNumber)
    {
        Id = id;
        Shortcut = config;
        Description = description;
        SlotNumber = slotNumber;
    }
}