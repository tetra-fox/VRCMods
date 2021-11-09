using MelonLoader;
using System.Reflection;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle(AdBlocker.BuildInfo.Name)]
[assembly: AssemblyProduct(AdBlocker.BuildInfo.Name)]
[assembly: AssemblyCopyright(AdBlocker.BuildInfo.Author)]
[assembly: AssemblyFileVersion(AdBlocker.BuildInfo.Version)]
[assembly: ComVisible(false)]
[assembly: MelonInfo(typeof(AdBlocker.Mod), AdBlocker.BuildInfo.Name, AdBlocker.BuildInfo.Version, AdBlocker.BuildInfo.Author, AdBlocker.BuildInfo.DownloadLink)]
[assembly: MelonGame("VRChat", "VRChat")]