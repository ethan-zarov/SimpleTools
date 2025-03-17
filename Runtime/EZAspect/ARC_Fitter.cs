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

        [SerializeField] private bool lateUpdate;
        [SerializeField] private Transform topHook;
        [SerializeField] private Transform bottomHook;
        [Space]
        [SerializeField] private Transform componentTop;
        [SerializeField] private Transform componentBottom;
        [SerializeField] private Transform resizedComponent;
        [Space]
        [SerializeField] private float localXSize;
        [SerializeField] private VerticalClamp clampOption; // New variable for clamping option
        
        public void SetXSize(float xSize)
        {
            localXSize = xSize;
            UpdateAspectRatio(Camera.main);
        }

        protected override bool LateUpdate => lateUpdate;

        public override void UpdateAspectRatio(Camera camera)
        {
            // Step 1: Calculate the target X scale  
            float widthU = 1f.ViewportSizeXToWorldSizeX(camera); // Your method for converting viewport size to world size  
            float targetScaleX = widthU / localXSize;  
  
            // Step 2: Calculate the Y scale to fit between the top and bottom hooks  
            var topLocal = resizedComponent.InverseTransformPoint(componentTop.position);  
            var bottomLocal = resizedComponent.InverseTransformPoint(componentBottom.position);  
  
            float relativeDistance = Mathf.Abs(topLocal.y - bottomLocal.y);  
            float targetDistance = Mathf.Abs(topHook.position.y - bottomHook.position.y);  
  
            float targetScaleY = targetDistance / relativeDistance;  
  
            // Step 3: Use the minimum of the X and Y scales for uniform scaling  
            float uniformScale = Mathf.Min(targetScaleX, targetScaleY);  
  
            // Step 4: Adjust the scale of the resized component  
            resizedComponent.localScale = new Vector3(uniformScale, uniformScale, resizedComponent.localScale.z);  
  
            // Step 5: Calculate the middle points for component and hook  
            float midComponent = (componentTop.position.y + componentBottom.position.y) / 2f;  
            float midHook = (topHook.position.y + bottomHook.position.y) / 2f;

            // Step 6: Adjust the Y position based on clamping option
            Vector3 newPosition = resizedComponent.localPosition;
            
            if (clampOption == VerticalClamp.Top)
            {
                // Clamp to top: Align componentTop with topHook, ensure resizedComponent fits entirely
                float offset = topHook.position.y - componentTop.position.y;
                newPosition.y += offset;
            }
            else if (clampOption == VerticalClamp.Center)
            {
                // Center it between topHook and bottomHook
                newPosition.y = midHook - (midComponent - resizedComponent.position.y);
            }
            else if (clampOption == VerticalClamp.Bottom)
            {
                // Clamp to bottom: Align componentBottom with bottomHook, ensure resizedComponent fits entirely
                float offset = bottomHook.position.y - componentBottom.position.y;
                newPosition.y += offset;
            }

            resizedComponent.localPosition = newPosition;
        }
    }
}
