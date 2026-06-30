using Sirenix.OdinInspector;
using UnityEngine;

namespace Aspect_Ratio
{
    public class ARC_SRSizer : AspectRatioChanger
    {
        [SerializeField] private SpriteRenderer sr;
        [SerializeField] private float xPercent;
        [SerializeField] private bool adjustYSize;

        [SerializeField, ShowIf("adjustYSize")] private float ySize;
        [Space]
        [SerializeField] private bool factorScale;

#if UNITY_EDITOR
        private void Reset()
        {
            updatePhase = AspectRatioUpdatePhase.Size;
            updateOrder = 0;
        }
#endif

        public override void UpdateAspectRatio(Camera camera)
        {
            float width = xPercent.ViewportSizeXToWorldSizeX(camera);

            float targetYSize = adjustYSize ? this.ySize : sr.size.y;

            if (factorScale && transform.lossyScale.x > .01f) width /= transform.lossyScale.x;

            sr.size = new Vector2(width, targetYSize);
        }
    }
}
