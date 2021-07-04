﻿using MelonLoader;
using System;
using System.Reflection;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle(ProPlates.BuildInfo.Name)]
[assembly: AssemblyProduct(ProPlates.BuildInfo.Name)]
[assembly: AssemblyCopyright(ProPlates.BuildInfo.Author)]
[assembly: AssemblyFileVersion(ProPlates.BuildInfo.Version)]
[assembly: ComVisible(false)]
[assembly: MelonInfo(typeof(ProPlates.Mod), ProPlates.BuildInfo.Name, ProPlates.BuildInfo.Version, ProPlates.BuildInfo.Author, ProPlates.BuildInfo.DownloadLink)]
[assembly: MelonGame("VRChat", "VRChat")]
[assembly: MelonAdditionalDependencies("VRChatUtilityKit")]
[assembly: MelonColor(ConsoleColor.Blue)]