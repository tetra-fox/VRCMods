using MelonLoader;

namespace QMFreeze
{
    public static class QMFreezeSettings

    {
        public static bool Enabled { get; set; } = true;
        public static bool RestoreVelocity { get; set; } = false;

        public static void Register()
        {
            MelonPreferences.CreateCategory(BuildInfo.Name, BuildInfo.Name);
            MelonPreferences.CreateEntry(BuildInfo.Name, "Enabled", true, "Enable QMFreeze");
            MelonPreferences.CreateEntry(BuildInfo.Name, "RestoreVelocity", false, "Restore velocity");
        }

        public static void Apply()
        {
            bool _Enabled = MelonPreferences.GetEntryValue<bool>(BuildInfo.Name, "Enabled");
            // Unfreeze the player if they disable QMFreeze while the QM is open, otherwise they will fly infinitely if they try to move
            if (QMFreezeMod.Frozen && !_Enabled) QMFreezeMod.Unfreeze();
            // Then actually set the pref lol
            Enabled = _Enabled;

            RestoreVelocity = MelonPreferences.GetEntryValue<bool>(BuildInfo.Name, "RestoreVelocity");
        }
    }
}