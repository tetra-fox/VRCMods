using MelonLoader;
using System;
using UnityEngine;

[assembly: MelonInfo(typeof(AdBlocker.Mod), AdBlocker.BuildInfo.Name, AdBlocker.BuildInfo.Version, AdBlocker.BuildInfo.Author, AdBlocker.BuildInfo.DownloadLink)]
[assembly: MelonGame("VRChat", "VRChat")]
[assembly: MelonAdditionalDependencies("VRChatUtilityKit")]
[assembly: MelonOptionalDependencies("UI Expansion Kit")]

namespace AdBlocker;

internal static class BuildInfo
{
    public const string Name = "AdBlocker";
    public const string Author = "tetra, Xavi";
    public const string Version = "1.0.3";
    public const string DownloadLink = "https://github.com/tetra-fox/VRCMods";
}

public class Mod : MelonMod
{
    private static readonly MelonLogger.Instance Logger = new(BuildInfo.Name);

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
        Logger.Warning(msg);
        Helpers.DisplayHudMessage($"[AdBlocker]\n{msg}");

        Settings.Changed = false;
    }

    private static void TryRemove()
    {
        if (Settings.RemoveCarousel.Value)
        {
            try
            {
                GameObject.DestroyImmediate(Helpers.FindInactive("UserInterface/Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_Dashboard/ScrollRect/Viewport/VerticalLayoutGroup/Carousel_Banners"));
                Logger.Msg("Removed Carousel");
            }
            catch (Exception e)
            {
                Logger.Error("Failed to remove Carousel");
                Logger.Error(e);
            }
        }

        if (Settings.RemoveVrcPlusBanner.Value)
        {
            try
            {
                GameObject vrcPlusBanner = Helpers.FindInactive("UserInterface/Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_Dashboard/ScrollRect/Viewport/VerticalLayoutGroup/VRC+_Banners");
                GameObject.DestroyImmediate(vrcPlusBanner);
                Logger.Msg("Removed VRC+ Banner");
            }
            catch (Exception e)
            {
                Logger.Error("Failed to remove VRC+ Banner");
                Logger.Error(e);
            }
        }

        if (Settings.RemoveVrcPlusSupporter.Value)
        {
            try
            {
                GameObject.DestroyImmediate(Helpers.FindInactive("UserInterface/MenuContent/Screens/UserInfo/Buttons/RightSideButtons/RightUpperButtonColumn/Supporter"));
                Logger.Msg("Removed VRC+ Supporter Button");
            }
            catch (Exception e)
            {
                Logger.Error("Failed to remove VRC+ Supporter Button");
                Logger.Error(e);
            }
        }

        if (Settings.RemoveVrcPlusGift.Value)
        {
            try
            {
                GameObject.DestroyImmediate(Helpers.FindInactive("UserInterface/MenuContent/Screens/UserInfo/Buttons/RightSideButtons/RightUpperButtonColumn/GiftVRChatPlusButton"));
                Logger.Msg("Removed VRC+ Gift Button");
                GameObject.DestroyImmediate(Helpers.FindInactive("UserInterface/Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_SelectedUser_Remote/ScrollRect/Viewport/VerticalLayoutGroup/Buttons_UserActions/Button_GiftVRChatPlus"));
                Logger.Msg("Removed VRC+ QM Gift Button Remote");
                GameObject.DestroyImmediate(Helpers.FindInactive("UserInterface/Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_SelectedUser_Local/ScrollRect/Viewport/VerticalLayoutGroup/Buttons_UserActions/Button_GiftVRCPlus"));
                Logger.Msg("Removed VRC+ QM Gift Button Local");
            }
            catch (Exception e)
            {
                Logger.Error("Failed to remove VRC+ Gift Buttons");
                Logger.Error(e);
            }
        }

        if (Settings.RemoveVrcPlusTab.Value)
        {
            try
            {
                GameObject.DestroyImmediate(Helpers.FindInactive("UserInterface/MenuContent/Backdrop/Header/Tabs/ViewPort/Content/VRC+PageTab"));
                Logger.Msg("Removed VRC+ Tab");
            }
            catch (Exception e)
            {
                Logger.Error("Failed to remove VRC+ Tab");
                Logger.Error(e);
            }
        }

        if (Settings.RemoveVrcPlusPfp.Value)
        {
            try
            {
                GameObject.DestroyImmediate(Helpers.FindInactive("UserInterface/MenuContent/Screens/UserInfo/SelfButtons/ChangeProfilePicButton"));
                Logger.Msg("Removed VRC+ PFP Button");
            }
            catch (Exception e)
            {
                Logger.Error("Failed to remove VRC+ PFP Button");
                Logger.Error(e);
            }
        }
    }
}