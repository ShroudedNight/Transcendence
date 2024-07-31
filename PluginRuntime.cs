using System;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;

namespace com.shroudednight.transcendence;

internal class PluginRuntime
{
    private const float ATTRIBUTE_MAXIMUM = 1E+14f;
    private readonly ManualLogSource Logger;
    private long _updateCount;

    internal PluginRuntime(ManualLogSource log)
    {
        Logger = log;
    }

    public void BeforePlayerStart(Player player)
    {
        _updateCount = 0;
    }

    public void AfterPlayerStart(Player player)
    {
        player.desiredWalkSpeed = 8f;
        player.desiredRunSpeed = 16f;

        var gameplayControls = GameplayControls.Instance;
        gameplayControls.fallDamageMultiplier = 0f;
        gameplayControls.jumpHeight = 16f;
        gameplayControls.defaultInventorySlots = 12;
    }

    public void BeforePlayerFixedUpdate(Player player)
    {
        ++_updateCount;
        var gameplayController = GameplayController.Instance;
        
        player.SetMaxHealth(ATTRIBUTE_MAXIMUM, true);
        player.SetMaxNerve(ATTRIBUTE_MAXIMUM, true);
        player.SetCombatHeft(1f);

        gameplayController.currentLockpickStrength = 1.0f;
        
        
        player.hygiene += (1.0f - player.hygiene) / 2;
        player.breath += (1.0f - player.breath) / 2;
        player.brokenLeg /= 2;
        player.bruised /= 2;
        player.bleeding /= 2;
        player.blackEye /= 2;
        player.wet /= 2;

        var currentHydration = Mathf.Sqrt(player.hydration);
        player.hydration = currentHydration;
        player.nourishment = currentHydration;
        player.alertness = currentHydration;
        player.energy = currentHydration;
        player.excitement = currentHydration;
        player.heat = currentHydration;
        player.chores = currentHydration;

        player.drunk = 1.0f - currentHydration;
        player.headache = 1.0f - currentHydration;
        player.starchAddiction = 1.0f - currentHydration;
        // player.poisoned = 1.0f - currentHydration;
    }

    public void AfterPlayerFixedUpdate(Player player)
    {
    }

    public void Log(LogLevel level, string str)
    {
        Logger.Log(level, str);
    }
}