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
        private static MelonPreferences_Category cat;
        private static MelonPreferences_Entry<bool> carousselEntry;
        private static MelonPreferences_Entry<bool> vrcPlusBannerEntry;
        private static MelonPreferences_Entry<bool> vrcPlusSupporterEntry;
        private static MelonPreferences_Entry<bool> vrcPlusGiftEntry;
        private static MelonPreferences_Entry<bool> vrcPlusTabEntry;
        private static MelonPreferences_Entry<bool> vrcPlusPFPEntry;

        public override void OnApplicationStart()
        {
            //If using emmVRC leave vrcPlusSupporter and vrcPlusTab off because it will throw errors since they use EnableDisable Listeners when opening the menu and we just make the objects go poof
            cat = MelonPreferences.CreateCategory("AdBlocker", "AdBlocker Settings");
            carousselEntry = cat.CreateEntry("carousel", true, "Remove QM Caroussel");
            vrcPlusBannerEntry = cat.CreateEntry("vrcPlusBanner", true, "Remove VRC+ Banner");
            vrcPlusGiftEntry = cat.CreateEntry("vrcPlusGift", true, "Remove VRC+ Gift Buttons");
            vrcPlusPFPEntry = cat.CreateEntry("vrcPlusPFPButton", true, "Remove VRC+ PFP Button");
            vrcPlusSupporterEntry = cat.CreateEntry("vrcPlusSupporter", false, "Remove VRC+ Supporter Button");
            vrcPlusTabEntry = cat.CreateEntry("vrcPlusTab", false, "Remove VRC+ Tab");

            carousselEntry.OnValueChangedUntyped += OnPreferencesChanged;
            vrcPlusBannerEntry.OnValueChangedUntyped += OnPreferencesChanged;
            vrcPlusSupporterEntry.OnValueChangedUntyped += OnPreferencesChanged;
            vrcPlusGiftEntry.OnValueChangedUntyped += OnPreferencesChanged;
            vrcPlusTabEntry.OnValueChangedUntyped += OnPreferencesChanged;
            vrcPlusPFPEntry.OnValueChangedUntyped += OnPreferencesChanged;

            VRChatUtilityKit.Utilities.VRCUtils.OnUiManagerInit += Init;
        }

        private static void Init()
        {
            if (carousselEntry.Value)
            {
                try
                {
                    GameObject.DestroyImmediate(Helpers.FindInactive("UserInterface/Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_Dashboard/ScrollRect/Viewport/VerticalLayoutGroup/Carousel_Banners"));
                    MelonLogger.Msg("Removed Carousel");
                }
                catch(Exception e)
                {
                    MelonLogger.Error("Failed to remove Carousel");
                    MelonLogger.Error(e);
                }
            }

            if (vrcPlusBannerEntry.Value)
            {
                try
                {
                    GameObject vrcPlusBanner = Helpers.FindInactive("UserInterface/Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_Dashboard/ScrollRect/Viewport/VerticalLayoutGroup/VRC+_Banners");
                    GameObject.DestroyImmediate(vrcPlusBanner);
                    MelonLogger.Msg("Removed VRC+ Banner");
                }
                catch(Exception e)
                {
                    MelonLogger.Error("Failed to remove VRC+ Banner");
                    MelonLogger.Error(e);
                }
            }

            if (vrcPlusSupporterEntry.Value)
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

            if (vrcPlusGiftEntry.Value)
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

            if (vrcPlusTabEntry.Value)
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

            if (vrcPlusPFPEntry.Value)
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

        private void OnPreferencesChanged()
        {
            MelonLogger.Msg("Preferences changed, please restart your game for changes to take effect");
        }
    }
}