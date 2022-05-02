using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace ProPlates;

/* This was implementation was developed largely in part by @J-E-Mc. Thanks! */

internal static class PronounTools
{
    private static List<string> _allowedPronouns = new();

    public static void LoadTable()
    {
        Mod.Logger.Msg("Loading pronoun table...");
        // pronouns sourced from https://github.com/witch-house/pronoun.is/blob/master/resources/pronouns.tab
        using Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(BuildInfo.Name + ".pronouns.json");
        using StreamReader reader = new(stream!);
        string json = reader.ReadToEnd();
        _allowedPronouns = JsonConvert.DeserializeObject<List<string>>(json);
    }
    public static List<string> GetPronouns(string bio)
    {
        MatchCollection matches = Regex.Matches(bio, @"(?:^|\W)([a-zA-Z]*[⁄＼][a-zA-Z⁄＼]*)(?:$|\W)");
        List<string> pronouns = new();

        // O(n^2) ehehehe
        foreach (Match match in matches)
        {
            // split into separate pronouns
            string[] foundPronouns = match.Groups[1].Value.Split('⁄', '＼');
            foreach (string text in foundPronouns)
            {
                // ignore pronouns not in the pronouns table
                if (_allowedPronouns.All(s => s != text.ToLower())) continue;
                // add to list (ignoring duplicates)
                if (!pronouns.Contains(text)) pronouns.Add(text);
            }
        }

        return pronouns;
    }
}