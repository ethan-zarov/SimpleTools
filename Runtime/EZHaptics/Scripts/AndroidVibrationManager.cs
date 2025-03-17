using UnityEngine;

namespace SimpleTools.EZCloud
{

    public class AndroidVibrationManager : IVibrationManager
    {
        private readonly AndroidJavaObject _contextObject;

        public AndroidVibrationManager()
        {
            var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            var currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            _contextObject = new AndroidJavaObject("EZHaptics");
            _contextObject.Set("ctx", currentActivity);
        }
        
        
        public void PlayTransient(float intensity, float sharpness)
        {
            // Pass the floats directly to Java
            _contextObject.Call("playHapticTransient", intensity, sharpness);
        }
    }
}
