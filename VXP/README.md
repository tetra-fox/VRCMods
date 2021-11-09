## VXP - Vocal eXpression Parameter
VXP exposes an additional avatar parameter that avatars can leverage to *~dynamically~* control facial expressions based on the tone of the speaker's voice, through the **magical power of 𝓐𝓻𝓽𝓲𝓯𝓲𝓬𝓲𝓪𝓵 𝓘𝓷𝓽𝓮𝓵𝓵𝓲𝓰𝓮𝓷𝓬𝓮!**

## Configuration
|Name|Description|Default Value|Type|
|-|-|-|-|
|Enabled|Enable VXP|`true`|`bool`|
|Gesture Behavior|What should control your facial expressions?|`VPX Only`|`string`|
|Update Interval|How often VXP should analyze speech (in seconds)|`1`|`int`|

## Unity Setup for Avatar Creators
Before proceeding, be sure that you have a solid understanding of the Unity editor (more specifically, animators and animation controllers). I won't go into detail here, but here are some resources if you'd like to learn:
- [Introduction to 3D Animation Systems](https://learn.unity.com/course/introduction-to-3d-animation-systems)
- [Animator Controllers](https://learn.unity.com/tutorial/animator-controllers-2019-3)
- [Controlling Animation](https://learn.unity.com/tutorial/controlling-animation)

When creating your animator conditions, use the `VXP_Expression` parameter to control animator states. Also remember to add `VXP_Expression` to your expression parameters!

The expression values are as follows:
|Expression|Parameter Value (int)|
|-|-|
|None (don't use this)|0|
|Neutral|1|
|Calm|2|
|Happy|3|
|Sad|4|
|Angry|5|
|Fearful|6|
|Disgust|7|
|Surprised|8|

Additionally, VXP exposes another parameter called `VXP_Confidence`. This is just how sure of the tone VXP is. This is a float value (0-1). Use this however you please.

## FAQ
**Q:** Are trying to sell/farm/steal my voice data?

**A:** No. All the vocal processing is done locally on your machine so your voice data never leaves your computer. (Except for when it's sent to other players, of course.)

**Q:** Why is the .dll so big?

**A:** The trained voice model is stored within the .dll!

**Q:** Why don't my blendshapes work?

**A:** Unfortunately, blendshape animations must be disabled while VXP is enabled because there is no easy way to identify which blendshapes are used specifically for facial expressions. Other animation types such as bone and transform animations will still play.

**Q:** Do I need to use *all* of the expression values?

**A:** No, but it's highly recommended that you do, just to avoid any weirdness when VXP identifies an expression that you haven't assigned any animator state to.

**Q:** Can you set my avatar up for me?

**A: [No.](#Unity-Setup-for-Avatar-Creators)**