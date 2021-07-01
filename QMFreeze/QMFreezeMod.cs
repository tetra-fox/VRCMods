using HarmonyLib;
using MelonLoader;
using System.Linq;
using UnityEngine;
using VRC.SDKBase;

namespace QMFreeze
{
    public class BuildInfo
    {
        public const string Name = "QMFreeze";
        public const string Author = "tetra";
        public const string Version = "2.0.0";
        public const string DownloadLink = "https://github.com/tetra-fox/VRCMods";
    }

    public class QMFreezeMod : MelonMod
    {
        private static bool _freezeAllowed;
        public static bool Frozen;
        private static Vector3 _originalGravity;
        private static Vector3 _originalVelocity;

        public override void OnApplicationStart()
        {
            if (!MelonHandler.Mods.Any(m => m.Info.Name.Equals("VRChatUtilityKit")))
            {
                MelonLogger.Error("This mod requires VRChatUtilityKit to run! Download it from loukylor's GitHub:");
                MelonLogger.Error("https://github.com/loukylor/VRC-Mods");
                return;
            }

            MelonLogger.Msg("Registering settings...");
            QMFreezeSettings.Register();
            QMFreezeSettings.Apply();

            VRChatUtilityKit.Utilities.VRCUtils.OnUiManagerInit += Init;
        }

        private static void Init()
        {
            MelonLogger.Msg("Patching QM open/close...");
            VRChatUtilityKit.Ui.UiManager.OnQuickMenuOpened += Freeze;
            VRChatUtilityKit.Ui.UiManager.OnQuickMenuClosed += Unfreeze;

            MelonLogger.Msg("Setting up emm check...");
            VRChatUtilityKit.Utilities.VRCUtils.OnEmmWorldCheckCompleted += allowed => { _freezeAllowed = allowed; };

            MelonLogger.Msg("Initialized!");
        }

        [HarmonyPatch(typeof(NetworkManager), "OnLeftRoom")]
        private class OnLeftRoomPatch
        {
            private static void Prefix()
            {
                Unfreeze();
                _freezeAllowed = false;
            }
        }

        private static void Freeze()
        {
            if (!_freezeAllowed || !QMFreezeSettings.Enabled) return;
            _originalGravity = Physics.gravity;
            _originalVelocity = Networking.LocalPlayer.GetVelocity();

            // Don't need to freeze if you're not moving
            if (_originalVelocity == Vector3.zero) return;

            Physics.gravity = Vector3.zero;
            Networking.LocalPlayer.SetVelocity(Vector3.zero);
            Frozen = true;
        }

        public static void Unfreeze()
        {
            if (!_freezeAllowed || !Frozen || !QMFreezeSettings.Enabled) return;
            Physics.gravity = _originalGravity;
            // If you're trying to respawn after being launched at a super high velocity, you might want this off so that you don't keep flying after respawning
            if (QMFreezeSettings.RestoreVelocity) Networking.LocalPlayer.SetVelocity(_originalVelocity);
            Frozen = false;
        }

        public override void OnPreferencesLoaded() => QMFreezeSettings.Apply();

        public override void OnPreferencesSaved() => QMFreezeSettings.Apply();
    }
}