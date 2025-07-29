
using UnityEngine;

namespace Aspect_Ratio
{
    public static class AspectRatioExtensionMethods
    {
        public static float WorldSizeYToViewportSizeY(this float worldSizeY, Camera cam)
        {
            return (cam.WorldToViewportPoint(Vector3.up * worldSizeY) - cam.WorldToViewportPoint(Vector2.zero)).y;
        }
        
        public static float WorldSizeXToViewportSizeX(this float worldSizeX, Camera cam)
        {
            return (cam.WorldToViewportPoint(Vector3.right * worldSizeX) - cam.WorldToViewportPoint(Vector2.zero)).x;
        }

        public static float ViewportSizeYToWorldSizeY(this float viewportSizeY, Camera cam)
        {
            return (cam.ViewportToWorldPoint(Vector3.up * viewportSizeY) - cam.ViewportToWorldPoint(Vector3.zero)).y;
        }
        
        public static float ViewportSizeXToWorldSizeX(this float viewportSizeX, Camera cam)
        {
            return (cam.ViewportToWorldPoint(Vector3.right * viewportSizeX) - cam.ViewportToWorldPoint(Vector3.zero)).x;
        }

        public static float ScreenSizeYToViewportSizeY(this float screenSizeY, Camera cam)
        {
            return (cam.ScreenToViewportPoint(Vector3.up * screenSizeY) - cam.ScreenToViewportPoint(Vector3.zero)).y;
        }
        
        public static float ScreenSizeYToWorldSizeY(this float screenSizeY, Camera cam)
        {
            return (cam.ScreenToWorldPoint(Vector3.up * screenSizeY) - cam.ScreenToWorldPoint(Vector3.zero)).y;
        }
        
        
        public static float ViewportSizeYToScreenSizeY(this float viewportSizeY, Camera cam)
        {
            return (cam.ViewportToScreenPoint(Vector3.up * viewportSizeY) - cam.ViewportToScreenPoint(Vector3.zero)).y;
        }
    }
}