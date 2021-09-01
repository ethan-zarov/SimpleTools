using UnityEngine;
using System.Collections.Generic;
using EthanZarov.SimpleTools;

namespace EthanZarov.SimpleTools.EasyInput
{
    /// - Handles registering touch on any buttons within the scene - ///
    public static partial class EasyInput
    {
        /// <summary>
        /// Detects touches, and whether it hits a collider. Requires InputConstants.cs running in the current scene.
        /// </summary>
        /// <param name="collider">Collider to detect hits on.</param>
        /// <returns></returns>
        public static bool TouchedHitbox(this BoxCollider collider)
        {
            if (InputConstants.colliders.Length > 0)
            {
                foreach (var _coll in InputConstants.colliders)
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
        /// Detects whether a given touch position hits the target button collider.
        /// </summary>
        /// <param name="collider">Collider to detect hits on.</param>
        /// <param name="touchPos">Touch position from DetectTouches().</param>
        /// <returns></returns>
        public static bool TouchedHitbox(BoxCollider collider, Vector3 touchPos)
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
        /// Detects whether a given touch position hits the target button collider.
        /// </summary>
        /// <param name="collider">Collider to detect hits on.</param>
        /// <param name="touchPos">Touch position from DetectTouches().</param>
        /// <param name="layerMask">The layer the BoxCollider is on.</param>
        /// <returns></returns>
        public static bool TouchedHitbox(BoxCollider collider, Vector3 touchPos, LayerMask layerMask)
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
