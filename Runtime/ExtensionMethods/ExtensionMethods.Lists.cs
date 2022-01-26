using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;

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
        /// Get all possible combinations of a list of items (elements) of size k.
        /// </summary>
        /// <param name="elements">All items to draw from.</param>
        /// <param name="k">Amount of items to draw.</param>
        /// <typeparam name="T">Type of items.</typeparam>
        /// <returns>Returns every possible list/combination of k items out of elements.</returns>
        public static IEnumerable<IEnumerable<T>> GetCombinations<T>(this IEnumerable<T> elements, int k)
        {
            return k == 0 ? new[] { new T[0] } :
                elements.SelectMany((e, i) =>
                    elements.Skip(i + 1).GetCombinations(k - 1).Select(c => (new[] { e }).Concat(c)));
        }

        /// <summary>
        /// Duplicates an IList without reference.
        /// </summary>
        public static IList<T> Clone<T>(this IList<T> listToClone) where T : ICloneable
        {
            return listToClone.Select(item => (T)item.Clone()).ToList();
        }

        /// <summary>
        /// Returns a random item from a list.
        /// </summary>
        /// <param name="list">List to draw from.</param>
        /// <typeparam name="T">Type of item in list.</typeparam>
        /// <returns>Random item from list.</returns>
        public static T GetRandomItem<T>(this IList<T> list)
        {
            return list.Count == 0 ? default(T) : list[UnityEngine.Random.Range(0, list.Count)];
        }
        
        /// <summary>
        /// Swaps two values in a collection.
        /// </summary>
        /// <param name="list">Collection to reference</param>
        /// <param name="indexA">First index in collection.</param>
        /// <param name="indexB">Second index in collection.</param>
        /// <typeparam name="T">Type within collection.</typeparam>
        public static void SwapIndices<T>(this IList<T> list, int indexA, int indexB)
        {
            (list[indexA], list[indexB]) = (list[indexB], list[indexA]);
        }

    }


    public static class ThreadSafeRandom
    {
        [ThreadStatic] private static System.Random _local;
        public static System.Random ThisThreadsRandom => _local ?? (_local = new System.Random(unchecked(Environment.TickCount * 31 + Thread.CurrentThread.ManagedThreadId)));
    }
}