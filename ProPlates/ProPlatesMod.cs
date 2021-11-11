using MelonLoader;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using ProPlates.Components;
using TMPro;
using UnhollowerRuntimeLib;
using UnityEngine;
using VRC;
using Array = System.Array;
using Object = UnityEngine.Object;
using StringComparer = System.StringComparer;

[assembly: MelonInfo(typeof(ProPlates.Mod), ProPlates.BuildInfo.Name, ProPlates.BuildInfo.Version, ProPlates.BuildInfo.Author, ProPlates.BuildInfo.DownloadLink)]
[assembly: MelonGame("VRChat", "VRChat")]
[assembly: MelonColor(System.ConsoleColor.Blue)]

namespace ProPlates
{
    internal static class BuildInfo
    {
        public const string Name = "ProPlates";
        public const string Author = "tetra";
        public const string Version = "2.0.0";
        public const string DownloadLink = "https://github.com/tetra-fox/VRCMods";
    }

    public class Mod : MelonMod
    {
        private static string[] _pronounTable;
        private static readonly Regex PronounParser = new(@"^pronouns˸ (.*$)", RegexOptions.IgnoreCase | RegexOptions.Multiline);
        private static readonly List<string> PronounPairs = new();
        public override void OnApplicationStart()
        {
            MelonLogger.Msg("Initializing ProPlates...");
            MelonLogger.Msg("Registering settings...");
            Settings.Register();
            Settings.OnConfigChanged += ReloadPronouns;

            MelonLogger.Msg("Registering components...");
            ClassInjector.RegisterTypeInIl2Cpp<OpacityListener>();

            MelonLogger.Msg("Loading pronoun table...");
            using Stream s = Assembly.GetExecutingAssembly().GetManifestResourceStream(BuildInfo.Name + ".pronouns.csv");

            // pronoun table sourced from https://github.com/witch-house/pronoun.is/blob/master/resources/pronouns.tab
            _pronounTable = new StreamReader(s!).ReadToEnd().Split(',', '\n');

            // BIG LIST, it's *probably* fine
            foreach (string p1 in _pronounTable)
            {
                foreach (string p2 in _pronounTable)
                {
                    // vrchat please stop "sanitizing" user input by replacing characters with unicode equivalents
                    // it looks horrible and is terribly hacky
                    // and so is this lol
                    PronounPairs.AddRange(new List<string> { $"{p1}⁄{p2}", $"{p1}＼{p2}" });
                }
            }

            VRChatUtilityKit.Utilities.VRCUtils.OnUiManagerInit += Init;
        }

        private static void Init()
        {
            VRChatUtilityKit.Utilities.NetworkEvents.OnPlayerJoined += player =>
            {
                if (player) MakePlate(player, GetPlayerPronouns(player));
            };

            MelonLogger.Msg("Initialized!");
        }

        private static void MakePlate(Player player, string text)
        {
            if (string.IsNullOrEmpty(text)) return;
            if (Settings.MaxPronouns.Value < 1) return;

            PlayerNameplate nameplate = player._vrcplayer.field_Public_PlayerNameplate_0;
            if (nameplate.transform.Find("Contents/ProPlates Container")) return;

            MelonLogger.Msg("Setting pronouns for {0}", player.prop_APIUser_0.displayName);

            Transform pronounPlate = Object.Instantiate(nameplate.transform.Find("Contents/Quick Stats"),
                nameplate.transform.Find("Contents"), false);

            pronounPlate.name = "ProPlates Container";
            pronounPlate.localPosition = new Vector3(0f, -60f, 0f); // y coordinate is in increments of 30, yes i'm aware the avatar DL progress covers this
            pronounPlate.gameObject.active = true;

            OpacityListener opacityListener = pronounPlate.gameObject.AddComponent<OpacityListener>();
            opacityListener.reference = nameplate.transform.Find("Contents/Main/Background").GetComponent<ImageThreeSlice>();
            opacityListener.target = pronounPlate.gameObject.GetComponent<ImageThreeSlice>();

            // remove unnecessary gameobjects and set pronoun text
            for (int i = pronounPlate.childCount; i > 0; i--)
            {
                Transform c = pronounPlate.GetChild(i - 1);
                if (c.name == "Trust Text")
                {
                    c.name = "Text";
                    c.GetComponent<TextMeshProUGUI>().text = text;
                    c.GetComponent<TextMeshProUGUI>().color = Color.white;
                    continue;
                }
                Object.DestroyImmediate(c.gameObject);
            }
        }

        private static string GetPlayerPronouns(Player player)
        {
            string[] playerPronouns = PronounParser.Match(player.prop_APIUser_0.bio).Groups[1].Value.Split('⁄', '＼');

            // sanitize because pronoun jokes aren't funny
            playerPronouns = playerPronouns.Distinct(StringComparer.CurrentCultureIgnoreCase).Where(p => _pronounTable.Contains(p.ToLower())).ToArray();

            // fall back to exact string comparison (now in status too)
            if (playerPronouns.Length < 1)
            {
                //MelonLogger.Msg("No ProPlates format found, falling back");
                playerPronouns = ParseEntireProfile(player);
            }

            if (playerPronouns.Length > Settings.MaxPronouns.Value) Array.Resize(ref playerPronouns, Settings.MaxPronouns.Value);

            return playerPronouns.Length < 1 ? null : string.Join("/", playerPronouns);
        }

        private static string[] ParseEntireProfile(Player player)
        {
            string[] playerPronouns = { };

            // combine bio and status to make my life easier
            string playerInfo = string.Concat(player.prop_APIUser_0.statusDescription, player.prop_APIUser_0.bio).ToLower();

            string foundPronouns = PronounPairs.FirstOrDefault(pair => playerInfo.Contains(pair));

            if (!string.IsNullOrEmpty(foundPronouns)) playerPronouns = foundPronouns.Split('⁄', '＼');

            return playerPronouns;
        }

        private static void ReloadPronouns()
        {
            try
            {
                Il2CppSystem.Collections.Generic.List<Player> players = PlayerManager.field_Private_Static_PlayerManager_0.field_Private_List_1_Player_0;
                foreach (Player p in players)
                {
                    try
                    {
                        Object.DestroyImmediate(p.prop_VRCPlayer_0.field_Public_PlayerNameplate_0.transform
                            .Find("Contents/ProPlates Container").gameObject);
                        MelonLogger.Msg("Removed pronouns for {0}", p.prop_APIUser_0.displayName);
                    }
                    catch { }
                    MakePlate(p, GetPlayerPronouns(p));
                }
            }
            catch { }
        }
    }
}