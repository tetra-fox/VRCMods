using System.Collections.Generic;
using System.IO;

namespace ProPlates
{
	internal static class Tools
	{
		public static string[] ParseTable(Stream fileStream)
		{
			return new StreamReader(fileStream!).ReadToEnd().Split(',', '\n');
		}

		public static List<string> GeneratePairs(string[] table)
		{
			List<string> pairs = new();
			// BIG LIST, it's *probably* fine
			foreach (string p1 in table) {
				foreach (string p2 in table) {
					// vrchat please stop "sanitizing" user input by replacing characters with unicode equivalents
					// it looks horrible and is terribly hacky
					// and so is this lol
					pairs.AddRange(new List<string> {$"{p1}⁄{p2}", $"{p1}＼{p2}"});
				}
			}

			return pairs;
		}
	}
}