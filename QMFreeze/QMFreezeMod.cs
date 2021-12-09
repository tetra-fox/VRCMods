using MelonLoader;
using UnityEngine;
using VRC.SDKBase;

[assembly: MelonInfo(typeof(QMFreeze.Mod), QMFreeze.BuildInfo.Name, QMFreeze.BuildInfo.Version, QMFreeze.BuildInfo.Author, QMFreeze.BuildInfo.DownloadLink)]
[assembly: MelonGame("VRChat", "VRChat")]

namespace QMFreeze
{
	internal static class BuildInfo
	{
		public const string Name = "QMFreeze";
		public const string Author = "tetra";
		public const string Version = "3.0.1";
		public const string DownloadLink = "https://github.com/tetra-fox/VRCMods";
	}

	public class Mod : MelonMod
	{
		private static bool _freezeAllowed;
		private static bool _frozen;
		private static Vector3 _originalGravity;
		private static Vector3 _originalVelocity;

		public override void OnApplicationStart()
		{
			this.LoggerInstance.Msg("Registering settings...");
			Settings.Register();
			Settings.OnConfigChanged += OnDisableCheck;

			VRChatUtilityKit.Utilities.VRCUtils.OnUiManagerInit += this.Init;
		}

		private void Init()
		{
			this.LoggerInstance.Msg("Patching QM open/close...");
			VRChatUtilityKit.Ui.UiManager.OnQuickMenuOpened += Freeze;
			VRChatUtilityKit.Ui.UiManager.OnQuickMenuClosed += Unfreeze;

			this.LoggerInstance.Msg("Setting up risky functions check...");
			VRChatUtilityKit.Utilities.VRCUtils.OnEmmWorldCheckCompleted += allowed => { _freezeAllowed = allowed; };
			VRChatUtilityKit.Utilities.NetworkEvents.OnRoomLeft += LeaveRoomPatch;

			this.LoggerInstance.Msg("Initialized!");
		}

		private static void Freeze()
		{
			if (!_freezeAllowed || !Settings.Enabled.Value) return;
			_originalGravity = Physics.gravity;
			_originalVelocity = Networking.LocalPlayer.GetVelocity();

			// Don't need to freeze if you're not moving
			if (_originalVelocity == Vector3.zero) return;

			Physics.gravity = Vector3.zero;
			Networking.LocalPlayer.SetVelocity(Vector3.zero);
			_frozen = true;
		}

		private static void Unfreeze()
		{
			if (!_freezeAllowed || !_frozen) return;
			Physics.gravity = _originalGravity;
			// If you're trying to respawn after being launched at a super high velocity, you might want this off so that you don't keep flying after respawning
			if (Settings.RestoreVelocity.Value) Networking.LocalPlayer.SetVelocity(_originalVelocity);
			_frozen = false;
		}

		private static void OnDisableCheck()
		{
			if (_frozen && !Settings.Enabled.Value) Unfreeze();
		}

		private static void LeaveRoomPatch()
		{
			Unfreeze();
			_freezeAllowed = false;
		}
	}
}