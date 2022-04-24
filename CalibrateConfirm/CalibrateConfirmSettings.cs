using System;
using MelonLoader;

namespace CalibrateConfirm;

internal static class Settings
{
	private static readonly MelonPreferences_Category Prefs = MelonPreferences.CreateCategory(BuildInfo.Name, BuildInfo.Name);
	public static MelonPreferences_Entry<int> PromptLength;
	public static MelonPreferences_Entry<bool> AddPromptToSettingsTab;

	public static event Action OnConfigChanged;

	public static bool Changed;

	public static void Register()
	{
		PromptLength = Prefs.CreateEntry(nameof(PromptLength), 5, "Prompt length");
		AddPromptToSettingsTab = Prefs.CreateEntry(nameof(AddPromptToSettingsTab), false, "Add prompt to settings tab calibrate button");

		OnConfigChanged += () => Changed = true;
			
		foreach (MelonPreferences_Entry e in Prefs.Entries) e.OnValueChangedUntyped += () => OnConfigChanged?.Invoke();
	}
}