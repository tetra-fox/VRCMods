using MelonLoader;
using System.Linq;

namespace UnmuteSound
{
    internal class Utils
    {
        internal static bool HasMod(string modName)
        {
            return MelonHandler.Mods.Any(m => m.Info.Name.Equals(modName));
        }
    }
}