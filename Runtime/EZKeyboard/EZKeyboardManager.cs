using System;
using System.Collections.Generic;
using System.Linq;
using Aspect_Ratio;
using UnityEngine;

namespace EthanZarov.SimpleTools.EZKeyboard.Settings
{
    public class EZKeyboardManager : MonoBehaviour
    {
        private static EZKeyboardManager _main;
        public static Action<char> OnCharacterButtonPress;
        public static Action OnBackspaceButtonPress;
        
        [SerializeField] private EZKeyboardSettings settings;
        [SerializeField] private SpriteRenderer backdropSr;
        [SerializeField] private Transform globalRowHolder;
        private Transform[] _rowHolders;
        
        
        
        private List<EZKeyboardButtonDisplay> _allDisplays;
        private KeyboardRowDisplay[] _rowDisplays;
        private int RowCount => settings.layout.rows.Length;
        
        [Serializable]
        public struct KeyboardRowDisplay
        {
            public EZKeyboardButtonDisplay[] rowDisplays;
        }
        
        private Camera _cam;

        #region Initialization
        
        private void Awake()
        {
            _main = this;
            _cam = Camera.main;
            InitializeKeys();
        }



        private void InitializeKeys()
        {
            
            _allDisplays = new List<EZKeyboardButtonDisplay>();
            _rowHolders = new Transform[RowCount];
            _rowDisplays = new KeyboardRowDisplay[RowCount];

            for (int i = 0; i < RowCount; i++)
            {
                var row = settings.layout.rows[i];
                GameObject rowHolder = new GameObject(row.rowName);
                rowHolder.transform.SetParent(globalRowHolder);
                _rowHolders[i] = rowHolder.transform;
                CreateKeyboardRow(ref _rowDisplays[i].rowDisplays, row.buttons, rowHolder.transform);
            }
        }

        private void CreateKeyboardRow(ref EZKeyboardButtonDisplay[] output, EZKeyboardButton[] input, Transform outputTransform)
        {
            output = new EZKeyboardButtonDisplay[input.Length];

            for (int i = 0; i < input.Length; i++)
            {
                output[i] = SpawnButtonDisplay(input[i], outputTransform);
                _allDisplays.Add(output[i]);   
            }
        }

        private EZKeyboardButtonDisplay SpawnButtonDisplay(EZKeyboardButton button, Transform parentTransform)
        {
            EZKeyboardButtonDisplay template = button.buttonType == EZKeyboardButtonType.Character
                ? settings.characterButtonDisplayTemplate
                : settings.backspaceButtonDisplayTemplate;

            if (button.buttonType == EZKeyboardButtonType.Custom)
            {
                template = settings.customDisplayTemplates[button.customButtonPrefabIndex];
            }
            
            var output = Instantiate(template, parentTransform);
            output.name = button.GetObjectName();

            output.SetButton(button);
            return output;
        }
        
        #endregion
        
        
        private void Update()
        {
            Redraw();
            
            if (Input.inputString != "")
            {
                var upper = Input.inputString.ToUpper();
                foreach (char c in upper)
                {
                    if ((int)c < 'A' || (int)c > 'Z') continue;
                    OnCharacterButtonPress?.Invoke(c);
                }
            }
            
            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                OnBackspaceButtonPress?.Invoke();
            }
        }

        private void Redraw()
        {
            UpdateBackground();
            float baseButtonWidth = CalculateButtonBaseWidth();
            SetButtonSizes(baseButtonWidth);

            for (int i = 0; i < RowCount; i++)
            {
                UpdateRowPositions(_rowDisplays[i].rowDisplays, baseButtonWidth);
            }
            
            float scaleFactor = GetScaleFactor();
            globalRowHolder.localScale = new Vector3(scaleFactor, scaleFactor, 1);
            
            
            float rowYReader = settings.yMargins.y + settings.keyHeight / 2;

            for (int i = 0; i < RowCount; i++)
            {
                _rowHolders[i].localPosition = new Vector3(0, -rowYReader, 0);
                rowYReader += settings.keyHeight + settings.yPadding;
            }
            
            float globalRowY = _cam.ViewportToWorldPoint(Vector3.up * (settings.viewportSizeY)).y;
            globalRowHolder.localPosition = new Vector3(0, globalRowY, 0);
            
            
        }
        
