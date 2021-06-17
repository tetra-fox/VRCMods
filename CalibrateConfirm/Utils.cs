using MelonLoader;
using System.Linq;
using UnityEngine;

namespace CalibrateConfirm
{
    internal static class Utils
    {
        internal static bool HasMod(string modName)
        {
            return MelonHandler.Mods.Any(m => m.Info.Name.Equals(modName));
        }

        // stolen from loukylor cuz i can't do math

        public static Vector3 ConvertToUnityUnits(Vector3 menuPosition)
        {
            menuPosition.y *= -1;
            return new Vector3(-1050, 1470) + menuPosition * 420;
        }

        public static Vector3 ConvertToMenuUnits(Vector3 unityPosition)
        {
            Vector3 menuUnits = (unityPosition - new Vector3(-1050, 1470)) / 420;
            menuUnits.y *= -1;
            return menuUnits;
        }
    }
}