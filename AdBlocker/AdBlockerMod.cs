using MelonLoader;
using System;
using UnityEngine;

[assembly: MelonInfo(typeof(AdBlocker.Mod), AdBlocker.BuildInfo.Name, AdBlocker.BuildInfo.Version, AdBlocker.BuildInfo.Author, AdBlocker.BuildInfo.DownloadLink)]
[assembly: MelonGame("VRChat", "VRChat")]

namespace AdBlocker
{
	internal static class BuildInfo
	{
		public const string Name = "AdBlocker";
		public const string Author = "tetra, Xavi";
		public const string Version = "1.0.2";
		public const string DownloadLink = "https://github.com/tetra-fox/VRCMods";
	}

	public class Mod : MelonMod
	{
		public override void OnApplicationStart()
		{
			Settings.Register();
			VRChatUtilityKit.Utilities.VRCUtils.OnUiManagerInit += TryRemove;
		}

		public override void OnPreferencesSaved()
		{
			// only if the user changed AdBlocker prefs
			if (!Settings.Changed) return;

			const string msg = "Preferences changed, please restart your game for changes to take effect";
			this.LoggerInstance.Warning(msg);
			Helpers.DisplayHudMessage($"[AdBlocker]\n{msg}");

			Settings.Changed = false;
		}

		private void TryRemove()
		{
			if (Settings.RemoveCarousel.Value) {
				try {
					GameObject.DestroyImmediate(Helpers.FindInactive("UserInterface/Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_Dashboard/ScrollRect/Viewport/VerticalLayoutGroup/Carousel_Banners"));
					this.LoggerInstance.Msg("Removed Carousel");
				}
				catch (Exception e) {
					this.LoggerInstance.Error("Failed to remove Carousel");
					this.LoggerInstance.Error(e);
				}
			}

			if (Settings.RemoveVrcPlusBanner.Value) {
				try {
					GameObject vrcPlusBanner = Helpers.FindInactive("UserInterface/Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_Dashboard/ScrollRect/Viewport/VerticalLayoutGroup/VRC+_Banners");
					GameObject.DestroyImmediate(vrcPlusBanner);
					this.LoggerInstance.Msg("Removed VRC+ Banner");
				}
				catch (Exception e) {
					this.LoggerInstance.Error("Failed to remove VRC+ Banner");
					this.LoggerInstance.Error(e);
				}
			}

			if (Settings.RemoveVrcPlusSupporter.Value) {
				try {
					GameObject.DestroyImmediate(Helpers.FindInactive("UserInterface/MenuContent/Screens/UserInfo/Buttons/RightSideButtons/RightUpperButtonColumn/Supporter"));
					this.LoggerInstance.Msg("Removed VRC+ Supporter Button");
				}
				catch (Exception e) {
					this.LoggerInstance.Error("Failed to remove VRC+ Supporter Button");
					this.LoggerInstance.Error(e);
				}
			}

			if (Settings.RemoveVrcPlusGift.Value) {
				try {
					GameObject.DestroyImmediate(Helpers.FindInactive("UserInterface/MenuContent/Screens/UserInfo/Buttons/RightSideButtons/RightUpperButtonColumn/GiftVRChatPlusButton"));
					this.LoggerInstance.Msg("Removed VRC+ Gift Button");
					GameObject.DestroyImmediate(Helpers.FindInactive("UserInterface/Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_SelectedUser_Remote/ScrollRect/Viewport/VerticalLayoutGroup/Buttons_UserActions/Button_GiftVRChatPlus"));
					this.LoggerInstance.Msg("Removed VRC+ QM Gift Button Remote");
					GameObject.DestroyImmediate(Helpers.FindInactive("UserInterface/Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_SelectedUser_Local/ScrollRect/Viewport/VerticalLayoutGroup/Buttons_UserActions/Button_GiftVRCPlus"));
					this.LoggerInstance.Msg("Removed VRC+ QM Gift Button Local");
				}
				catch (Exception e) {
					this.LoggerInstance.Error("Failed to remove VRC+ Gift Buttons");
					this.LoggerInstance.Error(e);
				}
			}

			if (Settings.RemoveVrcPlusTab.Value) {
				try {
					GameObject.DestroyImmediate(Helpers.FindInactive("UserInterface/MenuContent/Backdrop/Header/Tabs/ViewPort/Content/VRC+PageTab"));
					this.LoggerInstance.Msg("Removed VRC+ Tab");
				}
				catch (Exception e) {
					this.LoggerInstance.Error("Failed to remove VRC+ Tab");
					this.LoggerInstance.Error(e);
				}
			}

			if (Settings.RemoveVrcPlusPfp.Value) {
				try {
					GameObject.DestroyImmediate(Helpers.FindInactive("UserInterface/MenuContent/Screens/UserInfo/SelfButtons/ChangeProfilePicButton"));
					this.LoggerInstance.Msg("Removed VRC+ PFP Button");
				}
				catch (Exception e) {
					this.LoggerInstance.Error("Failed to remove VRC+ PFP Button");
					this.LoggerInstance.Error(e);
				}
			}
		}
	}
}