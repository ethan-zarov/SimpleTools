using UnityEngine;
using System.Collections.Generic;

namespace EthanZarov.SimpleTools
{
    public static partial class ExtensionMethods
    {
        
        public static Vector2 RadToVec2(this float value)
        {
            return new Vector2(Mathf.Cos(value), Mathf.Sin(value));
        }
        public static Vector2 RadToVec2(this float value, float length)
        {
            return RadToVec2(value) * length;
        }
        public static Vector2 DegToVec2(this float value)
        {
            return RadToVec2(value * Mathf.Deg2Rad);
        }
        public static Vector2 DegToVec2(this float value, float length)
        {
            return RadToVec2(value * Mathf.Deg2Rad) * length;
        }

        public static Vector2 Lerp_DeltaTime(this Vector2 value, Vector2 target, float t)
        {
            return Vector2.Lerp(value, target, 1 - Mathf.Pow(t / 1000000, Time.deltaTime));
        }
        
        public static Vector2 Lerp_UnscaledDeltaTime(this Vector2 value, Vector2 target, float t)
        {
            return Vector2.Lerp(value, target, 1 - Mathf.Pow(t / 1000000, Time.unscaledDeltaTime));
        }
        
        public static Vector3 Lerp_DeltaTime(this Vector3 value, Vector3 target, float t)
        {
            return Vector3.Lerp(value, target, 1 - Mathf.Pow(t / 1000000, Time.deltaTime));
        }
        
        public static Vector3 Lerp_UnscaledDeltaTime(this Vector3 value, Vector3 target, float t)
        {
            return Vector3.Lerp(value, target, 1 - Mathf.Pow(t / 1000000, Time.unscaledDeltaTime));
        }
        
        #region Vector Local / World Space
        /// <summary>
        /// Gets the local offset of an object relative to its global position.
        /// </summary>
        /// <param name="obj">Object to get the local offset of.</param>
        /// <returns>Vector2 offset of the local position.</returns>
        public static Vector2 GetLocalOffset(this Transform obj)
        {
            return obj.position - obj.localPosition;
        }
        /// <summary>
        /// Get a global position relative to an objects local offset.
        /// </summary>
        /// <param name="localPos">The target local position.</param>
        /// <param name="localOffset">The target object's local offset to use.</param>
        /// <returns>The outputted global position.</returns>
        public static Vector2 LocalToGlobalPos(this Vector3 localPos, Vector2 localOffset)
        {
            return (Vector2)localPos + localOffset;
        }
        /// <summary>
        /// Get the local position based on an object's local offset from a global position.
        /// </summary>
        /// <param name="globalPos">The input global position.</param>
        /// <param name="localOffset">The target object's local offset to use.</param>
        /// <returns>The outputted local position.</returns>
        public static Vector2 GlobalToLocalPos(this Vector3 globalPos, Vector2 localOffset)
        {
            return (Vector2)globalPos - localOffset;
        }
        #endregion
        
        public static Vector2 Normal(this Vector2 v)
        {
            return new Vector2(-v.y, v.x);
        }

        public static Vector2 Rotate(this Vector2 v, float degrees)
        {
            float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
            float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

            float tx = v.x;
            float ty = v.y;
            v.x = (cos * tx) - (sin * ty);
            v.y = (sin * tx) + (cos * ty);
            return v;
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