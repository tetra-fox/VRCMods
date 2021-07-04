using MelonLoader;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using VRChatUtilityKit.Ui;

namespace CalibrateConfirm
{
    public class BuildInfo
    {
        public const string Name = "CalibrateConfirm";
        public const string Author = "tetra";
        public const string Version = "2.0.1";
        public const string DownloadLink = "https://github.com/tetra-fox/VRCMods";
    }

    public class Mod : MelonMod
    {
        private SingleButton _calibrateConfirmButton;
        private GameObject _calibrateButton;

        public override void OnApplicationStart()
        {
            VRChatUtilityKit.Utilities.VRCUtils.OnUiManagerInit += Init;
        }

        private void Init()
        {
            if (!XRDevice.isPresent) return;
            MelonLogger.Msg("Initializing...");

            _calibrateButton = GameObject.Find("UserInterface/QuickMenu/ShortcutMenu/CalibrateButton");

            object timeout = new();

            _calibrateConfirmButton = new SingleButton(GameObject.Find("UserInterface/QuickMenu/ShortcutMenu"),
                new Vector3(3, 1, 0), "Confirm?", null, "Are you sure you want to calibrate?",
                "CalibrateConfirmBtn");

            _calibrateConfirmButton.gameObject.SetActive(false);

            _calibrateConfirmButton.ButtonComponent.onClick = _calibrateButton.GetComponent<Button>().onClick;

            _calibrateConfirmButton.ButtonComponent.onClick.AddListener((Action)delegate
           {
               _calibrateConfirmButton.gameObject.SetActive(false);
               _calibrateButton.SetActive(true);
               MelonCoroutines.Stop(timeout);
           });

            _calibrateButton.GetComponent<Button>().onClick = new Button.ButtonClickedEvent();

            _calibrateButton.GetComponent<Button>().onClick.AddListener((Action)delegate
            {
                _calibrateConfirmButton.gameObject.SetActive(true);
                _calibrateButton.SetActive(false);
                timeout = MelonCoroutines.Start(Timeout(5));
            });

            // shit fix for when opening QM while timeout period is active
            UiManager.OnQuickMenuOpened += delegate { if (_calibrateConfirmButton.gameObject.active) _calibrateButton.SetActive(false); };

            MelonLogger.Msg("Initialized!");
        }

        private IEnumerator Timeout(int length)
        {
            int timeout = length;

            while (timeout > 0)
            {
                _calibrateConfirmButton.TextComponent.text = "Confirm?\n" + timeout;
                timeout--;
                yield return new WaitForSeconds(1);
            }

            _calibrateConfirmButton.gameObject.SetActive(false);
            _calibrateButton.SetActive(true);
        }
    }
}