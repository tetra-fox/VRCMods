using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using MelonLoader;
using ParamLib;
using UnityEngine;
using VRC.Core;
using VRC.SDK3.Avatars.ScriptableObjects;
using VXP.Components;
using UnhollowerRuntimeLib;
using UnityEngine.Playables;
using VRC.SDK3.Avatars.Components;
using Object = UnityEngine.Object;


namespace VXP
{
    public static class BuildInfo
    {
        public const string Name = "VXP";
        public const string Author = "tetra & tjhorner";
        public const string Version = "1.0.0";
        public const string DownloadLink = "https://github.com/tetra-fox/VRCMods";
    }

    public class Mod : MelonMod
    {
        private static GameObject ParamDriver;
        
        public static Action<int, float> OnParamsUpdated = (expression, confidence) => { };
        
        public override void OnApplicationStart()
        {
            MelonLogger.Msg("Registering settings...");
            Settings.Register();
            Settings.OnConfigChanged += OnSettingsChanged;
            
            VRChatUtilityKit.Utilities.VRCUtils.OnUiManagerInit += Init;
        }
        
        private static void Init()
        {
            if (Settings.Enabled.Value)
            {
                ParamDriver = CreateParamDriver();
                VRChatUtilityKit.Utilities.NetworkEvents.OnAvatarInstantiated += InitComponent;
                MelonLogger.Msg("Hooked OnAvatarInstantiated");
            }

            MelonLogger.Msg("Initialized!");
        }

        private static GameObject CreateParamDriver()
        {
            GameObject VXPDriver = new("VXPDriver");
            Object.DontDestroyOnLoad(VXPDriver);
            VXPDriver.AddComponent<ParameterDriver>();

            return VXPDriver;
        }

        private static void OnSettingsChanged()
        {
            VRChatUtilityKit.Utilities.NetworkEvents.OnAvatarInstantiated -= InitComponent;
            
            switch (Settings.Enabled.Value)
            {
                case false:
                {
                    if (ParamDriver != null) ParamDriver.SetActive(false);
                    EnableGestureControllerBlendshapeAnimations(true);
                    MelonLogger.Msg("Disabled VXP");
                    break;
                }
                case true:
                    VRChatUtilityKit.Utilities.NetworkEvents.OnAvatarInstantiated += InitComponent;

                    if (ParamDriver == null)
                    {
                        ParamDriver = CreateParamDriver();
                    }
                    else
                    {
                        ParamDriver.SetActive(true);
                    }

                    switch (Settings.GestureBehavior.Value)
                    {
                        case Settings.GestureBehaviors.HandsOnly:
                            Settings.Enabled.Value = false;
                            break;
                        case Settings.GestureBehaviors.VXPOnly:
                            EnableGestureControllerBlendshapeAnimations(false);
                            break;
                        case Settings.GestureBehaviors.HandsAndVXP:
                            // TODO: idk bruh
                            EnableGestureControllerBlendshapeAnimations(true);
                            break;
                        default:
                            MelonLogger.Msg("what the hell");
                            throw new ArgumentOutOfRangeException();
                    }
                    MelonLogger.Msg("Enabled VXP");
                    break;
            }
        }

        private static void InitComponent(VRCAvatarManager arg1, ApiAvatar arg2, GameObject arg3)
        {
            MelonLogger.Msg("Initialized parameters to new avatar");
            ParamDriver.SetActive(false);
            ParamDriver.SetActive(true);    
        }

        private static void EnableGestureControllerBlendshapeAnimations(bool enabled)
        {
            HandGestureController handGestureController = VRC.Player.prop_Player_0._vrcplayer
                .field_Private_VRC_AnimationController_0.field_Private_HandGestureController_0;
            
            handGestureController.enabled = enabled;
        }

        private static IEnumerator BehaviorCheck(HandGestureController gestureController) {
            switch (Settings.GestureBehavior.Value)
            {
                case Settings.GestureBehaviors.HandsOnly:
                    // TODO: ignore VPX
                    gestureController.enabled = true;
                    break;
                case Settings.GestureBehaviors.VXPOnly:
                    gestureController.enabled = false;
                    break;
                case Settings.GestureBehaviors.HandsAndVXP:
                    // TODO: idk bruh
                    gestureController.enabled = true;
                    break;
                default:
                    MelonLogger.Msg("what the hell");
                    Settings.GestureBehavior.Value = Settings.GestureBehaviors.VXPOnly;
                    throw new ArgumentOutOfRangeException();
            }
            yield return new WaitForEndOfFrame();
        }
    }

    internal enum Expressions
    {
        None = 0,
        Neutral = 1,
        Calm = 2,
        Happy= 3,
        Sad= 4,
        Angry= 5,
        Fearful= 6,
        Disgust= 7,
        Surprised= 8
    }
}