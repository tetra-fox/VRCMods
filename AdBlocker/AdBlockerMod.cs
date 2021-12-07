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

        private static void TryRemove()
        {
            if (Settings.RemoveCarousel.Value)
            {
                try
                {
                    GameObject.DestroyImmediate(Helpers.FindInactive("UserInterface/Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_Dashboard/ScrollRect/Viewport/VerticalLayoutGroup/Carousel_Banners"));
                    MelonLogger.Msg("Removed Carousel");
                }
                catch (Exception e)
                {
                    MelonLogger.Error("Failed to remove Carousel");
                    MelonLogger.Error(e);
                }
            }

            if (Settings.RemoveVrcPlusBanner.Value)
            {
                try
                {
                    GameObject vrcPlusBanner = Helpers.FindInactive("UserInterface/Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_Dashboard/ScrollRect/Viewport/VerticalLayoutGroup/VRC+_Banners");
                    GameObject.DestroyImmediate(vrcPlusBanner);
                    MelonLogger.Msg("Removed VRC+ Banner");
                }
                catch (Exception e)
                {
                    MelonLogger.Error("Failed to remove VRC+ Banner");
                    MelonLogger.Error(e);
                }
            }

            if (Settings.RemoveVrcPlusSupporter.Value)
            {
                try
                {
                    GameObject.DestroyImmediate(Helpers.FindInactive("UserInterface/MenuContent/Screens/UserInfo/Buttons/RightSideButtons/RightUpperButtonColumn/Supporter"));
                    MelonLogger.Msg("Removed VRC+ Supporter Button");
                }
                catch (Exception e)
                {
                    MelonLogger.Error("Failed to remove VRC+ Supporter Button");
                    MelonLogger.Error(e);
                }
            }

            if (Settings.RemoveVrcPlusGift.Value)
            {
                try
                {
                    GameObject.DestroyImmediate(Helpers.FindInactive("UserInterface/MenuContent/Screens/UserInfo/Buttons/RightSideButtons/RightUpperButtonColumn/GiftVRChatPlusButton"));
                    MelonLogger.Msg("Removed VRC+ Gift Button");
                    GameObject.DestroyImmediate(Helpers.FindInactive("UserInterface/Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_SelectedUser_Remote/ScrollRect/Viewport/VerticalLayoutGroup/Buttons_UserActions/Button_GiftVRChatPlus"));
                    MelonLogger.Msg("Removed VRC+ QM Gift Button Remote");
                    GameObject.DestroyImmediate(Helpers.FindInactive("UserInterface/Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_SelectedUser_Local/ScrollRect/Viewport/VerticalLayoutGroup/Buttons_UserActions/Button_GiftVRCPlus"));
                    MelonLogger.Msg("Removed VRC+ QM Gift Button Local");

                }
                catch (Exception e)
                {
                    MelonLogger.Error("Failed to remove VRC+ Gift Buttons");
                    MelonLogger.Error(e);
                }
            }

            if (Settings.RemoveVrcPlusTab.Value)
            {
                try
                {
                    GameObject.DestroyImmediate(Helpers.FindInactive("UserInterface/MenuContent/Backdrop/Header/Tabs/ViewPort/Content/VRC+PageTab"));
                    MelonLogger.Msg("Removed VRC+ Tab");
                }
                catch (Exception e)
                {
                    MelonLogger.Error("Failed to remove VRC+ Tab");
                    MelonLogger.Error(e);
                }
            }

            if (Settings.RemoveVrcPlusPfp.Value)
            {
                try
                {
                    GameObject.DestroyImmediate(Helpers.FindInactive("UserInterface/MenuContent/Screens/UserInfo/SelfButtons/ChangeProfilePicButton"));
                    MelonLogger.Msg("Removed VRC+ PFP Button");
                }
                catch (Exception e)
                {
                    MelonLogger.Error("Failed to remove VRC+ PFP Button");
                    MelonLogger.Error(e);
                }
            }
        }
    }
}