using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_IOS
using UnityEngine.iOS;
#endif

namespace SimpleTools.EZCloud
{
    
    public static class Haptics
    {
        private static bool _isInitialized;
        private static bool _hapticsEnabled = true;
        private static IVibrationManager _vibrationManager;

        private static float Multiplier => 1f;
        
        private static void CheckForInitialization()
        {
            if (_isInitialized) return;
            
            #if UNITY_IOS && !UNITY_EDITOR
                _vibrationManager = new IOSVibrationManager(); 
            #elif UNITY_ANDROID && !UNITY_EDITOR
                _vibrationManager = new AndroidVibrationManager();
            #else
                _vibrationManager = new DefaultVibrationManager(); 
            #endif

            
            _isInitialized = true;
        }

        public static void SetEnabled(bool hapticsEnabled)
        {
            _hapticsEnabled = hapticsEnabled;
        }


        
        public static void PlayTransient(float intensity, float sharpness)
        {
            if (!_hapticsEnabled || Multiplier < .01f) return;

            intensity *= Multiplier;
            

#if UNITY_IOS && !UNITY_EDITOR
            // On iOS <= 12, we skip haptics (same logic as existing code).
            Version currentVersion = new Version(UnityEngine.iOS.Device.systemVersion);
            Version ios13 = new Version("13.0");
            if (currentVersion < ios13) return;
#endif
            
            CheckForInitialization();
            _vibrationManager.PlayTransient(intensity, sharpness);
        }
        
        
    }

    public enum HapticSetting
    {
        Soft,
        Default,
        Rigid
    }

}
