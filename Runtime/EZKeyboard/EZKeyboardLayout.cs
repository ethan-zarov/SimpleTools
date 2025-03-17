using System;
using UnityEngine;

namespace EthanZarov.SimpleTools.EZKeyboard
{
    [CreateAssetMenu(fileName = "kbLayout", menuName = "EZTools/EZKeyboard/KeyboardLayout")]
    public class EZKeyboardLayout : ScriptableObject
    {
        public KeyboardRowLayout[] rows;

        [Serializable]
        public struct KeyboardRowLayout
        {
            public string rowName;
            public EZKeyboardButton[] buttons;
        }
    }
}