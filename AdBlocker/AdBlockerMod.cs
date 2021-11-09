using System;
using MelonLoader;
using UnityEngine;
using UnityEngine.Input;

namespace AdBlocker
{
    public static class BuildInfo
    {
        public const string Name = "AdBlocker";
        public const string Author = "tetra";
        public const string Version = "1.0.0";
        public const string DownloadLink = "https://github.com/tetra-fox/VRCMods";
    }
    
    public class Mod : MelonMod
    {
        public override void OnApplicationStart()
        {
            //VRChatUtilityKit.Ui.UiManager.OnQuickMenuOpened += RemoveBanners;
        }

        public override void OnUpdate()
        {
        }

        private static void RemoveBanners()
        {
            GameObject carousel = GameObject.Find("UserInterface/Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_Dashboard/ScrollRect/Viewport/VerticalLayoutGroup/Carousel_Banners");
            //GameObject vrcplus = GameObject.Find("UserInterface/Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_Dashboard/ScrollRect/Viewport/VerticalLayoutGroup/VRC+_Banners");
            GameObject.DestroyImmediate(carousel);
            //GameObject.DestroyImmediate(vrcplus);
            MelonLogger.Msg(carousel.name);
            MelonLogger.Msg("naw");
            
            VRChatUtilityKit.Ui.UiManager.OnQuickMenuOpened -= RemoveBanners;
        }
    }
}