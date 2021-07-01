using MelonLoader;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Il2CppSystem.Collections.Generic;
using TMPro;
using UnityEngine;
using VRC;
using VRC.Core;

namespace ProPlates
{
    public class BuildInfo
    {
        public const string Name = "ProPlates";
        public const string Author = "tetra";
        public const string Version = "1.0.0";
        public const string DownloadLink = "https://github.com/tetra-fox/VRCMods";
    }

    public class ProPlatesMod : MelonMod
    {
        private static string[] _pronounTable;

        private static readonly Regex PronounParser = new(@"^pronouns˸ (.*$)", RegexOptions.IgnoreCase | RegexOptions.Multiline);

        public override void OnApplicationStart()
        {
            if (!MelonHandler.Mods.Any(m => m.Info.Name.Equals("VRChatUtilityKit")))
            {
                MelonLogger.Error("This mod requires VRChatUtilityKit to run! Download it from loukylor's GitHub:");
                MelonLogger.Error("https://github.com/loukylor/VRC-Mods");
                return;
            }

            MelonLogger.Msg("Initializing ProPlates...");

            MelonLogger.Msg("Registering settings...");
            ProPlatesSettings.Register();
            ProPlatesSettings.Apply();

            MelonLogger.Msg("Loading pronoun table...");

            Stream s = Assembly.GetExecutingAssembly().GetManifestResourceStream(BuildInfo.Name + ".pronouns.csv");

            // pronoun table sourced from https://github.com/witch-house/pronoun.is/blob/master/resources/pronouns.tab
            _pronounTable = new StreamReader(s).ReadToEnd().Split(',', '\n');

            VRChatUtilityKit.Utilities.VRCUtils.OnUiManagerInit += Init;
        }

        private static void Init()
        {
            VRChatUtilityKit.Utilities.NetworkEvents.OnPlayerJoined += player =>
            {
                if (player != null) SetPronouns(player);
            };

            MelonLogger.Msg("Initialized!");
        }

        private static void SetPronouns(Player player)
        {
            if (ProPlatesSettings.MaxPronouns == 0) return;

            // vrchat pls stop obfuscating this is horrible
            PlayerNameplate nameplate = player.prop_VRCPlayer_0.field_Public_PlayerNameplate_0;
            APIUser apiUser = player.prop_APIUser_0;

            if (nameplate.transform.Find("Contents/Pronouns Container")) return;

            // parse bio for pronouns
            string[] playerPronouns = PronounParser.Match(apiUser.bio).Groups[1].Value.Split('⁄', '＼');
            // then sanitize because pronoun jokes aren't funny
            playerPronouns = playerPronouns.Distinct(StringComparer.CurrentCultureIgnoreCase).Where(p => _pronounTable.Contains(p.ToLower())).ToArray();

            if (playerPronouns.Length > ProPlatesSettings.MaxPronouns) Array.Resize(ref playerPronouns, ProPlatesSettings.MaxPronouns);

            if (playerPronouns.Length < 1) return;

            MelonLogger.Msg("Setting pronouns for {0}", apiUser.displayName);

            // create new plate
            Transform pronounPlate = GameObject.Instantiate(nameplate.transform.Find("Contents/Quick Stats"),
                nameplate.transform.Find("Contents"), false);

            pronounPlate.name = "Pronouns Container";
            pronounPlate.localPosition = new Vector3(0f, -60f, 0f); // y coordinate is in increments of 30, yes i'm aware the avatar DL progress covers this
            pronounPlate.gameObject.active = true;

            // remove unnecessary gameobjects and set pronoun text
            for (int i = pronounPlate.childCount; i > 0; i--)
            {
                Transform c = pronounPlate.GetChild(i - 1);
                if (c.name == "Trust Text")
                {
                    c.name = "Pronouns Text";
                    c.GetComponent<TextMeshProUGUI>().text = string.Join("/", playerPronouns);
                    c.GetComponent<TextMeshProUGUI>().color = Color.white;
                }
                else
                {
                    GameObject.DestroyImmediate(c.gameObject);
                }
            }
        }

        public static void ReloadPronouns()
        {
            try
            {
                List<Player> players = PlayerManager.field_Private_Static_PlayerManager_0.field_Private_List_1_Player_0;
                foreach (Player p in players)
                {
                    try
                    {
                        GameObject.DestroyImmediate(p.prop_VRCPlayer_0.field_Public_PlayerNameplate_0.transform
                            .Find("Contents/Pronouns Container").gameObject);
                        MelonLogger.Msg("Destroyed pronouns for {0}", p.prop_APIUser_0.displayName);
                    }
                    catch { }
                    SetPronouns(p);
                }
            }
            catch { }
        }

        public override void OnPreferencesLoaded() => ProPlatesSettings.Apply();

        public override void OnPreferencesSaved() => ProPlatesSettings.Apply();
    }
}