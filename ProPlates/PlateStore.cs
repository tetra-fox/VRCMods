using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VRC;

namespace ProPlates;
public class PlateStore
{
    public Dictionary<Player, Plate> GetPlates { get; } = new();
    public bool IsEmpty => this.GetPlates.Count == 0;

    public void Add(Player player, Plate plate) => this.GetPlates[player] = plate;

    public void TryRemove(Player player)
    {
        if (!this.GetPlates.TryGetValue(player, out Plate plate)) return;
        this.GetPlates.Remove(player);
        if (!GameObject.Find(VRChatUtilityKit.Utilities.Extensions.GetPath(plate.GameObject))) return;
        Object.DestroyImmediate(plate.GameObject);
        Mod.Logger.Msg("Removed pronouns for {0}", player.prop_APIUser_0.displayName);
    }

    public void Empty()
    {
        foreach (Player player in this.GetPlates.Keys.ToList()) this.TryRemove(player);
    }
}