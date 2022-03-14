using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;


namespace EthanZarov.SimpleTools
{
    public static partial class ExtensionMethods
    {   
        /// <summary>
        /// Logs a current value with a name into the console.
        /// </summary>
        /// <param name="value">Value to log.</param>
        /// <param name="name">Name to assign it to.</param>
        /// <param name="type">Type of log to push to the console.</param>
        /// <returns>"Name: value"</returns>
        public static string LogValue<T>(this T value, string name, LogType type = LogType.Log)
        {
            switch (type)
            {
                case LogType.Log:
                    Debug.Log($"{name}: {value.ToString()}");
                    break;
                case LogType.Error:
                    Debug.LogError($"{name}: {value.ToString()}");
                    break;
                case LogType.Assert:
                    Debug.LogAssertion($"{name}: {value.ToString()}");
                    break;
                case LogType.Warning:
                    Debug.LogWarning($"{name}: {value.ToString()}");
                    break;
                case LogType.Exception:
                    Debug.LogError($"{name}: {value.ToString()}");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
            
            return $"{name}: {value.ToString()}";
        }



        /// <summary>
        /// Logs a current list with a name into the console.
        /// </summary>
        /// <param name="collection">Values to log.</param>
        /// <param name="name">Name to assign it to.</param>
        /// <param name="maxSize">Max amount of values out of the collection to display.</param>
        /// <param name="type">Type of log to push to the console.</param>
        /// <returns>"Name: value, value, value..."</returns>
        public static string LogCollection<T>(this ICollection<T> collection, string name, int maxSize = -1, LogType type = LogType.Log)
        {
            if (collection == null)
            {
                Debug.LogWarning($"{name}: COLLECTION NULL");
                return $"{name}: COLLECTION NULL";
            }

            if (collection.Count == 0)
            {
                Debug.Log($"{name}: EMPTY COLLECTION");
                return $"{name}: EMPTY COLLECTION";
            }
            
            
            var output = "";
            var maxLength = maxSize == -1 ? collection.Count : Mathf.Min(collection.Count, maxSize);

            for (var i = 0; i < maxLength; i++)
            {
                output += collection.ElementAt(i).ToString();
                if (i < maxLength - 1) output += ", ";
            }

            if (maxLength == maxSize) output += "...";

            switch (type)
            {
                case LogType.Log:
                    Debug.Log($"{name}: {output}");
                    break;
                case LogType.Error:
                    Debug.LogError($"{name}: {output}");
                    break;
                case LogType.Assert:
                    Debug.LogAssertion($"{name}: {output}");
                    break;
                case LogType.Warning:
                    Debug.LogWarning($"{name}: {output}");
                    break;
                case LogType.Exception:
                    Debug.LogError($"{name}: {output}");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
            
            return $"{name}: {output}";
        }
    }

}