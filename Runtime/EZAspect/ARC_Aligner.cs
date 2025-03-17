using System;
using UnityEngine;

namespace Aspect_Ratio
{
    public class ARC_Aligner : AspectRatioChanger
    {
        [Header("Base Settings")]
        [SerializeField] private bool useLocalPosition = true;
        [SerializeField] private bool lateUpdate; //If true, updates after other aspectratio changers.
        [SerializeField] private bool xOnly;
        [SerializeField] private bool yOnly;
        [SerializeField] private ScreenAlignment alignment;
        [SerializeField] private bool useSafeArea;
        

        
        protected override bool LateUpdate => lateUpdate;
        public override void UpdateAspectRatio(Camera camera)
        {
            //Take the viewport position and remap from screen top to bottom to safe area top to bottom.
            float screenPositionY = Remap(alignment.viewportPosition.y, 0, 1, Screen.safeArea.yMin, Screen.safeArea.yMax);
            float viewportPositionY = screenPositionY.ScreenSizeYToViewportSizeY(camera);

            if (!useSafeArea) viewportPositionY = alignment.viewportPosition.y;
            
            Vector2 basePosition = camera.ViewportToWorldPoint(new Vector2(alignment.viewportPosition.x, viewportPositionY));
            var targetPosition = basePosition + alignment.worldPositionOffset;
            if (useLocalPosition)
            {
                if (xOnly) targetPosition.y = transform.localPosition.y;
                if (yOnly) targetPosition.x = transform.localPosition.x;
                transform.localPosition = targetPosition;
            }
            else
            {
                if (xOnly) targetPosition.y = transform.position.y;
                if (yOnly) targetPosition.x = transform.position.x;
                transform.position = targetPosition;
            }
        }


        
        [Serializable]
        public struct ScreenAlignment
        {
            public Vector2 viewportPosition;
            public Vector2 worldPositionOffset;
        }
        
        private float Remap (float value, float from1, float to1, float from2, float to2) {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }



    }
}