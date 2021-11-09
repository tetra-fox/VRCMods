using System;
using MelonLoader;
using UIExpansionKit.API;

namespace VXP
{

    public static class Settings
    {
        private static readonly MelonPreferences_Category Prefs = MelonPreferences.CreateCategory(BuildInfo.Name, "Vocal eXpression Parameter");
        internal static MelonPreferences_Entry<bool> Enabled;
        // internal static GestureBehaviors GestureBehavior;
        internal static MelonPreferences_Entry<int> UpdateInterval;
        
        private static MelonPreferences_Entry<string> GestureBehavior_UIXFriendly;

        internal static event Action OnConfigChanged;

        internal static void Register()
        {
            Enabled = Prefs.CreateEntry(nameof(Enabled), true, "Enable VXP");
            // GestureBehavior = Prefs.CreateEntry(nameof(GestureBehavior), GestureBehaviors.VXPOnly, "Gesture behavior (what should your avatar prioritize)");
            GestureBehavior_UIXFriendly = Prefs.CreateEntry(nameof(GestureBehavior_UIXFriendly), nameof(GestureBehaviors.VXPOnly), "Gesture behavior (what should your avatar prioritize)");
            UpdateInterval = Prefs.CreateEntry(nameof(UpdateInterval), 1, "Update interval (in seconds)");
            
            foreach (MelonPreferences_Entry entry in Prefs.Entries)
                entry.OnValueChangedUntyped += () => OnConfigChanged?.Invoke();
            
            // HandsOnly and disabling the mod do essentially the same thing.
            
            // knah pls add enum support to uix
            if (!VRChatUtilityKit.Utilities.VRCUtils.IsUIXPresent) return;
            
            ExpansionKitApi.RegisterSettingAsStringEnum(Prefs.Identifier, nameof(GestureBehavior_UIXFriendly), new[]
            {
                (nameof(GestureBehaviors.VXPOnly), "VXP only"),
                (nameof(GestureBehaviors.HandsOnly), "Hands only"),
                (nameof(GestureBehaviors.HandsAndVXP), "Hands and VXP"),
            });

            GestureBehavior_UIXFriendly.OnValueChanged += (_, value) => MapGestureBehaviorEnum(value);
            MapGestureBehaviorEnum(GestureBehavior_UIXFriendly.Value);
        }

        private static void MapGestureBehaviorEnum(string value)
        {
            if (!Enum.TryParse(value, true, out GestureBehaviors behavior)) return;
            MelonLogger.Msg("Set behavior to {0}", behavior.ToString());
            GestureBehavior.Value = behavior;
        }
        
        // i'll remove this when uix gets proper enum support,
        // i just wanted to be able to use the same syntax
        // because i already wrote like 80% of the mod by 
        // the time i figured out uix didn't support enums lol
        internal static class GestureBehavior
        {
            internal static GestureBehaviors Value;
        }
        
        internal enum GestureBehaviors
        {
            HandsOnly,
            VXPOnly,
            HandsAndVXP
        }
    }
}