## Mod Directory
- [QMFreeze](#qmfreeze)
- [CalibrateConfirm](#calibrateconfirm)
- [ProPlates](#proplates)
- [~~UnmuteSound~~](#unmutesound)

## A note before continuing
The mods listed here require **VRChatUtilityKit** by **loukylor**. Please be sure to install it before using any of my mods! You can get it [here](https://github.com/loukylor/VRC-Mods).

## QMFreeze
QMFreeze freezes your avatar when you open the quick menu, in case you are falling or otherwise unable to click anything on the quick menu.

## CalibrateConfirm
CalibrateConfirm adds a confirmation prompt when you click calibrate, in the case that you've accidentally clicked calibrate and don't want to stand up to recalibrate your avatar

## ProPlates
ProPlates displays a player's pronouns under their nameplate.

**If you'd like to set your own pronouns**, simply add a line to your VRChat bio formatted like this: `pronouns: xxx/xxx/xxx`. Note that this is **NOT** case-sensitive. You may add as many pronouns as you like, however, this mod only displays the first 8 by default. This value is configurable in `MelonPreferences.cfg`, however I don't really recommend going over that because if someone has a lot of pronouns, you'll just see a floating wall of text.

>**NOTE:** This mod also has some basic safeguards against trolls because even in the current year, people still think attack helicopter jokes are funny for whatever reason.

Safeguards include:
- Default display limit of 8 pronouns
- Duplicate checking
- Pronoun filtering (`pronouns.csv` contains the most common good faith pronouns. If you feel that more pronouns belong here, feel free to send a pull request!)

![a](https://i.imgur.com/AZEl7LA.png)


## ~~UnmuteSound~~
~~UnmuteSound plays an audio cue upon unmuting your microphone.~~

**NOTE: this mod is broken on ML 0.4.0-- ML 0.4.0 uses a fork of Harmony, HarmonyX, to do its method patching, which seems to have different behavior that I don't understand**
