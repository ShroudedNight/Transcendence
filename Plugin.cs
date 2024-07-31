using System;
using BepInEx;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using Il2CppInterop.Runtime;
using UnityEngine;

namespace com.shroudednight.transcendence;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BasePlugin
{
    private static PluginRuntime Runtime { get; set; }

    public override void Load()
    {
        try
        {
            Runtime = new PluginRuntime(Log);
            var harmony = new Harmony($"{MyPluginInfo.PLUGIN_GUID}");
            harmony.PatchAll();
        }
        catch (Exception e)
        {
            Log.FATAL($"Unable to load ${MyPluginInfo.PLUGIN_GUID}: {e}");
            throw;
        }
        Log.INFO($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
    }

    [HarmonyPatch(typeof(Player), "Start")]
    public class Player_PatchStart
    {
        public static void Prefix(Player __instance)
        {
            Runtime.BeforePlayerStart(__instance);
        }

        public static void Postfix(Player __instance)
        {
            Runtime.AfterPlayerStart(__instance);
        }
    }

    [HarmonyPatch(typeof(Player), "FixedUpdate")]
    public class Player_PatchFixedUpdate
    {
        public static void Prefix(Player __instance)
        {
            Runtime.BeforePlayerFixedUpdate(__instance);
        }

        public static void Postfix(Player __instance)
        {
            Runtime.AfterPlayerFixedUpdate(__instance);
        }
    }

    [HarmonyPatch(typeof(UpgradeEffectController), nameof(UpgradeEffectController.GetUpgradeEffect),
        [typeof(SyncDiskPreset.Effect)])]
    public class UpgradeEffectController_GetUpgradeEffect
    {
        public static bool Prefix(UpgradeEffectController __instance, ref SyncDiskPreset.Effect effect)
        {
            return true;
        }

        public static void Postfix(UpgradeEffectController __instance, ref SyncDiskPreset.Effect effect, ref float __result)
        {
            __result = effect switch
            {
                SyncDiskPreset.Effect.reduceMedicalCosts => 1.0f,
                SyncDiskPreset.Effect.awakenAtHome => 1.0f,
                SyncDiskPreset.Effect.priceModifier => 1.0f,
                SyncDiskPreset.Effect.dialogChanceModifier => 1.0f,
                SyncDiskPreset.Effect.doorBargeModifier => 1.0f,
                SyncDiskPreset.Effect.throwPowerModifier => 3.0f,
                SyncDiskPreset.Effect.reachModifier => 10.0f,
                SyncDiskPreset.Effect.incomingDamageModifier => 0.0f,
                SyncDiskPreset.Effect.footSizePerception => 1.0f,
                SyncDiskPreset.Effect.heightPerception => 1.0f,
                SyncDiskPreset.Effect.wealthPerception => 1.0f,
                SyncDiskPreset.Effect.salaryPerception => 1.0f,
                SyncDiskPreset.Effect.agePerception => 1.0f,
                SyncDiskPreset.Effect.singlePerception => 1.0f,
                SyncDiskPreset.Effect.lockpickingSpeedModifier => 1.0f,
                SyncDiskPreset.Effect.lockpickingEfficiencyModifier => 1.0f,
                SyncDiskPreset.Effect.triggerIllegalOnPick => 1.0f,
                SyncDiskPreset.Effect.KOTimeModifier => 3.0f,
                SyncDiskPreset.Effect.securityBreakerModifier => 3.0f,
                SyncDiskPreset.Effect.securityGraceTimeModifier => 3.0f,
                _ => __result
            };
        }
    }

    [HarmonyPatch(typeof(FurnishingsController), nameof(FurnishingsController.UpdateListDisplay))]
    public class FurnishingsController_UpdateListDisplay
    {
        public static bool prefix(FurnishingsController __instance)
        {
            __instance.displayClasses.Clear();
            foreach (var decorClass in (FurniturePreset.DecorClass[])Enum.GetValues(typeof(FurniturePreset.DecorClass)))
            {
                __instance.displayClasses.Add(decorClass);
            }
            foreach (var preset in Toolbox.Instance.allFurniture)
            {
                preset.purchasable = true;
            }

            return true;
        }
    }
    
    [HarmonyPatch(typeof(ApartmentItemsController), nameof(ApartmentItemsController.UpdateListDisplay))]
    public class ApartmentItemsController_UpdateListDisplay
    {
        public static bool prefix(ApartmentItemsController __instance)
        {
            foreach (var preset in Toolbox.Instance.objectPresetDictionary)
            {
                preset.Value.spawnable = true;
                preset.Value.allowInApartmentShop = true;
            }

            return true;
        }
    }
}