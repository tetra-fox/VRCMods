using MelonLoader;
using System;

namespace ProPlates
{
    public static class Settings

    {
        private static MelonPreferences_Category Prefs = MelonPreferences.CreateCategory(BuildInfo.Name, BuildInfo.Name);
        internal static MelonPreferences_Entry<int> MaxPronouns;

        internal static event Action OnConfigChanged; 

        internal static void Register()
        {
            MaxPronouns = Prefs.CreateEntry("MaxPronouns", 8, "Max pronouns to display (0 to disable)");
            foreach (MelonPreferences_Entry e in Prefs.Entries)
                e.OnValueChangedUntyped += delegate { OnConfigChanged?.Invoke(); };
        }
    }
}