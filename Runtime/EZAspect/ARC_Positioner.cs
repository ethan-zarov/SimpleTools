using UnityEngine;

namespace Aspect_Ratio
{
    public class ARC_Positioner : AspectRatioChanger
    {
        [Header("Base Settings")] [SerializeField]
        private bool lateUpdate;
        [SerializeField] private bool useLocalPosition = true;
        [Space]
        [SerializeField] private bool useX;
        [SerializeField] private AnimationCurve xCurve;
        [SerializeField] private Vector2 clampX = new Vector2(-999, 999);
        [Space]
        
        [SerializeField] private bool useY;
        [SerializeField] private AnimationCurve yCurve;
        [SerializeField] private Vector2 clampY = new Vector2(-999, 999);
        
        private float _baseX;
        private float _baseY;
        
        private void Start()
        {

            if (useLocalPosition)
            {
                _baseX = transform.localPosition.x;
                _baseY = transform.localPosition.y;
            }
            else
            {
                _baseX = transform.position.x;
                _baseY = transform.position.y;
            }
        }

        protected override bool LateUpdate => lateUpdate;
        public override void UpdateAspectRatio(Camera camera)
        {
            var aspect = camera.aspect;
            float x = _baseX;
            float y = _baseY;

            if (useX) x= Mathf.Clamp(xCurve.Evaluate(aspect), clampX.x, clampX.y);
            if (useY) y= Mathf.Clamp(yCurve.Evaluate(aspect), clampY.x, clampY.y);

            if (useLocalPosition)
            {
                transform.localPosition = new Vector3(x, y);
            }
            else transform.position = new Vector3(x, y);


        }
    }
}