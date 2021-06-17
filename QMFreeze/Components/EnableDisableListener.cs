using System;
using UnhollowerBaseLib.Attributes;
using UnityEngine;

namespace QMFreeze.Components
{
    public class EnableDisableListener : MonoBehaviour
    {
#nullable enable

        [method: HideFromIl2Cpp] public event Action? OnEnabled;

        [method: HideFromIl2Cpp] public event Action? OnDisabled;

#nullable disable

        public EnableDisableListener(IntPtr obj0) : base(obj0)
        {
        }

        private void OnEnable()
        {
            OnEnabled?.Invoke();
        }

        private void OnDisable()
        {
            OnDisabled?.Invoke();
        }
    }
}