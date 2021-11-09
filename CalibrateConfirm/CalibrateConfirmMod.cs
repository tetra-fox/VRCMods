using MelonLoader;
using System;
using System.Collections;
using System.IO;
using System.Reflection;
using UnhollowerRuntimeLib;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using VRChatUtilityKit.Ui;

namespace CalibrateConfirm
{
    internal class BuildInfo
    {
        public const string Name = "CalibrateConfirm";
        public const string Author = "tetra";
        public const string Version = "3.0.0";
        public const string DownloadLink = "https://github.com/tetra-fox/VRCMods";
    }

    public class Mod : MelonMod
    {
        private SingleButton _calibrateConfirmButton;
        private GameObject _calibrateButton;
        private bool _initialized;

        public override void OnApplicationStart()
        {
            // can't gameobject.find inactive objects so wait until we open quick menu to init
            UiManager.OnQuickMenuOpened += Init;
        }

        private void Init()
        {
            // if (!XRDevice.isPresent || initialized) return;
            if (_initialized) return;
            MelonLogger.Msg("Initializing...");
            _calibrateButton = GameObject.Find("UserInterface/Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_Dashboard/ScrollRect/Viewport/VerticalLayoutGroup/Buttons_QuickActions/SitStandCalibrateButton/Button_CalibrateFBT");

            object timeout = new();

            AssetBundle assetBundle;
            using var stream = Assembly.GetExecutingAssembly()
                .GetManifestResourceStream("CalibrateConfirm.calibrateconfirm.assetbundle");
                
            using (var tempStream = new MemoryStream((int)stream.Length))
            {
                stream.CopyTo(tempStream);

                assetBundle = AssetBundle.LoadFromMemory_Internal(tempStream.ToArray(), 0);
                assetBundle.hideFlags |= HideFlags.DontUnloadUnusedAsset;
            }
            
            MelonLogger.Msg("a");
            
            Sprite calibrateSprite = assetBundle.LoadAsset_Internal("ConfirmFBT", Il2CppType.Of<Sprite>()).Cast<Sprite>();

            _calibrateConfirmButton = new SingleButton(() =>
                {
                    _calibrateButton.GetComponent<Button>().onClick.Invoke(); 
                    _calibrateConfirmButton.gameObject.SetActive(false);
                    _calibrateButton.SetActive(true);
                    MelonCoroutines.Stop(timeout);
                },
                calibrateSprite, "Confirm?", "Button_ConfirmFBT", "Are you sure you want to calibrate?");
            
            MelonLogger.Msg("b");
            var sitStandGroup = GameObject.Find("UserInterface/Canvas_QuickMenu(Clone)/Container/Window/QMParent/Menu_Dashboard/ScrollRect/Viewport/VerticalLayoutGroup/Buttons_QuickActions/SitStandCalibrateButton");

            UiManager.AddButtonToExistingGroup(sitStandGroup, _calibrateConfirmButton);

            MelonLogger.Msg("d");
            _calibrateConfirmButton.gameObject.SetActive(false);
            MelonLogger.Msg("f");
            
            MelonLogger.Msg("g");

            _calibrateButton.GetComponent<Button>().onClick = new Button.ButtonClickedEvent();

            
            MelonLogger.Msg("h");
            _calibrateButton.GetComponent<Button>().onClick.AddListener((Action)delegate
            {
                _calibrateConfirmButton.gameObject.SetActive(true);
                _calibrateButton.SetActive(false);
                timeout = MelonCoroutines.Start(Timeout(5));
            });
            
            MelonLogger.Msg("e");

            // shit fix for when opening QM while timeout period is active
            UiManager.OnQuickMenuOpened += delegate { if (_calibrateConfirmButton.gameObject.active) _calibrateButton.SetActive(false); };

            _initialized = true;
            MelonLogger.Msg("Initialized!");
        }

        private IEnumerator Timeout(int length)
        {
            int timeout = length;

            while (timeout > 0)
            {
                _calibrateConfirmButton.Text = "Confirm?\n" + timeout;
                timeout--;
                yield return new WaitForSeconds(1);
            }

            _calibrateConfirmButton.gameObject.SetActive(false);
            _calibrateButton.SetActive(true);
        }
    }
}