        private void UpdateBackground()
        {
            float backgroundYHeight = settings.viewportSizeY.ViewportSizeYToWorldSizeY(_cam);
            backdropSr.size = new Vector2(1f.ViewportSizeXToWorldSizeX(_cam), backgroundYHeight);
        }

        private void SetButtonSizes(float baseButtonWidth)
        {
            foreach (var display in _allDisplays)
            {
                display.SetSize(baseButtonWidth, settings);
            }
        }

        private float GetScaleFactor()
        {
            return settings.viewportSizeY.ViewportSizeYToWorldSizeY(_cam) / CalculateKeyboardHeight();
        }

        private float CalculateKeyboardHeight()
        {
            float buttonTotalHeight = settings.layout.rows.Length * settings.keyHeight;
            float totalPadding = 2 * settings.yPadding;
            float totalMargins = settings.yMargins.x + settings.yMargins.y;
            
            return buttonTotalHeight + totalPadding + totalMargins;
        }

        private float CalculateKeyboardWidth()
        {
            return CalculateKeyboardHeight() / GetKeyboardAspect();
        }
        
        private float GetKeyboardAspect()
        {
            return settings.viewportSizeY.ViewportSizeYToWorldSizeY(_cam) / 1f.ViewportSizeXToWorldSizeX(_cam);
        }
        
        private float CalculateButtonBaseWidth()
        {
            var widestRow = GetWidestRowArray().rowDisplays;
            float totalMultWidth = GetTotalButtonMult(widestRow);
            float totalPadding = (widestRow.Length-1) * settings.xPadding;
            float totalMargins = settings.xMargins.x + settings.xMargins.y;
            
            float totalButtonAvailability = CalculateKeyboardWidth() - totalPadding - totalMargins;
            return totalButtonAvailability / totalMultWidth;
        }

        private KeyboardRowDisplay GetWidestRowArray()
        {
            float maxMult = 0;
            KeyboardRowDisplay widestRow = _rowDisplays[0];
            
            foreach (var row in _rowDisplays)
            {
                float rowMult = GetTotalButtonMult(row.rowDisplays);
                if (rowMult > maxMult)
                {
                    maxMult = rowMult;
                    widestRow = row;
                }
            }
            
            return widestRow;
        }
        
        private float GetTotalButtonMult(EZKeyboardButtonDisplay[] row)
        {
            return row.Sum(b => b.XMult);
        }
        
        
        //Place all the buttons so that they will appear centered relative to 0,0 local position of row transform. Each button width can be gotten by getting button.XMult * baseButtonWidth
        private void UpdateRowPositions(EZKeyboardButtonDisplay[] buttons, float buttonBaseWidth)
        {
            float totalWidth = GetTotalButtonMult(buttons) * buttonBaseWidth;
            float totalPadding = (buttons.Length - 1) * settings.xPadding;
            float totalMargins = settings.xMargins.x + settings.xMargins.y;
            float totalAvailability = totalWidth + totalPadding + totalMargins;
            float startingX = -totalAvailability / 2 + settings.xMargins.x;
            
            for (int i = 0; i < buttons.Length; i++)
            {
                float buttonWidth = buttons[i].XMult * buttonBaseWidth;
                float buttonX = startingX + buttonWidth / 2;
                buttons[i].transform.localPosition = new Vector3(buttonX, 0, 0);
                startingX += buttonWidth + settings.xPadding;
            }
        }

        public static void PressButton(EZKeyboardButton button)
        {
            switch (button.buttonType)
            {
                case EZKeyboardButtonType.Character:
                    OnCharacterButtonPress?.Invoke(button.character);
                    break;
                case EZKeyboardButtonType.Backspace:
                    OnBackspaceButtonPress?.Invoke();
                    break;
            }
        }
    }
}