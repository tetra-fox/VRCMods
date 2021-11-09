using MelonLoader;

namespace VXP
{
    public class BuildInfo
    {
        public const string Name = "VXP";
        public const string Author = "tetra & tjhorner";
        public const string Version = "1.0.0";
        public const string DownloadLink = "https://github.com/tetra-fox/VRCMods";
    }

    public class Mod : MelonMod
    {
        public override void OnUpdate()
        {
            
        }
    }

    internal enum Expression
    {
        Default = 0,
        Neutral = 1,
        Calm = 2,
        Happy = 3,
        Sad = 4,
        Angry = 5,
        Fearful = 6,
        Disgust = 7,
        Surprised = 8
    }
}