using System;
using UnityEngine;

namespace Aspect_Ratio
{
    public class ARC_Aligner : AspectRatioChanger
    {
        [Header("Base Settings")]
        [SerializeField] private bool xOnly;
        [SerializeField] private bool yOnly;
        [SerializeField] private ScreenAlignment alignment;
        [SerializeField] private bool useSafeArea;

#if UNITY_EDITOR
        private void Reset()
        {
            updatePhase = AspectRatioUpdatePhase.AlignPost;
            updateOrder = 0;
        }
#endif

        public override void UpdateAspectRatio(Camera camera)
        {
            float screenPositionY = Remap(alignment.viewportPosition.y, 0, 1, Screen.safeArea.yMin, Screen.safeArea.yMax);
            float viewportPositionY = screenPositionY.ScreenSizeYToViewportSizeY(camera);

            if (!useSafeArea) viewportPositionY = alignment.viewportPosition.y;
            else
            {
#if UNITY_EDITOR
                viewportPositionY -= .07f;
#endif
            }

            Vector3 viewportPoint = new Vector3(
                alignment.viewportPosition.x,
                viewportPositionY,
                camera.WorldToViewportPoint(transform.position).z);
            Vector3 worldTarget = camera.ViewportToWorldPoint(viewportPoint);
            worldTarget += (Vector3)alignment.worldPositionOffset;
            worldTarget.z = transform.position.z;

            if (xOnly) worldTarget.y = transform.position.y;
            if (yOnly) worldTarget.x = transform.position.x;

            transform.position = worldTarget;
        }

        [Serializable]
        public struct ScreenAlignment
        {
            public Vector2 viewportPosition;
            public Vector2 worldPositionOffset;
        }

        private float Remap(float value, float from1, float to1, float from2, float to2)
        {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }
    }
}
