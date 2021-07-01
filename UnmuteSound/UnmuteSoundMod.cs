using HarmonyLib;
using MelonLoader;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace UnmuteSound
{
    public class BuildInfo
    {
        public const string Name = "UnmuteSound";
        public const string Author = "tetra";
        public const string Version = "2.0.0";
    }

    public class UnmuteSoundMod : MelonMod
    {
        private static AudioSource _unmuteBlop;

        private static bool _wasUnmuted;
        private static bool _joiningRoom = true;

        public override void OnApplicationStart()
        {
            if (!MelonHandler.Mods.Any(m => m.Info.Name.Equals("VRChatUtilityKit")))
            {
                MelonLogger.Error("This mod requires VRChatUtilityKit to run! Download it from loukylor's GitHub:");
                MelonLogger.Error("https://github.com/loukylor/VRC-Mods");
                return;
            }
            VRChatUtilityKit.Utilities.VRCUtils.OnUiManagerInit += Init;
        }

        private void Init()
        {
            MelonLogger.Msg("Patching methods...");

            typeof(DefaultTalkController).GetMethods()
                .Where(m => m.Name.StartsWith("Method_Public_Static_Void_Boolean_") && !m.Name.Contains("PDM")).ToList()
                .ForEach(m =>
                {
                    HarmonyInstance.Patch(m,
                        prefix: new HarmonyMethod(typeof(UnmuteSoundMod).GetMethod("ToggleVoicePrefix",
                            BindingFlags.NonPublic | BindingFlags.Static)));
                    MelonLogger.Msg("Patched " + m.Name);
                });

            MelonLogger.Msg("Creating audio source...");

            // this is the actual name of the audio clip lol
            AudioClip Blop = GameObject.Find("UserInterface/UnscaledUI/HudContent/Hud/VoiceDotParent")
                .GetComponent<HudVoiceIndicator>().field_Public_AudioClip_0;

            _unmuteBlop = GameObject.Find("UserInterface/UnscaledUI/HudContent/Hud/VoiceDotParent")
                .AddComponent<AudioSource>();

            _unmuteBlop.clip = Blop;
            _unmuteBlop.playOnAwake = false;
            _unmuteBlop.pitch = 1.2f;

            // thanks knah https://github.com/knah/VRCMods/blob/142dab764543a17ab10092ec684bf7cf19e72683/JoinNotifier/JoinNotifierMod.cs#L64-L70
            VRCAudioManager audioManager = VRCAudioManager.field_Private_Static_VRCAudioManager_0;
            _unmuteBlop.outputAudioMixerGroup = new[]
            {
                audioManager.field_Public_AudioMixerGroup_0,
                audioManager.field_Public_AudioMixerGroup_1,
                audioManager.field_Public_AudioMixerGroup_2
            }.Single(mg => mg.name == "UI");

            MelonLogger.Msg("Initialized!");
        }

        [HarmonyPrefix]
        private static bool ToggleVoicePrefix(ref bool __0)
        {
            // __0 true on mute
            MelonLogger.Msg(__0);
            if (__0) _wasUnmuted = false;
            if (__0 || _wasUnmuted || _joiningRoom) return true;
            _unmuteBlop.Play();
            _wasUnmuted = true;
            return true;
        }

        [HarmonyPatch(typeof(NetworkManager), "OnJoinedRoom")]
        private class OnJoinedRoomPatch
        {
            private static void Prefix() => _joiningRoom = false;
        }

        [HarmonyPatch(typeof(NetworkManager), "OnLeftRoom")]
        private class OnLeftRoomPatch
        {
            private static void Prefix() => _joiningRoom = true;
        }
    }
}