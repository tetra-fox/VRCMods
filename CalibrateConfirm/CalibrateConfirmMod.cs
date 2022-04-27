using MelonLoader;
using System;
using System.Collections;
using System.IO;
using System.Reflection;
using CalibrateConfirm.Components;
using UnhollowerRuntimeLib;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using VRChatUtilityKit.Ui;

[assembly: MelonInfo(typeof(CalibrateConfirm.Mod), CalibrateConfirm.BuildInfo.Name, CalibrateConfirm.BuildInfo.Version, CalibrateConfirm.BuildInfo.Author, CalibrateConfirm.BuildInfo.DownloadLink)]
[assembly: MelonGame("VRChat", "VRChat")]

namespace CalibrateConfirm;

internal static class BuildInfo
{
	public const string Name = "CalibrateConfirm";
	public const string Author = "tetra";
	public const string Version = "3.0.3";
	public const string DownloadLink = "https://github.com/tetra-fox/VRCMods";
}

public class Mod : MelonMod
{
	private static readonly MelonLogger.Instance Logger = new(BuildInfo.Name);

	public override void OnApplicationStart()
	{
		Settings.Register();
		VRChatUtilityKit.Utilities.VRCUtils.OnUiManagerInit += Init;
	}

	private static void Init()
	{
		if (!XRDevice.isPresent) return;
		Logger.Msg("Initializing...");

		// load our asset bundle
		AssetBundle assetBundle;
		using Stream stream = Assembly.GetExecutingAssembly()
			.GetManifestResourceStream(BuildInfo.Name + ".icon.assetbundle");

		using (MemoryStream tempStream = new((int) stream!.Length)) {
			stream.CopyTo(tempStream);
			assetBundle = AssetBundle.LoadFromMemory_Internal(tempStream.ToArray(), 0);
			assetBundle.hideFlags |= HideFlags.DontUnloadUnusedAsset;
		}

		// load sprite from the asset bundle
		Sprite confirmFbtSprite = assetBundle.LoadAsset_Internal("ConfirmFBT", Il2CppType.Of<Sprite>()).Cast<Sprite>();
		
		GameObject calibrateFbtButton = Helpers.FindInactive("UserInterface/Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_Dashboard/ScrollRect/Viewport/VerticalLayoutGroup/Buttons_QuickActions/SitStandCalibrateButton/Button_CalibrateFBT");
		GameObject sitStandGroup = Helpers.FindInactive("UserInterface/Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_Dashboard/ScrollRect/Viewport/VerticalLayoutGroup/Buttons_QuickActions/SitStandCalibrateButton");
		MakeConfirmButton(calibrateFbtButton, sitStandGroup, confirmFbtSprite);

		
		if (Settings.AddPromptToSettingsTab.Value) {
			GameObject calibrateFbtButtonSettings = Helpers.FindInactive("UserInterface/Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_Settings/Panel_QM_ScrollRect/Viewport/VerticalLayoutGroup/Buttons_FBT/Button_CalibrateFBT");
			GameObject groupSettings = Helpers.FindInactive("UserInterface/Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_Settings/Panel_QM_ScrollRect/Viewport/VerticalLayoutGroup/Buttons_FBT");
			MakeConfirmButton(calibrateFbtButtonSettings, groupSettings, confirmFbtSprite);
			
			Helpers.FindInactive("UserInterface/Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_Settings/Panel_QM_ScrollRect/Viewport/VerticalLayoutGroup/Buttons_FBT/Button_ConfirmFBT").transform.SetSiblingIndex(2);
		}

		Logger.Msg("Initialized!");
	}

	private static IEnumerator Timeout(int length, VRCButton confirmFbtButton, GameObject calibrateFbtButton)
	{
		int timeLeft = length;

		while (timeLeft > 0) {
			confirmFbtButton.Text = "Confirm?\n" + timeLeft;
			timeLeft--;
			yield return new WaitForSeconds(1);
		}

		confirmFbtButton.gameObject.SetActive(false);
		calibrateFbtButton.SetActive(true);
	}
	
	private static void MakeConfirmButton(GameObject calibrateFbtButton, GameObject buttonGroup, Sprite confirmFbtSprite)
	{
		MethodInfo calibrateMethod = Helpers.CalibrateMethod;

		object timeout = new();

		SingleButton confirmFbtButton = null;
		confirmFbtButton = new SingleButton(() =>
			{
				calibrateMethod.Invoke(VRCTrackingManager.field_Private_Static_VRCTrackingManager_0, null);
				confirmFbtButton.gameObject.SetActive(false);
				calibrateFbtButton.SetActive(true);
				MelonCoroutines.Stop(timeout);
			},
			confirmFbtSprite, "Confirm?", "Button_ConfirmFBT", "Are you sure you want to calibrate?");

		ButtonWatcher watcher = calibrateFbtButton.gameObject.AddComponent<ButtonWatcher>();
		watcher.reference = confirmFbtButton.gameObject;
		watcher.target = calibrateFbtButton;

		confirmFbtButton.gameObject.SetActive(false);

		UiManager.AddButtonToExistingGroup(buttonGroup, confirmFbtButton);
		
		// reset original calibrate button's onClick
		calibrateFbtButton.GetComponent<Button>().onClick = new Button.ButtonClickedEvent();

		calibrateFbtButton.GetComponent<Button>().onClick.AddListener((Action) delegate
		{
			confirmFbtButton.gameObject.SetActive(true);
			calibrateFbtButton.SetActive(false);
			timeout = MelonCoroutines.Start(Timeout(Settings.PromptLength.Value, confirmFbtButton, calibrateFbtButton));
		});
	}
		
	public override void OnPreferencesSaved()
	{
		if (!Settings.Changed) return;

		const string msg = "Preferences changed, please restart your game for changes to take effect";
		Logger.Warning(msg);
		Helpers.DisplayHudMessage($"[CalibrateConfirm]\n{msg}");

		Settings.Changed = false;
	}
}