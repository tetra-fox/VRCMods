using MelonLoader;
using System;

namespace AdBlocker;

internal static class Settings
{
    private static readonly MelonPreferences_Category Prefs = MelonPreferences.CreateCategory(BuildInfo.Name, $"{BuildInfo.Name} Settings");
    public static MelonPreferences_Entry<bool> RemoveCarousel;
    public static MelonPreferences_Entry<bool> RemoveVrcPlusBanner;
    public static MelonPreferences_Entry<bool> RemoveVrcPlusSupporter;
    public static MelonPreferences_Entry<bool> RemoveVrcPlusGift;
    public static MelonPreferences_Entry<bool> RemoveVrcPlusTab;
    public static MelonPreferences_Entry<bool> RemoveVrcPlusPfp;
    public static MelonPreferences_Entry<bool> RemoveVrcPlusGetMoreFavorites;

    public static event Action OnConfigChanged;

    public static bool Changed;

    public static void Register()
    {
        // If using emmVRC leave vrcPlusSupporter and vrcPlusTab off because it will throw errors since
        // they use EnableDisable Listeners when opening the menu and we just make the objects go poof
        RemoveCarousel = Prefs.CreateEntry(nameof(RemoveCarousel), true, "Remove QM carousel");
        RemoveVrcPlusBanner = Prefs.CreateEntry(nameof(RemoveVrcPlusBanner), true, "Remove VRC+ banner");
        RemoveVrcPlusGift = Prefs.CreateEntry(nameof(RemoveVrcPlusGift), true, "Remove VRC+ gift buttons");
        RemoveVrcPlusGetMoreFavorites = Prefs.CreateEntry(nameof(RemoveVrcPlusGetMoreFavorites), true, @"Remove ""Get More Favorites"" button");
        RemoveVrcPlusPfp = Prefs.CreateEntry(nameof(RemoveVrcPlusPfp), false, "Remove VRC+ PFP button");
        RemoveVrcPlusSupporter = Prefs.CreateEntry(nameof(RemoveVrcPlusSupporter), false, "Remove VRC+ supporter button");
        RemoveVrcPlusTab = Prefs.CreateEntry(nameof(RemoveVrcPlusTab), false, "Remove VRC+ tab");

        OnConfigChanged += () => Changed = true;

        foreach (MelonPreferences_Entry e in Prefs.Entries) e.OnValueChangedUntyped += () => OnConfigChanged?.Invoke();
    }
}