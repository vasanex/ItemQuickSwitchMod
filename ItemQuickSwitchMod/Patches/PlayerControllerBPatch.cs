using HarmonyLib;
using GameNetcodeStuff;
using System.Collections.Generic;
using System.Reflection;
using System;
using UnityEngine.InputSystem;
using UnityEngine;

// ReSharper disable InconsistentNaming
namespace ItemQuickSwitchMod.Patches;

[HarmonyPatch(typeof(PlayerControllerB))]
internal class PlayerControllerBPatch
{
    private static readonly Dictionary<string, MethodInfo> MethodCache = new();
    // private static readonly Dictionary<string, FieldInfo> FieldCache = new();

    private static readonly object[] BackwardsParam = { false };
    private static readonly object[] ForwardsParam = { true };

    private static object InvokePrivateMethod(PlayerControllerB instance, string methodName, object[] parameters = null)
    {
        MethodCache.TryGetValue(methodName, out var method);
        method ??= typeof(PlayerControllerB).GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);

        MethodCache[methodName] = method;
        return method?.Invoke(instance, parameters);
    }

    // currently unused because switched to ref access:
    //
    // private static T GetPrivateFieldValue<T>(PlayerControllerB instance, string fieldName)
    // {
    //     FieldCache.TryGetValue(fieldName, out var field);
    //     field ??= typeof(PlayerControllerB).GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
    //
    //     FieldCache[fieldName] = field;
    //     return (T)field?.GetValue(instance);
    // }

    [HarmonyPatch("Update")]
    [HarmonyPostfix]
    public static void PlayerControllerB_Update(
        PlayerControllerB __instance,
        ref float ___timeSinceSwitchingSlots,
        ref bool ___throwingObject
    )
    {
        if ((!__instance.IsOwner || !__instance.isPlayerControlled ||
             __instance.IsServer && !__instance.isHostPlayerObject) && !__instance.isTestingPlayer) return;

        var keyDown = Array.Find(CustomAction.AllActions, it =>
            Keyboard.current[it.ConfigEntry.Value].wasPressedThisFrame);

        if (keyDown == null) return;

        switch (keyDown.Id)
        {
            case "Emote1":
                PerformEmote(__instance, 1);
                break;
            case "Emote2":
                PerformEmote(__instance, 2);
                break;
            default:
                StopEmotes(__instance);
                if (SwitchItemSlots(__instance, keyDown.SlotNumber, ___timeSinceSwitchingSlots, ___throwingObject))
                {
                    ___timeSinceSwitchingSlots = 0f;
                }
                break;
        }
    }

    private static void PerformEmote(PlayerControllerB __instance, int emoteId)
    {
        __instance.timeSinceStartingEmote = 0.0f;
        __instance.performingEmote = true;
        __instance.playerBodyAnimator.SetInteger("emoteNumber", emoteId);
        __instance.StartPerformingEmoteServerRpc();
    }

    private static bool SwitchItemSlots(PlayerControllerB __instance, int requestedSlot, float timeSinceSwitchingSlots,
        bool isThrowingObject
    )
    {
        if (!IsItemSwitchPossible(__instance, timeSinceSwitchingSlots, isThrowingObject) ||
            __instance.currentItemSlot == requestedSlot)
        {
            return false;
        }

        var distance = __instance.currentItemSlot - requestedSlot;
        var requestedSlotIsLowerThanCurrent = (distance > 0);

        if (Math.Abs(distance) == __instance.ItemSlots.Length - 1)
        {
            // we can just skip one slot forwards/backwards here and save RPC calls.
            var parameterArray = requestedSlotIsLowerThanCurrent ? ForwardsParam : BackwardsParam;
            InvokePrivateMethod(__instance, "SwitchItemSlotsServerRpc", parameterArray);
        }
        else
        {
            var parameterArray = requestedSlotIsLowerThanCurrent ? BackwardsParam : ForwardsParam;
            do
            {
                InvokePrivateMethod(__instance, "SwitchItemSlotsServerRpc", parameterArray);
                distance += requestedSlotIsLowerThanCurrent ? -1 : 1;
            } while (distance != 0);
        }

        ShipBuildModeManager.Instance.CancelBuildMode();
        __instance.playerBodyAnimator.SetBool("GrabValidated", false);

        var switchItemParams = new object[] { requestedSlot, null };
        InvokePrivateMethod(__instance, "SwitchToItemSlot", switchItemParams);

        if (__instance.currentlyHeldObjectServer != null)
            __instance.currentlyHeldObjectServer.gameObject.GetComponent<AudioSource>()
                .PlayOneShot(__instance.currentlyHeldObjectServer.itemProperties.grabSFX, 0.6f);

        return true;
    }

    private static bool IsItemSwitchPossible(PlayerControllerB __instance, float timeSinceSwitchingSlots,
        bool isThrowingObject)
    {
        return !(timeSinceSwitchingSlots < 0.01 || __instance.inTerminalMenu || __instance.isGrabbingObjectAnimation ||
                 __instance.inSpecialInteractAnimation || isThrowingObject || __instance.isTypingChat ||
                 __instance.twoHanded || __instance.activatingItem || __instance.jetpackControls ||
                 __instance.disablingJetpackControls);
    }

    private static void StopEmotes(PlayerControllerB __instance)
    {
        __instance.performingEmote = false;
        __instance.StopPerformingEmoteServerRpc();
        __instance.timeSinceStartingEmote = 0.0f;
    }
}