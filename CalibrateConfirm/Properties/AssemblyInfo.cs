using MelonLoader;
using System.Reflection;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle(CalibrateConfirm.BuildInfo.Name)]
[assembly: AssemblyProduct(CalibrateConfirm.BuildInfo.Name)]
[assembly: AssemblyCopyright(CalibrateConfirm.BuildInfo.Author)]
[assembly: AssemblyFileVersion(CalibrateConfirm.BuildInfo.Version)]
[assembly: ComVisible(false)]
[assembly: MelonInfo(typeof(CalibrateConfirm.CalibrateConfirmMod), CalibrateConfirm.BuildInfo.Name, CalibrateConfirm.BuildInfo.Version, CalibrateConfirm.BuildInfo.Author, CalibrateConfirm.BuildInfo.DownloadLink)]
[assembly: MelonGame("VRChat", "VRChat")]
[assembly: MelonAdditionalDependencies(new string[1] { "VRChatUtilityKit" })]