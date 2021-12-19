using UnityEngine;
using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using Random = System.Random;

namespace EthanZarov.SimpleTools
{
    public static partial class ExtensionMethods
    {
        /// <summary>
        /// Shuffles an IList into a random order.
        /// </summary>
        public static void Shuffle<T>(this IList<T> list)
        {
            var n = list.Count;
            while (n > 1)
            {
                n--;
                var k = ThreadSafeRandom.ThisThreadsRandom.Next(n + 1);
                (list[k], list[n]) = (list[n], list[k]);
            }
        }

        /// <summary>
        /// Duplicates an IList without reference.
        /// </summary>
        public static IList<T> Clone<T>(this IList<T> listToClone) where T : ICloneable
        {
            return listToClone.Select(item => (T)item.Clone()).ToList();
        }

        public static T GetRandomItem<T>(this IList<T> list)
        {
            return list.Count == 0 ? default(T) : list[UnityEngine.Random.Range(0, list.Count)];
        }
        

    }


    public static class ThreadSafeRandom
    {
        [ThreadStatic] private static System.Random _local;
        public static System.Random ThisThreadsRandom => _local ?? (_local = new System.Random(unchecked(Environment.TickCount * 31 + Thread.CurrentThread.ManagedThreadId)));
    }
}