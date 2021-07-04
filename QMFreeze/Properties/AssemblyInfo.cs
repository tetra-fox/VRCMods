using MelonLoader;
using System.Reflection;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle(QMFreeze.BuildInfo.Name)]
[assembly: AssemblyProduct(QMFreeze.BuildInfo.Name)]
[assembly: AssemblyCopyright(QMFreeze.BuildInfo.Author)]
[assembly: AssemblyFileVersion(QMFreeze.BuildInfo.Version)]
[assembly: ComVisible(false)]
[assembly: MelonInfo(typeof(QMFreeze.Mod), QMFreeze.BuildInfo.Name, QMFreeze.BuildInfo.Version, QMFreeze.BuildInfo.Author, QMFreeze.BuildInfo.DownloadLink)]
[assembly: MelonGame("VRChat", "VRChat")]
[assembly: MelonAdditionalDependencies("VRChatUtilityKit")]