using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VRC;

namespace ProPlates;
public class PlateStore
{
    public readonly List<KeyValuePair<Player, Plate>> Plates = new();

    public void Add(Player player, Plate plate) => this.Plates.Add(new KeyValuePair<Player, Plate>(player, plate));

    public void Remove(KeyValuePair<Player, Plate> plate)
    {
        this.Plates.Remove(plate);
        if (!GameObject.Find(VRChatUtilityKit.Utilities.Extensions.GetPath(plate.Value.GameObject))) return;
        Object.DestroyImmediate(plate.Value.GameObject);
        Mod.Logger.Msg("Removed pronouns for {0}", plate.Key.prop_APIUser_0.displayName);
    }

    public void Empty()
    {
        foreach (KeyValuePair<Player, Plate> plate in this.Plates.ToList()) this.Remove(plate);
    }

    public bool IsEmpty => this.Plates.Count == 0;
}