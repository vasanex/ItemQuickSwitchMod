using HarmonyLib;
using GameNetcodeStuff;
using System.Collections.Generic;
using System.Reflection;
using System;
using UnityEngine.InputSystem;
using System.Diagnostics;
using BepInEx.Logging;
using UnityEngine;

namespace ItemQuickSwitchMod.Patches;

[HarmonyPatch(typeof(PlayerControllerB))]
internal class PlayerControllerBPatch
{
    private static readonly Dictionary<string, MethodInfo> MethodCache = new();
    private static readonly Dictionary<string, FieldInfo> FieldCache = new();

    private static readonly object[] BackwardsParam = new object[1] { false };
    private static readonly object[] ForwardsParam = new object[1] { true };

    private static object InvokePrivateMethod(PlayerControllerB instance, string methodName, object[] parameters = null)
    {
        MethodCache.TryGetValue(methodName, out var method);
        method ??= typeof(PlayerControllerB).GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);

        MethodCache[methodName] = method;
        return method?.Invoke(instance, parameters);
    }

    private static object GetPrivateField(PlayerControllerB instance, string fieldName)
    {
        FieldCache.TryGetValue(fieldName, out var field);
        field ??= typeof(PlayerControllerB).GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);

        FieldCache[fieldName] = field;
        return field?.GetValue(instance);
    }

    [HarmonyPatch("Update")]
    [HarmonyPostfix]
    public static void PlayerControllerB_Update(PlayerControllerB __instance)
    {

        if ((!__instance.IsOwner || !__instance.isPlayerControlled ||
            __instance.IsServer && !__instance.isHostPlayerObject) && !__instance.isTestingPlayer) return;

        var keyDown = Array.Find(CustomAction.AllActions, it =>
            Keyboard.current[it.ConfigEntry.Value].wasPressedThisFrame);

        if (keyDown == null) return;

        switch(keyDown?.Id)
        {
            case "Emote1":
                PerformEmote(__instance, 1);
                break;
            case "Emote2":
                PerformEmote(__instance, 2);
                break;
            default:
                StopEmotes(__instance);
                SwitchItemSlots(__instance, keyDown.SlotNumber);
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

    private static void SwitchItemSlots(PlayerControllerB __instance, int requestedSlot)
    {
        if (!isItemSwitchPossible(__instance) || __instance.currentItemSlot == requestedSlot) return;

        var distance = __instance.currentItemSlot - requestedSlot;

        if (distance > 0)
        {
            var param = (distance > 0) ? BackwardsParam : ForwardsParam;
            do
            {
                InvokePrivateMethod(__instance, "SwitchItemSlotsServerRpc", param);
                distance += (distance > 0) ? -1 : 1;
            } while (distance > 0);
        }

        ShipBuildModeManager.Instance.CancelBuildMode();
        __instance.playerBodyAnimator.SetBool("GrabValidated", false);

        var switchSlotsTimerField = typeof(PlayerControllerB).GetField("timeSinceSwitchingSlots", BindingFlags.NonPublic | BindingFlags.Instance);
        switchSlotsTimerField?.SetValue(__instance, 0.0f);

        var switchItemParams = new object[] { requestedSlot, __instance.ItemSlots[requestedSlot] };
        InvokePrivateMethod(__instance, "SwitchToItemSlot", switchItemParams);

        if (__instance.currentlyHeldObjectServer != null)
        {
            __instance.currentlyHeldObjectServer.gameObject.GetComponent<AudioSource>()
                .PlayOneShot(__instance.currentlyHeldObjectServer.itemProperties.grabSFX, 0.6f);
        }
    }

    private static bool isItemSwitchPossible(PlayerControllerB __instance)
    {
        var switchSlotsTimer = (float)GetPrivateField(__instance, "timeSinceSwitchingSlots");
        var throwingObject = (bool)GetPrivateField(__instance, "throwingObject");

        return !(switchSlotsTimer < 0.01 || __instance.inTerminalMenu || __instance.isGrabbingObjectAnimation ||
                 __instance.inSpecialInteractAnimation || throwingObject || __instance.isTypingChat ||
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
