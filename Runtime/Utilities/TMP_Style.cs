using TMPro;
using UnityEngine;

namespace EthanZarov {

    public class TMPStyle : MonoBehaviour
    {
        [SerializeField] private TMPStyleType styleType;

        private TextMeshPro _textbox;
        private TextMeshProUGUI _textboxUGUI;


        private void Awake()
        {
            if (GetComponent<TextMeshPro>())
            {
                _textbox = GetComponent<TextMeshPro>();

                _textbox.font = styleType.font;
                _textbox.fontStyle = styleType.style;
            }
            else
            {
                _textboxUGUI = GetComponent<TextMeshProUGUI>();

                _textboxUGUI.fontStyle = styleType.style;
                _textboxUGUI.font = styleType.font;
            }
        }
    }


    [CreateAssetMenu(fileName = "TextStyle", menuName = "TextMeshPro/Text Style")]
    public class TMPStyleType : ScriptableObject
    {
        public TMP_FontAsset font;
        public FontStyles style;
    }

}