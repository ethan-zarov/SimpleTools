using UnityEngine;


namespace TMPro {

    public class TMP_Style : MonoBehaviour
    {
        [SerializeField] private TMP_StyleType styleType;

        TextMeshPro textbox;
        TextMeshProUGUI textboxUGUI;

        bool isUGUI;

        private void Awake()
        {
            if (GetComponent<TextMeshPro>())
            {
                textbox = GetComponent<TextMeshPro>();

                textbox.font = styleType.font;
                textbox.fontStyle = styleType.style;
            }
            else
            {
                textboxUGUI = GetComponent<TextMeshProUGUI>();


                textboxUGUI.fontStyle = styleType.style;
                textboxUGUI.font = styleType.font;
            }
        }
    }


    [CreateAssetMenu(fileName = "TextStyle", menuName = "TextMeshPro/Text Style")]
    public class TMP_StyleType : ScriptableObject
    {
        public TMP_FontAsset font;
        public FontStyles style;
    }

}