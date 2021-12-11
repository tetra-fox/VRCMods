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

namespace CalibrateConfirm
{
	internal static class BuildInfo
	{
		public const string Name = "CalibrateConfirm";
		public const string Author = "tetra";
		public const string Version = "3.0.1";
		public const string DownloadLink = "https://github.com/tetra-fox/VRCMods";
	}

	public class Mod : MelonMod
	{
		private static readonly MelonLogger.Instance Logger = new(BuildInfo.Name);
		private static SingleButton _confirmFbtButton;
		private static GameObject _calibrateFbtButton;

		public override void OnApplicationStart() { VRChatUtilityKit.Utilities.VRCUtils.OnUiManagerInit += Init; }

		private static void Init()
		{
			if (!XRDevice.isPresent) return;
			Logger.Msg("Initializing...");

			_calibrateFbtButton = Helpers.FindInactive("UserInterface/Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_Dashboard/ScrollRect/Viewport/VerticalLayoutGroup/Buttons_QuickActions/SitStandCalibrateButton/Button_CalibrateFBT");

			// load our asset bundle
			AssetBundle assetBundle;
			using Stream stream = Assembly.GetExecutingAssembly()
				.GetManifestResourceStream("CalibrateConfirm.calibrateconfirm.assetbundle");

			using (MemoryStream tempStream = new((int) stream.Length)) {
				stream.CopyTo(tempStream);
				assetBundle = AssetBundle.LoadFromMemory_Internal(tempStream.ToArray(), 0);
				assetBundle.hideFlags |= HideFlags.DontUnloadUnusedAsset;
			}

			// load sprite from the asset bundle
			Sprite confirmFbtSprite = assetBundle.LoadAsset_Internal("ConfirmFBT", Il2CppType.Of<Sprite>()).Cast<Sprite>();

			MethodInfo calibrateMethod = Helpers.GetCalibrateMethod();

			object timeout = new();

			_confirmFbtButton = new SingleButton(() =>
				{
					calibrateMethod.Invoke(VRCTrackingManager.field_Private_Static_VRCTrackingManager_0, null);
					_confirmFbtButton.gameObject.SetActive(false);
					_calibrateFbtButton.SetActive(true);
					MelonCoroutines.Stop(timeout);
				},
				confirmFbtSprite, "Confirm?", "Button_ConfirmFBT", "Are you sure you want to calibrate?");

			ButtonWatcher watcher = _calibrateFbtButton.gameObject.AddComponent<ButtonWatcher>();
			watcher.reference = _confirmFbtButton.gameObject;
			watcher.target = _calibrateFbtButton;

			_confirmFbtButton.gameObject.SetActive(false);

			GameObject sitStandGroup = Helpers.FindInactive("UserInterface/Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_Dashboard/ScrollRect/Viewport/VerticalLayoutGroup/Buttons_QuickActions/SitStandCalibrateButton");

			UiManager.AddButtonToExistingGroup(sitStandGroup, _confirmFbtButton);

			// reset calibrate button's onClick
			_calibrateFbtButton.GetComponent<Button>().onClick = new Button.ButtonClickedEvent();

			_calibrateFbtButton.GetComponent<Button>().onClick.AddListener((Action) delegate
			{
				_confirmFbtButton.gameObject.SetActive(true);
				_calibrateFbtButton.SetActive(false);
				timeout = MelonCoroutines.Start(Timeout(5));
			});

			Logger.Msg("Initialized!");
		}

		private static IEnumerator Timeout(int length)
		{
			int timeLeft = length;

			while (timeLeft > 0) {
				_confirmFbtButton.Text = "Confirm?\n" + timeLeft;
				timeLeft--;
				yield return new WaitForSeconds(1);
			}

			_confirmFbtButton.gameObject.SetActive(false);
			_calibrateFbtButton.SetActive(true);
		}
	}
}