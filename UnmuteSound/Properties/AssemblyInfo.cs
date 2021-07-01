using MelonLoader;
using System.Reflection;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle(UnmuteSound.BuildInfo.Name)]
[assembly: AssemblyProduct(UnmuteSound.BuildInfo.Name)]
[assembly: AssemblyCopyright(UnmuteSound.BuildInfo.Author)]
[assembly: AssemblyFileVersion(UnmuteSound.BuildInfo.Version)]
[assembly: ComVisible(false)]
[assembly: MelonInfo(typeof(UnmuteSound.UnmuteSoundMod), UnmuteSound.BuildInfo.Name, UnmuteSound.BuildInfo.Version, UnmuteSound.BuildInfo.Author)]
[assembly: MelonGame("VRChat", "VRChat")]
[assembly: MelonAdditionalDependencies(new string[1] { "VRChatUtilityKit" })]