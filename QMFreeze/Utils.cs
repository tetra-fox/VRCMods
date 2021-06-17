using MelonLoader;
using System;
using System.Linq;
using VRC.Core;

namespace QMFreeze
{
    internal class Utils
    {
        internal static bool HasMod(string modName)
        {
            return MelonHandler.Mods.Any(m => m.Info.Name.Equals(modName));
        }

        // Stolen from Psychloor
        // https://github.com/Psychloor/PlayerRotater/blob/4552fdbf856cf7a3d658b2e17c9f890cafef004d/PlayerRotater/Utilities.cs#L45-L101
        internal static void CheckWorld()
        {
            MelonLogger.Msg("Checking world...");
            string worldId = RoomManager.field_Internal_Static_ApiWorld_0.id;

            // So, there used to be a world check using the emmVRC API, but it seems like that
            // endpoint always returns an empty response, so I've just removed it altogether.
            // We'll just check the world tags instead. Though I guess that's one less network
            // call that needs to be made. Yay, security? Or optimization? idk.

            API.Fetch<ApiWorld>(
                worldId,
                new Action<ApiContainer>(
                    container =>
                    {
                        ApiWorld apiWorld;
                        if ((apiWorld = container.Model.TryCast<ApiWorld>()) != null)
                        {
                            foreach (string worldTag in apiWorld.tags)
                                if (worldTag.IndexOf("game", StringComparison.OrdinalIgnoreCase) >= 0)
                                {
                                    MelonLogger.Msg("QMFreeze NOT permitted");
                                    Mod.FreezeAllowed = false;
                                    return;
                                }
                            MelonLogger.Msg("QMFreeze permitted");
                            Mod.FreezeAllowed = true;
                        }
                        else
                        {
                            MelonLogger.Error("Failed to cast ApiModel to ApiWorld");
                        }
                    }),
                disableCache: false);
        }
    }
}