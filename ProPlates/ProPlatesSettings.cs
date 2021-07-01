using System;
using MelonLoader;

namespace ProPlates
{
    public static class ProPlatesSettings

    {
        public static int MaxPronouns { get; set; } = 8;

        public static void Register()
        {
            MelonPreferences.CreateCategory(BuildInfo.Name, BuildInfo.Name);
            MelonPreferences.CreateEntry(BuildInfo.Name, "MaxPronouns", 8, "Max pronouns to display (0 to disable)");
        }

        public static void Apply()
        {
            MaxPronouns = Math.Max(0, MelonPreferences.GetEntryValue<int>(BuildInfo.Name, "MaxPronouns"));
            ProPlatesMod.ReloadPronouns();
        }
    }
}