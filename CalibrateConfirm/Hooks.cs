﻿using System;
using System.Collections;

namespace CalibrateConfirm
{
    internal class Hooks
    {
        internal static IEnumerator UiManager(Action method)
        {
            while (VRCUiManager.prop_VRCUiManager_0 == null)
                yield return null;
            method();
        }
    }
}