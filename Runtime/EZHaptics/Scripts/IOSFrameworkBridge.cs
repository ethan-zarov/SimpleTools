using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AOT;
using System.Runtime.InteropServices;

namespace SimpleTools.EZCloud
{
        
    public class IOSFrameworkBridge
    {
        #if UNITY_IOS
        [DllImport("__Internal")]
        private static extern void performTransientHaptic(float intensity, float sharpness);
        #endif
        
        public static void PlayTransientHaptic(float intensity, float sharpness)
        {
#if UNITY_IOS
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                performTransientHaptic(intensity, sharpness);
            }
#endif
        }
    }


}