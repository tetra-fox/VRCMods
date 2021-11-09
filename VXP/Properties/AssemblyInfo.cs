using System;
using MelonLoader;
using System.Reflection;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle(VXP.BuildInfo.Name)]
[assembly: AssemblyProduct(VXP.BuildInfo.Name)]
[assembly: AssemblyCopyright(VXP.BuildInfo.Author)]
[assembly: AssemblyFileVersion(VXP.BuildInfo.Version)]
[assembly: ComVisible(false)]
[assembly: MelonInfo(typeof(VXP.Mod), VXP.BuildInfo.Name, VXP.BuildInfo.Version, VXP.BuildInfo.Author, VXP.BuildInfo.DownloadLink)]
[assembly: MelonGame("VRChat", "VRChat")]
[assembly: MelonColor(ConsoleColor.Magenta)]
[assembly: MelonAdditionalDependencies(new string[] { "VRChatUtilityKit" })]