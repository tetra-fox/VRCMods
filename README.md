<p align="center">
    <img src="https://img.shields.io/static/v1?label=melonloader&message=v0.5.4&color=green&style=flat-square">
    <img src="https://img.shields.io/static/v1?label=vrchat&message=1192&color=00c9ab&style=flat-square">
    <img src="https://img.shields.io/github/workflow/status/tetra-fox/VRCMods/Build?style=flat-square">
    <img src="https://img.shields.io/github/downloads/tetra-fox/VRCMods/total?color=informational&style=flat-square">
</p>

## Mod Directory
- [AdBlocker](../../tree/master/AdBlocker)
- [CalibrateConfirm](../../tree/master/CalibrateConfirm)
- [ProPlates](../../tree/master/ProPlates)
- [QMFreeze](../../tree/master/QMFreeze)
- [UnmuteSound](../../tree/master/UnmuteSound)
<!-- - [ComfierVRMenu])(../../tree/master/ComfierVRMenu) SOON™ -->
<!-- - [VXP])(../../tree/master/VXP) SOON™ -->
  
## Preface
Modification of the VRChat client is a violation of VRChat's [Terms of Service](https://hello.vrchat.com/legal), is not officially supported or endorsed by VRChat, and **can result in an account ban**. However, if using mods that _enhance_ your user experience and do not harm the experience of others, it is generally safe to say that no bans will be issued, as generally, they are undetectable by other users.

The mods listed here require **VRChatUtilityKit**. Please be sure to install it before using any of my mods! You can get it [here](https://github.com/SleepyVRC/Mods/releases).

## Building Locally
1. Ensure you have the latest [.NET SDK 6](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
2. Ensure you have run VRChat with [MelonLoader](https://github.com/LavaGang/MelonLoader) installed at least once (to generate the needed assemblies)
3. Install [VRChatUtilityKit](https://github.com/SleepyVRC/Mods/releases) to `path/to/VRChat/Mods`
4. Change the [_first_ `VRChatPath` property](https://github.com/tetra-fox/VRCMods/blob/main/Directory.Build.props#L6) in `Directory.build.props` to the path of your VRChat installation.
5. Build in your favorite IDE or run `dotnet build` in the repository's directory
