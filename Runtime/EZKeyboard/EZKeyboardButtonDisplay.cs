using EthanZarov.SimpleTools.EZKeyboard.Settings;
using TMPro;
using UnityEngine;

namespace EthanZarov.SimpleTools.EZKeyboard
{
    public class EZKeyboardButtonDisplay : MonoBehaviour
    {
        [SerializeField] private float xMult = 1;
        [SerializeField] private SpriteRenderer sr;
        [SerializeField] private TextMeshPro textbox;

        [SerializeField] private BoxCollider2D boxCollider;
        
        private EZKeyboardButton _button;
        public float XMult => xMult;
        
        
        public void SetButton(EZKeyboardButton button)
        {
            _button = button;
            textbox.text = button.character.ToString();
        }


        public void SetSize(float baseWidth, EZKeyboardSettings settings)
        {
            sr.size = new Vector2(baseWidth * xMult, settings.keyHeight);
            
            boxCollider.size = sr.size;
        }

        public void PressButton()
        {
            EZKeyboardManager.PressButton(_button);
        }


    }
}