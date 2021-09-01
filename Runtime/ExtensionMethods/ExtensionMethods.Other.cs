using UnityEngine;
using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;

namespace EthanZarov.SimpleTools
{
    public static partial class ExtensionMethods
    {
        #region Lists

        /// <summary>
        /// Shuffles an IList into a random order.
        /// </summary>
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = ThreadSafeRandom.ThisThreadsRandom.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        /// <summary>
        /// Duplicates an IList without reference.
        /// </summary>
        public static IList<T> Clone<T>(this IList<T> listToClone) where T : ICloneable
        {
            return listToClone.Select(item => (T)item.Clone()).ToList();
        }

        #endregion

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
    }


    public static class ThreadSafeRandom
    {
        [ThreadStatic] private static System.Random Local;

        public static System.Random ThisThreadsRandom
        {
            get { return Local ?? (Local = new System.Random(unchecked(Environment.TickCount * 31 + Thread.CurrentThread.ManagedThreadId))); }
        }
    }
}