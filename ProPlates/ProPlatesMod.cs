using MelonLoader;
using System.Collections.Generic;
using UnityEngine;
using VRC;

[assembly: MelonInfo(typeof(ProPlates.Mod), ProPlates.BuildInfo.Name, ProPlates.BuildInfo.Version, ProPlates.BuildInfo.Author, ProPlates.BuildInfo.DownloadLink)]
[assembly: MelonGame("VRChat", "VRChat")]

namespace ProPlates;

internal static class BuildInfo
{
    public const string Name = "ProPlates";
    public const string Author = "tetra";
    public const string Version = "3.0.1";
    public const string DownloadLink = "https://github.com/tetra-fox/VRCMods";
}

public class Mod : MelonMod
{
    internal static readonly MelonLogger.Instance Logger = new(BuildInfo.Name, System.ConsoleColor.Blue);
    private static PlateStore _cabinet = new();

    public override void OnApplicationStart()
    {
        Logger.Msg("Initializing ProPlates...");
        Logger.Msg("Registering settings...");
        Settings.Register();
        Settings.OnConfigChanged += UpdatePlates;

        PronounTools.LoadTable();

        VRChatUtilityKit.Utilities.VRCUtils.OnUiManagerInit += Init;
    }

    private static void Init()
    {
        VRChatUtilityKit.Utilities.NetworkEvents.OnPlayerJoined += TryMakePlate;
        VRChatUtilityKit.Utilities.NetworkEvents.OnPlayerLeft += player => _cabinet.TryRemove(player);
        VRChatUtilityKit.Utilities.NetworkEvents.OnRoomLeft += () => _cabinet = new PlateStore();

        Logger.Msg("Initialized!");
    }
    
    private static string GetPlayerPronouns(Player player)
    {
        // combine bio and status to make my life easier
        string statsAndBio = $"{player.prop_APIUser_0.statusDescription} {player.prop_APIUser_0.bio}".ToLower();

        List<string> pronouns = PronounTools.GetPronouns(statsAndBio);

        if (pronouns.Count - Settings.MaxPronouns.Value is var toRemove and > 0)
            pronouns.RemoveRange(Settings.MaxPronouns.Value, toRemove);

        return pronouns.Count != 0 ? string.Join("/", pronouns) : null;
    }

    private static void TryMakePlate(Player player)
    {
        string pronouns = GetPlayerPronouns(player);
        if (!player || pronouns == null || Settings.MaxPronouns.Value == 0) return;
        
        Logger.Msg("Setting pronouns for {0}", player.prop_APIUser_0.displayName);
        Plate customPlate = new(player, pronouns, Color.white, containerPrefix: "ProPlates");
        _cabinet.Add(player, customPlate);
    }
    
    private static void UpdatePlates()
    {
        // if MaxPronouns is zero, empty the PlateStore (destroy all plate objects)
        if (Settings.MaxPronouns.Value < 1)
        {
            _cabinet.Empty();
            return;
        }

        // if we are updating and cabinet is empty, we probably went from 0 to non zero,
        // so we need to recreate the plate objects as they've been destroyed (or did not exist in the first place)
        if (_cabinet.IsEmpty)
        {
            foreach (Player player in PlayerManager.field_Private_Static_PlayerManager_0.field_Private_List_1_Player_0)
                TryMakePlate(player);
            return;
        }

        // update text in place
        foreach (KeyValuePair<Player, Plate> plate in _cabinet.GetPlates)
        {
            string pronouns = GetPlayerPronouns(plate.Key);
            if (pronouns == "") return;
            plate.Value.Text = pronouns;
            Logger.Msg("Updated {0}'s pronouns with new limit", plate.Key.prop_APIUser_0.displayName);
        }
    }
}