using UnityEngine;
using System.Collections.Generic;
using EthanZarov.SimpleTools;

namespace EthanZarov.EZInput
{
    /// - Handles registering touch on any buttons within the scene - ///
    public static partial class EZInput
    {
        /// <summary>
        /// Detects whether a given touch position hits this collider.
        /// </summary>
        /// <param name="collider">Collider to detect hits on.</param>
        /// <param name="touchPos">Touch position from DetectTouches().</param>
        /// <returns></returns>
        public static bool EZI_Touched(this Collider collider, Vector3 touchPos)
        {
            Collider[] colliders = Physics.OverlapSphere(touchPos, .1f);
            if (colliders.Length > 0)
            {
                foreach (var _coll in colliders)
                {
                    if (_coll == collider)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// Detects whether a given touch position hits this collider.
        /// </summary>
        /// <param name="collider">Collider to detect hits on.</param>
        /// <param name="touchPos">Touch position from DetectTouches().</param>
        /// <param name="layerMask">The layer the BoxCollider is on.</param>
        /// <returns></returns>
        public static bool EZI_Touched(this Collider collider, Vector3 touchPos, LayerMask layerMask)
        {
            Collider[] colliders = Physics.OverlapSphere(touchPos, .1f, layerMask);
            if (colliders.Length > 0)
            {
                foreach (var _coll in colliders)
                {
                    if (_coll == collider)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

    }
}
