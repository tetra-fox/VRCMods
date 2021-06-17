# Mod Directory
* [QMFreeze](#QMFreeze)
* [CalibrateConfirm](#CalibrateConfirm)
* [UnmuteSound](#UnmuteSound)

## QMFreeze
QMFreeze freezes your avatar when you open the quick menu, in case you are falling or otherwise unable to click anything on the quick menu.
### Optional dependencies
* [UIExpansionKit](https://github.com/knah/VRCMods/) (**HIGHLY** recommended. This mod hooks into UIX's `VRCUiManagerInit` which is much more efficient.)

## CalibrateConfirm
CalibrateConfirm adds a confirmation prompt when you click calibrate, in the case that you've accidentally clicked calibrate and don't want to stand up to recalibrate your avatar
### Optional dependencies
* [UIExpansionKit](https://github.com/knah/VRCMods/) (**HIGHLY** recommended. This mod hooks into UIX's `VRCUiManagerInit` which is much more efficient.)

## ~~UnmuteSound~~
UnmuteSound plays an audio cue upon unmuting your microphone.

**NOTE: this mod is broken on ML 0.4.0-- ML 0.4.0 uses a fork of Harmony, HarmonyX, to do its method patching, which seems to have different behavior that I don't understand**
