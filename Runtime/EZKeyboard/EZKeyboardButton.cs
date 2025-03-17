using System;

namespace EthanZarov.SimpleTools.EZKeyboard
{
    [Serializable]
    public struct EZKeyboardButton
    {
        public EZKeyboardButtonType buttonType;
        public char character;

        public string GetObjectName()
        {
            if (buttonType == EZKeyboardButtonType.Character) return $"{character}-Button";
            else return "Backspace";
        }
    }
}