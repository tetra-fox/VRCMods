using System;
using MelonLoader;
using UnityEngine;

namespace ProPlates.Components
{
	[RegisterTypeInIl2Cpp]
	public class OpacityListener : MonoBehaviour
	{
		public OpacityListener(IntPtr ptr) : base(ptr) { }
		public ImageThreeSlice reference;
		public ImageThreeSlice target;

		private void Update()
		{
			if (target.color == reference.color) return;
			target.color = reference.color;
		}
	}
}