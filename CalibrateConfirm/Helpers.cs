using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnhollowerRuntimeLib.XrefScans;
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

			for (int i = 0; i < hierarchy.Length - 1; i++) currentObject = currentObject.transform.Find(hierarchy[i + 1]).gameObject;

			return currentObject;
		}

		public static MethodInfo GetCalibrateMethod()
		{
			// stolen from knah
			// https://github.com/knah/VRCMods/blob/9ad060a8aa05c1454696f2625ad6a857fec1fed6/IKTweaks/IKTweaksMod.cs#L248-L260
			foreach (MethodInfo methodInfo in typeof(VRCTrackingManager).GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly))
			{
				if (!methodInfo.Name.StartsWith("Method_Public_Virtual_Final_New_Void_") || methodInfo.GetParameters().Length != 0) continue;

				List<MethodBase> callees = XrefScanner.XrefScan(methodInfo).Where(it => it.Type == XrefType.Method)
					.Select(it => it.TryResolve()).Where(it => it != null).ToList();

				if (callees.Count != 1) continue;
				if (callees[0].DeclaringType != typeof(VRCTrackingManager) || callees[0] is not MethodInfo mi || mi.ReturnType != typeof(bool))
					continue;

				return methodInfo;
			}
			return null;
			// if this is returned then vrchat must've wildly changed VRCTrackingManager
		}
		
		public static void DisplayHudMessage(string msg) => VRCUiManager.prop_VRCUiManager_0.field_Private_List_1_String_0.Add(msg);
	}
}