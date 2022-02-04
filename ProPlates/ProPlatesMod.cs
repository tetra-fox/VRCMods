using MelonLoader;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using VRC;
using Array = System.Array;
using StringComparer = System.StringComparer;

[assembly: MelonInfo(typeof(ProPlates.Mod), ProPlates.BuildInfo.Name, ProPlates.BuildInfo.Version, ProPlates.BuildInfo.Author, ProPlates.BuildInfo.DownloadLink)]
[assembly: MelonGame("VRChat", "VRChat")]

namespace ProPlates
{
	internal static class BuildInfo
	{
		public const string Name = "ProPlates";
		public const string Author = "tetra";
		public const string Version = "2.1.1";
		public const string DownloadLink = "https://github.com/tetra-fox/VRCMods";
	}

	public class Mod : MelonMod
	{
		internal static readonly MelonLogger.Instance Logger = new(BuildInfo.Name, System.ConsoleColor.Blue);
		private static string[] _pronounTable;
		private static List<string> _pronounPairs = new();
		private static readonly Regex PronounParser = new(@"^pronouns˸ (.*$)", RegexOptions.IgnoreCase | RegexOptions.Multiline);

		public override void OnApplicationStart()
		{
			Logger.Msg("Initializing ProPlates...");
			Logger.Msg("Registering settings...");
			Settings.Register();
			Settings.OnConfigChanged += UpdatePlates;

			Logger.Msg("Loading pronoun table...");
			
			// pronoun table sourced from https://github.com/witch-house/pronoun.is/blob/master/resources/pronouns.tab
			Stream s = Assembly.GetExecutingAssembly().GetManifestResourceStream(BuildInfo.Name + ".pronouns.csv");
			
			_pronounTable = Tools.ParseTable(s);
			_pronounPairs = Tools.GeneratePairs(_pronounTable);

			VRChatUtilityKit.Utilities.VRCUtils.OnUiManagerInit += Init;
		}

		private static void Init()
		{
			VRChatUtilityKit.Utilities.NetworkEvents.OnPlayerJoined += player =>
			{
				string pronouns = GetPlayerPronouns(player);
				if (player && pronouns != null && Settings.MaxPronouns.Value > 0) MakePlate(player, pronouns);
			};

			VRChatUtilityKit.Utilities.NetworkEvents.OnPlayerLeft += player =>
			{
				KeyValuePair<Player, Plate> plateToRemove = Cabinet.Shelf.Find(pair => pair.Key == player);
				if (plateToRemove.Equals(new KeyValuePair<Player, Plate>())) return; // no matches
				Cabinet.Remove(plateToRemove);
			};
			
			VRChatUtilityKit.Utilities.NetworkEvents.OnRoomLeft += Cabinet.Empty;

			Logger.Msg("Initialized!");
		}

		private static void MakePlate(Player player, string text)
		{
			Logger.Msg("Setting pronouns for {0}", player.prop_APIUser_0.displayName);
			Plate customPlate = new(player, text, Color.white, containerPrefix: "ProPlates");

			Cabinet.Add(player, customPlate);
		}

		private static string GetPlayerPronouns(Player player)
		{
			string[] playerPronouns = PronounParser.Match(player.prop_APIUser_0.bio).Groups[1].Value.Split('⁄', '＼');

			// sanitize because pronoun jokes aren't funny
			playerPronouns = playerPronouns.Distinct(StringComparer.CurrentCultureIgnoreCase).Where(p => _pronounTable.Contains(p.ToLower())).ToArray();

			// fall back to exact string comparison (now in status too)
			if (playerPronouns.Length < 1) playerPronouns = ParseEntireProfile(player);

			if (playerPronouns.Length > Settings.MaxPronouns.Value) Array.Resize(ref playerPronouns, Settings.MaxPronouns.Value);

			return playerPronouns.Length < 1 ? null : string.Join("/", playerPronouns);
		}

