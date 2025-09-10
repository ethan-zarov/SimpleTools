using EthanZarov.SimpleTools.EZKeyboard.Settings;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace EthanZarov.SimpleTools.EZKeyboard
{
    public class EZKeyboardButtonDisplay : MonoBehaviour
    {
        [SerializeField] private float xMult = 1;
        [SerializeField] private SpriteRenderer sr;
        [SerializeField] private TextMeshPro textbox;
        [SerializeField] private TextMeshPro altTextbox;

        [SerializeField] private BoxCollider2D boxCollider;
        
        private EZKeyboardButton _button;
        public float XMult => xMult;
        
        [SerializeField] private UnityEvent<EZKeyboardButton> OnButtonSet;
        
        
        
        public void SetButton(EZKeyboardButton button)
        {
            _button = button;
            
            OnButtonSet?.Invoke(button);
            textbox.text = button.character.ToString();
            if (altTextbox != null)
            {
                altTextbox.text = button.character.ToString();
            }
        }


        public void SetSize(float baseWidth, EZKeyboardSettings settings)
        {
            sr.size = new Vector2(baseWidth * xMult, settings.keyHeight);
            
            boxCollider.size = sr.size;
        }

        public virtual void PressButton()
        {
            EZKeyboardManager.PressButton(_button);
        }


    }
}