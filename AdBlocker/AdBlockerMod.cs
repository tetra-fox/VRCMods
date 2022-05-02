using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using Object = UnityEngine.Object;

[assembly: MelonInfo(typeof(AdBlocker.Mod), AdBlocker.BuildInfo.Name, AdBlocker.BuildInfo.Version, AdBlocker.BuildInfo.Author, AdBlocker.BuildInfo.DownloadLink)]
[assembly: MelonGame("VRChat", "VRChat")]
[assembly: MelonAdditionalDependencies("VRChatUtilityKit")]
[assembly: MelonOptionalDependencies("UI Expansion Kit")]

namespace AdBlocker;

internal static class BuildInfo
{
    public const string Name = "AdBlocker";
    public const string Author = "tetra, Xavi";
    public const string Version = "1.0.4";
    public const string DownloadLink = "https://github.com/tetra-fox/VRCMods";
}

public class Mod : MelonMod
{
    private static readonly MelonLogger.Instance Logger = new(BuildInfo.Name);

    public override void OnApplicationStart()
    {
        Settings.Register();
        
        // Settings values and their corresponding GameObject paths
        Dictionary<MelonPreferences_Entry<bool>, string[]> ads = new()
        {
            {Settings.RemoveCarousel,
                new[] {"UserInterface/Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_Dashboard/ScrollRect/Viewport/VerticalLayoutGroup/Carousel_Banners"}},
            {Settings.RemoveVrcPlusBanner,
                new[] {"UserInterface/Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_Dashboard/ScrollRect/Viewport/VerticalLayoutGroup/VRC+_Banners"}},
            {Settings.RemoveVrcPlusSupporter,
                new[] {"UserInterface/MenuContent/Screens/UserInfo/Buttons/RightSideButtons/RightUpperButtonColumn/Supporter"}},
            {Settings.RemoveVrcPlusGift,
                new[] {"UserInterface/MenuContent/Screens/UserInfo/Buttons/RightSideButtons/RightUpperButtonColumn/GiftVRChatPlusButton",
                    "UserInterface/Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_SelectedUser_Remote/ScrollRect/Viewport/VerticalLayoutGroup/Buttons_UserActions/Button_GiftVRChatPlus",
                    "UserInterface/Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_SelectedUser_Local/ScrollRect/Viewport/VerticalLayoutGroup/Buttons_UserActions/Button_GiftVRCPlus"}},
            {Settings.RemoveVrcPlusTab,
                new[] {"UserInterface/MenuContent/Backdrop/Header/Tabs/ViewPort/Content/VRC+PageTab"}},
            {Settings.RemoveVrcPlusPfp,
                new[] {"UserInterface/MenuContent/Screens/UserInfo/SelfButtons/ChangeProfilePicButton"}},
            {Settings.RemoveVrcPlusGetMoreFavorites,
                new[] {"UserInterface/MenuContent/Screens/Avatar/Vertical Scroll View/Viewport/FavoriteListTemplate/GetMoreFavorites/MoreFavoritesButton",
                    "UserInterface/MenuContent/Screens/Avatar/Vertical Scroll View/Viewport/FavoriteListTemplate/GetMoreFavorites/MoreFavoritesText"}}
        };

        VRChatUtilityKit.Utilities.VRCUtils.OnUiManagerInit += () =>
        {
            foreach (KeyValuePair<MelonPreferences_Entry<bool>, string[]> ad in ads.Where(element => element.Key.Value))
            {
                foreach (string path in ad.Value)
                {
                    try
                    {
                        Object.DestroyImmediate(Helpers.FindInactive(path));
                    }
                    catch (Exception e)
                    {
                        Logger.Error($"Failed to {char.ToLower(ad.Key.DisplayName[0]) + ad.Key.DisplayName.Substring(1)}");
                        Logger.Error(e);
                    }
                }
            }
        };
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
}