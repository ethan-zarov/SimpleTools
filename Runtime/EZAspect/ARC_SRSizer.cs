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
        [SerializeField] private bool lateUpdate;

        [SerializeField] private bool factorScale;
        
        protected override bool LateUpdate => lateUpdate;
        public override void UpdateAspectRatio(Camera camera)
        {
            float width = xPercent.ViewportSizeXToWorldSizeX(camera);

            float targetYSize = adjustYSize ? this.ySize : sr.size.y;
            
            if (factorScale && transform.lossyScale.x > .01f) width /= transform.lossyScale.x;
            
            sr.size = new Vector2(width, targetYSize);
        }
    }
}