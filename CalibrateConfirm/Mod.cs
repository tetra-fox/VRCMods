using CalibrateConfirm.Components;
using MelonLoader;
using System;
using System.Collections;
using UnhollowerRuntimeLib;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

namespace CalibrateConfirm
{
    public class BuildInfo
    {
        public const string Name = "CalibrateConfirm";
        public const string Author = "tetra";
        public const string Version = "1.0.1";
        public const string DownloadLink = "https://github.com/tetra-fox/CalibrateConfirm/releases/download/1.0.1/CalibrateConfirm.dll";
    }

    public class Mod : MelonMod
    {
        public GameObject CalibrateConfirmButton;
        public GameObject CalibrateButton;

        public override void OnApplicationStart()
        {
            ClassInjector.RegisterTypeInIl2Cpp<EnableDisableListener>();

            if (Utils.HasMod("UI Expansion Kit"))
            {
                MelonLogger.Msg("UIX found. Using UIX hook.");
                UIExpansionKit.API.ExpansionKitApi.OnUiManagerInit += Init;
                return;
            }
            MelonLogger.Msg("UIX not found. Using fallback hook.");
            MelonCoroutines.Start(Hooks.UiManager(Init));
        }

        private void Init()
        {
            if (!XRDevice.isPresent) return;
            MelonLogger.Msg("Initializing...");

            CalibrateButton = GameObject.Find("UserInterface/QuickMenu/ShortcutMenu/CalibrateButton");

            CalibrateConfirmButton = UI.Button.Create(GameObject.Find("UserInterface/QuickMenu/ShortcutMenu/"), "Confirm?",
                "Are you sure you want to calibrate?", "CalibrateConfirmBtn", new Vector3(3, 1, 0),
                CalibrateButton.GetComponent<Button>().onClick);

            CalibrateConfirmButton.SetActive(false);

            CalibrateButton.GetComponent<Button>().onClick = new Button.ButtonClickedEvent();

            object timeout = new object();

            CalibrateButton.GetComponent<Button>().onClick.AddListener((Action)delegate
            {
                CalibrateConfirmButton.SetActive(true);
                CalibrateButton.SetActive(false);
                timeout = MelonCoroutines.Start(Timeout(5));
            });

            CalibrateConfirmButton.GetComponent<Button>().onClick.AddListener((Action)delegate
            {
                CalibrateConfirmButton.SetActive(false);
                CalibrateButton.SetActive(true);
                MelonCoroutines.Stop(timeout);
            });

            // shit fix for when opening QM while timeout period is active
            CalibrateButton.AddComponent<EnableDisableListener>().OnEnabled += delegate { if (CalibrateConfirmButton.active) CalibrateButton.SetActive(false); };

            MelonLogger.Msg("Initialized!");
        }

        private IEnumerator Timeout(int length)
        {
            int timeout = length;

            while (timeout > 0)
            {
                CalibrateConfirmButton.GetComponentInChildren<Text>().text = "Confirm?\n" + timeout;
                timeout--;
                yield return new WaitForSeconds(1);
            }

            CalibrateConfirmButton.SetActive(false);
            CalibrateButton.SetActive(true);
        }
    }
}