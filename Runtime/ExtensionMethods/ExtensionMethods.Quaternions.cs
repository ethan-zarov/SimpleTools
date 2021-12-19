using UnityEngine;
using System.Collections.Generic;

namespace EthanZarov.SimpleTools
{
    public static partial class ExtensionMethods
    {
        

        public static Quaternion Lerp_DeltaTime(this Quaternion value, Quaternion target, float t)
        {
            return Quaternion.Lerp(value, target, 1 - Mathf.Pow(t / 1000000, Time.deltaTime));
        }

        public static Quaternion Lerp_UnscaledDeltaTime(this Quaternion value, Quaternion target, float t)
        {
            return Quaternion.Lerp(value, target, 1 - Mathf.Pow(t / 1000000, Time.unscaledDeltaTime));
        }
        
                        
        public static Quaternion Lerp_FixedDeltaTime(this Quaternion value, Quaternion target, float t)
        {
            return Quaternion.Lerp(value, target, 1 - Mathf.Pow(t / 1000000, Time.fixedDeltaTime));
        }
        
        public static Quaternion Lerp_FixedUnscaledDeltaTime(this Quaternion value, Quaternion target, float t)
        {
            return Quaternion.Lerp(value, target, 1 - Mathf.Pow(t / 1000000, Time.fixedUnscaledDeltaTime));
        }

        


        public static Quaternion FaceDirection(this Vector2 v)
        {
            return v.FaceDirection(0);
        }

        public static Quaternion FaceDirection(this Vector2 v, float degreeOffset)
        {
            if (v != Vector2.zero)
            {
                float angle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg + degreeOffset;
                return Quaternion.AngleAxis(angle, Vector3.forward);
            }
            else return Quaternion.identity;
        }
    }

}