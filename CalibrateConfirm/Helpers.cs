using System.Reflection;
using UnityEngine;

namespace CalibrateConfirm
{
    internal static class Helpers
    {
        public static GameObject FindInactive(string path)
        {
            // very bad and inefficient do not use
            // or do i don't really care
            string[] hierarchy = path.Split('/');
            GameObject currentObject = GameObject.Find(hierarchy[0]);
            string targetObjectName = hierarchy[hierarchy.Length - 1];
            int cursor = 0;
            
            // also check if cursor is at end of path (in case gameobject names are reused in the hierarchy)
            while (currentObject.name != targetObjectName && cursor != hierarchy.Length)
            {
                GameObject nextObject = currentObject.transform.Find(hierarchy[cursor + 1]).gameObject;
                currentObject = nextObject;
                cursor++;
                // if nextObject is null, unity will just throw a NullReferenceException, stopping this loop
            }
            
            return currentObject;
        }
        
        public static MethodInfo GetCalibrateMethod()
        {
            // stolen from knah
            // https://github.com/knah/VRCMods/blob/abda67864be620b20ad73c1ce8b2c93778dc09a9/IKTweaks/IKTweaksMod.cs#L245-L250
            foreach (MethodInfo methodInfo in typeof(VRCTrackingManager).GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly))
            {
                if (!methodInfo.Name.StartsWith("Method_Public_Virtual_Final_New_Void_") || methodInfo.GetParameters().Length != 0) continue;

                return methodInfo;
            }

            return null;
        }
    }
}