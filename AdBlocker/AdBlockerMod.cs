using MelonLoader;
using UnityEngine;

namespace AdBlocker
{
    internal static class BuildInfo
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
            VRChatUtilityKit.Utilities.VRCUtils.OnUiManagerInit += Init;
        }

        private static void Init()
        {
            GameObject carousel = Helpers.FindInactive("UserInterface/Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_Dashboard/ScrollRect/Viewport/VerticalLayoutGroup/Carousel_Banners");
            GameObject vrcPlus = Helpers.FindInactive("UserInterface/Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_Dashboard/ScrollRect/Viewport/VerticalLayoutGroup/VRC+_Banners");
            
            GameObject.DestroyImmediate(carousel);
            MelonLogger.Msg("Removed Carousel");
            GameObject.DestroyImmediate(vrcPlus);
            MelonLogger.Msg("Removed VRC+");
        }
    }
}