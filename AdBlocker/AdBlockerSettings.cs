using MelonLoader;
using System;

namespace AdBlocker
{
	internal static class Settings

	{
		private static readonly MelonPreferences_Category Prefs = MelonPreferences.CreateCategory(BuildInfo.Name, "AdBlocker Settings");
		public static MelonPreferences_Entry<bool> RemoveCarousel;
		public static MelonPreferences_Entry<bool> RemoveVrcPlusBanner;
		public static MelonPreferences_Entry<bool> RemoveVrcPlusSupporter;
		public static MelonPreferences_Entry<bool> RemoveVrcPlusGift;
		public static MelonPreferences_Entry<bool> RemoveVrcPlusTab;
		public static MelonPreferences_Entry<bool> RemoveVrcPlusPfp;
		
		public static event Action OnConfigChanged;

		public static void Register()
		{
			// If using emmVRC leave vrcPlusSupporter and vrcPlusTab off because it will throw errors since
			// they use EnableDisable Listeners when opening the menu and we just make the objects go poof
			RemoveCarousel = Prefs.CreateEntry(nameof(RemoveCarousel), true, "Remove QM carousel");
			RemoveVrcPlusBanner = Prefs.CreateEntry(nameof(RemoveVrcPlusBanner), true, "Remove VRC+ banner");
			RemoveVrcPlusSupporter = Prefs.CreateEntry(nameof(RemoveVrcPlusTab), false, "Remove VRC+ supporter button");
			RemoveVrcPlusGift = Prefs.CreateEntry(nameof(RemoveVrcPlusSupporter), true, "Remove VRC+ gift buttons");
			RemoveVrcPlusTab = Prefs.CreateEntry(nameof(RemoveVrcPlusPfp), false, "Remove VRC+ tab");
			RemoveVrcPlusPfp = Prefs.CreateEntry(nameof(RemoveVrcPlusGift), true, "Remove VRC+ PFP button");

			OnConfigChanged += () =>
			{
				MelonLogger.Warning("Preferences changed, please restart your game for changes to take effect");
			};
			
			foreach (MelonPreferences_Entry e in Prefs.Entries) e.OnValueChangedUntyped += () => OnConfigChanged?.Invoke();
		}
	}
}