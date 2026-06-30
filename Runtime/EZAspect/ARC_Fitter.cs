using UnityEngine;

namespace Aspect_Ratio
{
    public class ARC_Fitter : AspectRatioChanger
    {
        public enum VerticalClamp
        {
            Top,
            Center,
            Bottom
        }

        private bool IsValid => topHook != null && bottomHook != null && componentTop != null && componentBottom != null && resizedComponent != null;

        [SerializeField] private Transform topHook;
        [SerializeField] private Transform bottomHook;
        [Space]
        [SerializeField] private Transform componentTop;
        [SerializeField] private Transform componentBottom;
        [SerializeField] private Transform resizedComponent;
        [Space]
        [SerializeField] private float localXSize;
        [SerializeField] private float maxScale = 999f;
        [SerializeField] private VerticalClamp clampOption;
        [Space]
        [SerializeField] private Transform leftXHook;
        [SerializeField] private Transform rightXHook;
        [SerializeField] private float xPadding;

        [SerializeField] private float editorYOffset;

        private bool _isActive = true;
        private bool _hasBaseline;
        private Vector3 _authoredLocalPosition;
        private Vector3 _authoredLocalScale;
        private float _componentLocalSpan;

#if UNITY_EDITOR
        private void Reset()
        {
            updatePhase = AspectRatioUpdatePhase.Fit;
            updateOrder = 0;
        }
#endif

        public override AspectRatioUpdatePhase Phase => AspectRatioUpdatePhase.Fit;
        public override int Order => updateOrder;

        public void SetActive(bool isActive)
        {
            _isActive = isActive;
            UpdateAspectRatio(Camera.main);
        }

        public void SetXSize(float xSize)
        {
            localXSize = xSize;
            UpdateAspectRatio(Camera.main);
        }

        public void SetHooks(Transform top, Transform bottom)
        {
            topHook = top;
            bottomHook = bottom;
            UpdateAspectRatio(Camera.main);
        }

        public float GetXSize(Camera camera)
        {
            if (!IsValid) return 0f;
            EnsureBaseline();
            RestoreBaseline();

            float uniformScale = CalculateUniformScale(camera);
            resizedComponent.localScale = new Vector3(uniformScale, uniformScale, _authoredLocalScale.z);
            return uniformScale * localXSize;
        }

        public override void UpdateAspectRatio(Camera camera)
        {
            if (!_isActive || !IsValid) return;

            EnsureBaseline();
            RestoreBaseline();

            float uniformScale = CalculateUniformScale(camera);

            if (uniformScale > 80) Debug.Break();

            resizedComponent.localScale = new Vector3(uniformScale, uniformScale, _authoredLocalScale.z);
            ApplyVerticalClamp();
        }

        private void EnsureBaseline()
        {
            if (_hasBaseline) return;

            _authoredLocalPosition = resizedComponent.localPosition;
            _authoredLocalScale = resizedComponent.localScale;
            _componentLocalSpan = MeasureComponentLocalSpan();
            _hasBaseline = true;
        }

        private void RestoreBaseline()
        {
            resizedComponent.localPosition = _authoredLocalPosition;
            resizedComponent.localScale = _authoredLocalScale;
        }

        private float MeasureComponentLocalSpan()
        {
            var topLocal = resizedComponent.InverseTransformPoint(componentTop.position);
            var bottomLocal = resizedComponent.InverseTransformPoint(componentBottom.position);
            return Mathf.Abs(topLocal.y - bottomLocal.y);
        }

        private void ApplyVerticalClamp()
        {
            float midComponent = (componentTop.position.y + componentBottom.position.y) * 0.5f;
            float midHook = (topHook.position.y + bottomHook.position.y) * 0.5f;

            Vector3 worldPos = resizedComponent.position;

            if (clampOption == VerticalClamp.Top)
                worldPos.y += topHook.position.y - componentTop.position.y;
            else if (clampOption == VerticalClamp.Center)
                worldPos.y += midHook - midComponent;
            else if (clampOption == VerticalClamp.Bottom)
                worldPos.y += bottomHook.position.y - componentBottom.position.y;

#if UNITY_EDITOR
            worldPos.y += editorYOffset;
#endif

            resizedComponent.position = worldPos;
        }

        private float CalculateUniformScale(Camera camera)
        {
            float relativeDistance = _componentLocalSpan;
            if (relativeDistance <= 0.0001f)
                relativeDistance = MeasureComponentLocalSpan();

            float targetDistance = Mathf.Abs(topHook.position.y - bottomHook.position.y);
            float targetScaleY = targetDistance / relativeDistance;
            float targetScaleX = GetTargetScaleX(camera);

            float uniformScale = Mathf.Min(targetScaleX, targetScaleY);

            if (leftXHook != null || rightXHook != null)
            {
                float maxWorldWidth = GetMaxWorldWidthX(camera);
                float maxScaleX = maxWorldWidth / localXSize;
                uniformScale = Mathf.Min(uniformScale, maxScaleX);
            }

            return Mathf.Min(uniformScale, maxScale);
        }

        private float GetTargetScaleX(Camera camera)
        {
            if (leftXHook != null && rightXHook != null)
            {
                float hookWidth = Mathf.Abs(rightXHook.position.x - leftXHook.position.x) - xPadding * 2f;
                return Mathf.Max(0f, hookWidth) / localXSize;
            }

            float widthU = 1f.ViewportSizeXToWorldSizeX(camera);
            return widthU / localXSize;
        }

        private float GetMaxWorldWidthX(Camera camera)
        {
            Vector3 sampleWorldPosition = resizedComponent.parent != null
                ? resizedComponent.parent.TransformPoint(_authoredLocalPosition)
                : _authoredLocalPosition;

            float depth = camera.WorldToViewportPoint(sampleWorldPosition).z;
            float centerX = camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, depth)).x;

            float maxWorldWidth;
            if (leftXHook != null && rightXHook != null)
                maxWorldWidth = Mathf.Abs(rightXHook.position.x - leftXHook.position.x);
            else if (rightXHook != null)
                maxWorldWidth = 2f * Mathf.Abs(rightXHook.position.x - centerX);
            else
                maxWorldWidth = 2f * Mathf.Abs(centerX - leftXHook.position.x);

            return Mathf.Max(0f, maxWorldWidth - xPadding * 2f);
        }
    }
}
