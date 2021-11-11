using UnityEngine;

namespace AdBlocker
{
    internal static class Helpers
    {
        public static GameObject FindInactive(string path)
        {
            // very bad and inefficient do not use
            // or do i don't really care
            string[] hierarchy = path.Split('/');
            GameObject currentObject = GameObject.Find(hierarchy[0]);

            for (int i = 0; i < hierarchy.Length - 1; i++) currentObject = currentObject.transform.Find(hierarchy[i + 1]).gameObject;

            return currentObject;
        }
    }
}