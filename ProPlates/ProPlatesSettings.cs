using MelonLoader;
using System;

namespace ProPlates;

internal static class Settings
{
    private static readonly MelonPreferences_Category Prefs = MelonPreferences.CreateCategory(BuildInfo.Name, BuildInfo.Name);
    public static MelonPreferences_Entry<int> MaxPronouns;

    public static event Action OnConfigChanged;

    public static void Register()
    {
        MaxPronouns = Prefs.CreateEntry(nameof(MaxPronouns), 3, "Max pronouns to display (0 to disable)");

        foreach (MelonPreferences_Entry e in Prefs.Entries) e.OnValueChangedUntyped += () => OnConfigChanged?.Invoke();
    }
}