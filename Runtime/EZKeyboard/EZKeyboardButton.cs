using System;
using Sirenix.OdinInspector;

namespace EthanZarov.SimpleTools.EZKeyboard
{
    [Serializable]
    public struct EZKeyboardButton
    {
        public EZKeyboardButtonType buttonType;
        [ShowIf("@buttonType == EZKeyboardButtonType.Character")] public char character;
        [ShowIf("@buttonType == EZKeyboardButtonType.Custom")] public int customButtonPrefabIndex;

        public string GetObjectName()
        {
            if (buttonType == EZKeyboardButtonType.Character) return $"{character}-Button";
            else return "Backspace";
        }
    }
}