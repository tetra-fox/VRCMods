using System;
using UnityEngine;

namespace ProPlates.Components
{
    public class OpacityListener : MonoBehaviour
    {
        public OpacityListener(IntPtr ptr) : base(ptr) {}
        public ImageThreeSlice reference;
        public ImageThreeSlice target;
        
        private void Update()
        {
            if (target.color == reference.color) return;
            target.color = reference.color;
        }
    }
}