		private static string[] ParseEntireProfile(Player player)
		{
			string[] playerPronouns = { };

			// combine bio and status to make my life easier
			string playerInfo = string.Concat(player.prop_APIUser_0.statusDescription, player.prop_APIUser_0.bio).ToLower();

			string foundPronouns = _pronounPairs.FirstOrDefault(pair => playerInfo.Contains(pair));

			if (!string.IsNullOrEmpty(foundPronouns)) playerPronouns = foundPronouns.Split('⁄', '＼');

			return playerPronouns;
		}

		private static void UpdatePlates()
		{
			// if maxpronouns are zero, empty the cabinet (destroy all plate objects)
			if (Settings.MaxPronouns.Value < 1) {
				Cabinet.Empty();
				return;
			}
			
			// if we are updating and cabinet is empty, we probably went from 0 to non zero,
			// so we need to recreate the plate objects as they've been destroyed (or did not exist in the first place)
			if (Cabinet.IsEmpty) {	
				foreach (Player player in PlayerManager.field_Private_Static_PlayerManager_0.field_Private_List_1_Player_0) {
					string pronouns = GetPlayerPronouns(player);
					if (pronouns == null) continue;
					MakePlate(player, pronouns);
				}
				return;
			}
			
			// update text in place
			foreach (KeyValuePair<Player, Plate> plate in Cabinet.Shelf) {
				string pronouns = GetPlayerPronouns(plate.Key);
				if (pronouns == "") return;
				plate.Value.Text = pronouns;
				Logger.Msg("Updated {0}'s pronouns with new limit", plate.Key.prop_APIUser_0.displayName);
			}
		}

#if DEBUG
		public override void OnUpdate()
		{
			// PlateAPI tests
			if (Input.GetKeyDown(KeyCode.LeftBracket)) {
				// change random plate to random text
				KeyValuePair<Player, Plate> randomPlate = GetRandomPlate();
				
				randomPlate.Value.Text = new System.Random().Next().ToString();
				Logger.Msg("updated {0}'s plate text", randomPlate.Value.Player.prop_APIUser_0.displayName);
			} else if (Input.GetKeyDown(KeyCode.RightBracket)) {
				// move random plate to random player
				KeyValuePair<Player, Plate> randomPlate = GetRandomPlate();
				Player randomPlayer = GetRandomPlayer();
				
				randomPlate.Value.Player = randomPlayer;
				// update the key as well
				Cabinet.Shelf.Add(new KeyValuePair<Player, Plate>(randomPlayer, randomPlate.Value));
				Cabinet.Shelf.Remove(randomPlate);
				Logger.Msg("moved {0}'s plate to {0}", randomPlate.Value.Player.prop_APIUser_0.displayName, randomPlayer.prop_APIUser_0.displayName);
			} else if (Input.GetKeyDown(KeyCode.Quote)) {
				// change random nameplate to random color
				KeyValuePair<Player, Plate> randomPlate = GetRandomPlate();
				
				randomPlate.Value.Color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
				Logger.Msg("updated {0}'s plate color", randomPlate.Value.Player.prop_APIUser_0.displayName);
			} else if (Input.GetKeyDown(KeyCode.Comma)) {
				// change random nameplate to random pos
				KeyValuePair<Player, Plate> randomPlate = GetRandomPlate();

				randomPlate.Value.Position = new Vector3(Random.Range(-100.0f, 100.0f), Random.Range(-100.0f, 100.0f), Random.Range(-100.0f, 100.0f));
				Logger.Msg("updated {0}'s plate position", randomPlate.Value.Player.prop_APIUser_0.displayName);
			} else if (Input.GetKeyDown(KeyCode.Semicolon)) {
				// assign everyone a nameplate
				foreach (Player player in PlayerManager.field_Private_Static_PlayerManager_0.field_Private_List_1_Player_0) {
					MakePlate(player, "test");
				}
			}
		}

		private static Player GetRandomPlayer()
		{
			List<Player> players = PlayerManager.field_Private_Static_PlayerManager_0.field_Private_List_1_Player_0.ToArray().ToList();
			return players[new System.Random().Next(players.Count)];
		}

		private static KeyValuePair<Player, Plate> GetRandomPlate()
		{
			return Cabinet.Shelf.Find(pair => pair.Key == GetRandomPlayer());
		}
#endif
	}
}