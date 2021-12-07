using System;
using MelonLoader;
using UnityEngine;

namespace CalibrateConfirm.Components {
	[RegisterTypeInIl2Cpp]
	internal class ButtonWatcher : MonoBehaviour {
		public ButtonWatcher(IntPtr ptr) : base(ptr) { }
		public GameObject reference;
		public GameObject target;

		private void Update() {
			// shit fix for when opening QM while timeout period is active
			if (reference.gameObject.active) target.SetActive(false);
		}
	}
}