using HarmonyLib;
using MelonLoader;
using QMFreeze.Components;
using UnhollowerRuntimeLib;
using UnityEngine;
using VRC.SDKBase;

namespace QMFreeze
{
    public class BuildInfo
    {
        public const string Name = "QMFreeze";
        public const string Author = "tetra";
        public const string Version = "1.0.5";
        public const string DownloadLink = "https://github.com/tetra-fox/QMFreeze/releases/download/1.0.4/QMFreeze.dll";
    }

    public class Mod : MelonMod
    {
        public static bool FreezeAllowed;
        public static bool Frozen;
        private static Vector3 _originalGravity;
        private static Vector3 _originalVelocity;

        public override void OnApplicationStart()
        {
            MelonLogger.Msg("Registering components...");
            ClassInjector.RegisterTypeInIl2Cpp<EnableDisableListener>();

            MelonLogger.Msg("Registering settings...");
            Settings.Register();
            Settings.Apply();

            if (Utils.HasMod("UI Expansion Kit"))
            {
                MelonLogger.Msg("UIX found. Using UIX hook.");
                UIExpansionKit.API.ExpansionKitApi.OnUiManagerInit += Init;
                return;
            }
            MelonLogger.Msg("UIX not found. Using fallback hook.");
            MelonCoroutines.Start(Hooks.UiManager(Init));
        }

        private static void Init()
        {
            MelonLogger.Msg("Adding QM listener...");
            // MicControls is enabled no matter the QM page that's open, so let's use that to determine whether or not the QM is open
            // Unless you have some other mod that removes this button then idk lol
            EnableDisableListener listener = GameObject.Find("/UserInterface/QuickMenu/MicControls").AddComponent<EnableDisableListener>();
            listener.OnEnabled += Freeze;
            listener.OnDisabled += Unfreeze;

            MelonLogger.Msg("Initialized!");
        }

        [HarmonyPatch(typeof(NetworkManager), "OnJoinedRoom")]
        private class OnJoinedRoomPatch
        {
            // This can definitely be exploited, so a world check is needed.
            private static void Prefix() => Utils.CheckWorld();
        }

        [HarmonyPatch(typeof(NetworkManager), "OnLeftRoom")]
        private class OnLeftRoomPatch
        {
            private static void Prefix()
            {
                Frozen = false;
                FreezeAllowed = false;
            }
        }

        public static void Freeze()
        {
            if (!FreezeAllowed || !Settings.Enabled) return;
            _originalGravity = Physics.gravity;
            _originalVelocity = Networking.LocalPlayer.GetVelocity();

            // Don't need to freeze if you're not moving
            if (_originalVelocity == Vector3.zero) return;

            Physics.gravity = Vector3.zero;
            Networking.LocalPlayer.SetVelocity(Vector3.zero);
            Frozen = true;
            // MelonLogger.Msg("Frozen");
        }

        public static void Unfreeze()
        {
            if (!FreezeAllowed || !Frozen || !Settings.Enabled) return;
            Physics.gravity = _originalGravity;
            // If you're trying to respawn after being launched at a super high velocity, you might want this off so that you don't keep flying after respawning
            if (Settings.RestoreVelocity) Networking.LocalPlayer.SetVelocity(_originalVelocity);
            Frozen = false;
            // MelonLogger.Msg("Unfrozen");
        }

        public override void OnPreferencesLoaded() => Settings.Apply();

        public override void OnPreferencesSaved() => Settings.Apply();
    }
}