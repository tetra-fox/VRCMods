## ProPlates ![VRCUK](https://img.shields.io/badge/VRChatUtilityKit-required-orange?style=flat-square)

ProPlates displays a player's pronouns under their nameplate.

ProPlates will attempt to look for pronouns formatted as `xxx/xxx` **OR** `xxx\xxx` (limited to two) anywhere in your
bio and status text.

**If that doesn't work, you want to display more than two, or you'd just like to manually configure how ProPlates
displays your pronouns**, simply add a line to your VRChat bio formatted like this: `pronouns: xxx/xxx/xxx`. Note that
this is **NOT** case-sensitive. You may add as many pronouns as you like, however, this mod only displays the first 8 by
default. This value is configurable in `MelonPreferences.cfg`, however I don't really recommend going over that because
if someone has a lot of pronouns, you'll just see a floating wall of text.

> **NOTE:** This mod also has some basic safeguards against trolls because even in the current year, people still think attack helicopter jokes are funny for whatever reason.

Safeguards include:

- Default display limit of 8 pronouns
- Duplicate checking
- Pronoun filtering (`pronouns.csv` contains the most common good faith pronouns. If you feel that more pronouns belong
  here, feel free to send a pull request!)

![a](https://i.imgur.com/AZEl7LA.png)
