using System;
using UnityEngine;

namespace Aspect_Ratio
{
    public abstract class AspectRatioChanger : MonoBehaviour
    {
        private void Awake()
        {
            AspectRatioManager.Register(this, LateUpdate);
        }

        private void Start()
        {
            UpdateAspectRatio(Camera.main);
        }


        protected abstract bool LateUpdate { get; }
        public abstract void UpdateAspectRatio(Camera camera);
    }
}