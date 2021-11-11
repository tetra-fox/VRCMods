using System;
using MelonLoader;

namespace QMFreeze
{
    public static class Settings

    {
        private static readonly MelonPreferences_Category Prefs = MelonPreferences.CreateCategory(BuildInfo.Name, BuildInfo.Name);
        internal static MelonPreferences_Entry<bool> Enabled;
        internal static MelonPreferences_Entry<bool> RestoreVelocity;

        internal static event Action OnConfigChanged;

        internal static void Register()
        {
            Enabled = Prefs.CreateEntry(nameof(Enabled), true, "Enable QMFreeze");
            RestoreVelocity = Prefs.CreateEntry(nameof(RestoreVelocity), false, "Restore velocity");

            foreach (MelonPreferences_Entry e in Prefs.Entries) e.OnValueChangedUntyped += () => OnConfigChanged?.Invoke();
        }
    }
}