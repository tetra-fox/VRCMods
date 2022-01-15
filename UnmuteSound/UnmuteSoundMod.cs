using System;
using MelonLoader;
using System.Linq;
using UnityEngine;

[assembly: MelonInfo(typeof(UnmuteSound.Mod), UnmuteSound.BuildInfo.Name, UnmuteSound.BuildInfo.Version, UnmuteSound.BuildInfo.Author, UnmuteSound.BuildInfo.DownloadLink)]
[assembly: MelonGame("VRChat", "VRChat")]

namespace UnmuteSound
{
	internal static class BuildInfo
	{
		public const string Name = "UnmuteSound";
		public const string Author = "tetra";
		public const string Version = "2.0.1";
		public const string DownloadLink = "https://github.com/tetra-fox/VRCMods";
	}

	public class Mod : MelonMod
	{
		private static readonly MelonLogger.Instance Logger = new(BuildInfo.Name);
		
		public override void OnApplicationStart() => VRChatUtilityKit.Utilities.VRCUtils.OnUiManagerInit += Init;

		private static void Init()
		{
			Logger.Msg("Creating audio source...");

			HudVoiceIndicator voiceIndicator = GameObject.Find("UserInterface/UnscaledUI/HudContent").GetComponent<HudVoiceIndicator>();

			AudioSource unmuteBlop = voiceIndicator.gameObject.AddComponent<AudioSource>();
			unmuteBlop.clip = voiceIndicator.field_Public_AudioClip_0;
			unmuteBlop.playOnAwake = false;
			unmuteBlop.pitch = 1.2f;

			// thanks knah https://github.com/knah/VRCMods/blob/142dab764543a17ab10092ec684bf7cf19e72683/JoinNotifier/JoinNotifierMod.cs#L64-L70
			VRCAudioManager audioManager = VRCAudioManager.field_Private_Static_VRCAudioManager_0;
			unmuteBlop.outputAudioMixerGroup = new[]
			{
				audioManager.field_Public_AudioMixerGroup_0,
				audioManager.field_Public_AudioMixerGroup_1,
				audioManager.field_Public_AudioMixerGroup_2
			}.Single(mg => mg.name == "UI");

			Logger.Msg("Patching methods...");

			bool joiningRoom = true;

			DefaultTalkController.field_Private_Static_Action_0 += (Action) (() =>
			{
				if (DefaultTalkController.field_Private_Static_Boolean_0 || joiningRoom) return;
				unmuteBlop.Play();
			});

			VRChatUtilityKit.Utilities.NetworkEvents.OnRoomJoined += () => joiningRoom = false;
			VRChatUtilityKit.Utilities.NetworkEvents.OnRoomLeft += () => joiningRoom = true;

			Logger.Msg("Initialized!");
		}
	}
}