using System.Linq;
using System.Reflection;
using UnityEngine;

namespace CalibrateConfirm {
	internal static class Helpers {
		public static GameObject FindInactive(string path) {
			// very bad and inefficient do not use
			// or do i don't really care
			string[] hierarchy = path.Split('/');
			GameObject currentObject = GameObject.Find(hierarchy[0]);

			for (int i = 0; i < hierarchy.Length - 1; i++) currentObject = currentObject.transform.Find(hierarchy[i + 1]).gameObject;

			return currentObject;
		}

		public static MethodInfo GetCalibrateMethod() {
			// stolen from knah then linq-ified
			// https://github.com/knah/VRCMods/blob/abda67864be620b20ad73c1ce8b2c93778dc09a9/IKTweaks/IKTweaksMod.cs#L245-L250
			return typeof(VRCTrackingManager).GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly).FirstOrDefault(methodInfo => methodInfo.Name.StartsWith("Method_Public_Virtual_Final_New_Void_") && methodInfo.GetParameters().Length == 0);
		}
	}
}