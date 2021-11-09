using UnityEngine;

namespace AdBlocker
{
    internal static class Helpers
    {
        // very bad and inefficient do not use
        // or do i don't really care
        internal static GameObject FindInactive(string path)
        {
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
    }
}