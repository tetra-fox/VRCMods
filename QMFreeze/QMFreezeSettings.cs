using System;
using MelonLoader;

namespace QMFreeze;

internal static class Settings
{
    private static readonly MelonPreferences_Category Prefs = MelonPreferences.CreateCategory(BuildInfo.Name, BuildInfo.Name);
    public static MelonPreferences_Entry<bool> Enabled;
    public static MelonPreferences_Entry<bool> RestoreVelocity;

    public static event Action OnConfigChanged;

    public static void Register()
    {
        Enabled = Prefs.CreateEntry(nameof(Enabled), true, "Enable QMFreeze");
        RestoreVelocity = Prefs.CreateEntry(nameof(RestoreVelocity), false, "Restore velocity");

        foreach (MelonPreferences_Entry e in Prefs.Entries) e.OnValueChangedUntyped += () => OnConfigChanged?.Invoke();
    }
}