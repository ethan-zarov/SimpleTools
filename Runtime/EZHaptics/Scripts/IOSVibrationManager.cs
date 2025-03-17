namespace SimpleTools.EZCloud
{
    public class IOSVibrationManager : IVibrationManager
    {

        
        public void PlayTransient(float intensity, float sharpness)
        {
            IOSFrameworkBridge.PlayTransientHaptic(intensity, sharpness);
        }
    }
}