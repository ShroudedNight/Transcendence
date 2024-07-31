using BepInEx.Logging;

namespace com.shroudednight.transcendence;

public static class Extensions
{
    public static void INFO(this ManualLogSource logSource, string data) => logSource.LogInfo(data);
    public static void WARN(this ManualLogSource logSource, string data) => logSource.LogWarning(data);
    public static void ERROR(this ManualLogSource logSource, string data) => logSource.LogError(data);
    public static void FATAL(this ManualLogSource logSource, string data) => logSource.LogFatal(data);
}