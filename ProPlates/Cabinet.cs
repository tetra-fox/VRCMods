using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VRC;

namespace ProPlates
{
	public class Cabinet
	{
		public static readonly List<KeyValuePair<Player, Plate>> Shelf = new();

		public static void Add(Player player, Plate plate) => Shelf.Add(new KeyValuePair<Player, Plate>(player, plate));

		public static void Remove(KeyValuePair<Player, Plate> plate)
		{
			// only remove gameobjects if we are actively in a world
			// since gameobjects are destroyed upon leave anyway
			if (VRChatUtilityKit.Utilities.VRCUtils.WorldInfoInstance.prop_ApiWorld_0 != null) {
				Object.DestroyImmediate(plate.Value.GameObject);
				Mod.Logger.Msg("Removed pronouns for {0}", plate.Key.prop_APIUser_0.displayName);
			}
			
			Shelf.Remove(plate);
		}

		public static void Empty()
		{
			foreach (KeyValuePair<Player, Plate> plate in Shelf.ToList()) {
				Remove(plate);
			}
		}

		public static bool IsEmpty => Shelf.Count == 0;
	}
}