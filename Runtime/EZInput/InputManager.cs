using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Utilities
{
    public class InputManager : MonoBehaviour
    {
         private bool _isPC;
        private static InputManager _main;
    
        public static bool isPC;
        private static Camera _cam;

        [SerializeField] private ContactFilter2D buttonContactFilter; //Layer mask used for detecting GameButtons.
        private Collider2D[] _hitboxAlloc; //Allocated array for detecting buttons 
        private const int HitboxAllocSize = 20;

        private List<GameButton> _pressedObjects;
    
        private void Awake()
        {
            _cam = Camera.main;
            _main = this;
            isPC = false;
        
#if UNITY_EDITOR
            isPC = true;
#endif
        
            _pressedObjects = new List<GameButton>();
            _hitboxAlloc = new Collider2D[HitboxAllocSize];
        }

        private static Vector3 _onDownPosition; //Last touch's position on frame that press began.

        private static bool _getDown;
        private static bool _getHeld;
        private static bool _getUp;
        private static Touch _activeTouch;

        private void Update()
        {
            if (!_getHeld) //listen for new touch
            {
                _getUp = false;
                if (!isPC)
                {
                    if (Input.touchCount == 0) return;
                }
                else if (!Input.GetMouseButtonDown(0)) return;
            
                _getDown = true;
                _getHeld = true;
                PressButtonsDown();
            
                if (!isPC) _activeTouch = Input.GetTouch(0);
            }
            else //deactivate old touch
            {
                _getDown = false;
            
                if (!isPC)
                {
                    if (Input.touchCount != 0)
                    {
                        _activeTouch = Input.GetTouch(0);
                        return;
                    }
                }
                else
                {
                    if (Input.GetMouseButton(0)) return;
                }

            
                _getUp = true;
                _getHeld = false;
                ReleaseButtonsUp();
            }
    
        }
 

        private void PressButtonsDown()
        {
            _pressedObjects.Clear();
            Physics2D.OverlapPoint(TouchPosition(), buttonContactFilter, _hitboxAlloc);
            foreach (var hitbox in _hitboxAlloc)
            {
                if (hitbox == null) continue;

                var gameButton = hitbox.GetComponent<GameButton>();
                if (gameButton== null) continue;

                gameButton.PushDownButton();
                _pressedObjects.Add(gameButton);
            }
        
        }

        private void ReleaseButtonsUp()
        {

        
            foreach (var pressedObject in _pressedObjects)
            {
                pressedObject.LetGo();
            }
            
        
            Physics2D.OverlapPoint(TouchPosition(), buttonContactFilter, _hitboxAlloc);
            foreach (var hitbox in _hitboxAlloc)
            {
                if (hitbox == null) continue;

                var gameButton = hitbox.GetComponent<GameButton>();
                if (gameButton== null) continue;

                if (!_pressedObjects.Contains(gameButton)) continue;
            
                gameButton.CompleteButtonPress();
            }

            for (var index = 0; index < _hitboxAlloc.Length; index++)
            {
                _hitboxAlloc[index] = null;
            }
        }
    
        public static bool GetDown()
        { return _getDown; }

        public static bool GetHeld()
        { return _getHeld; }
    
        public static bool GetUp()
        { return _getUp; }
    
        public static Vector2 TouchPosition()
        {
            
            if (Input.touchCount != 0)
            {
                _activeTouch = Input.GetTouch(0);
            }
            Ray ray = _cam.ScreenPointToRay(isPC ? Input.mousePosition : _activeTouch.position);
            return ray.GetPoint(10); //TODO: if different angle, ensure distance to 0 is correect.
        
            return isPC ? _cam.ScreenToWorldPoint(Input.mousePosition) : _cam.ScreenToWorldPoint(_activeTouch.position);
        }

        public static bool OverUI()
        {
            if (isPC)
            {
                return EventSystem.current.IsPointerOverGameObject();
            }
            else
            {
                return EventSystem.current.IsPointerOverGameObject(_activeTouch.fingerId);
            }
        }
        
        public static Vector2 TouchPositionRaw()
        {
            var pos = isPC ? (Vector2)Input.mousePosition : _activeTouch.position;
            //Convert screen position to inches
            pos.x /= Screen.dpi;
            pos.y /= Screen.dpi;
            return pos;
        }


    }
}