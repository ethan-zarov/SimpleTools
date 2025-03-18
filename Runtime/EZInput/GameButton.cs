using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Utilities
{
    public class GameButton : MonoBehaviour
    {
        [SerializeField] private bool defaultActive = true;
        
        [SerializeField] private bool useAnimation = true;
        [SerializeField, ShowIf("useAnimation")] private Puffer puffer;
        [Space]
        [Space] [SerializeField] private UnityEvent pressButton;

        public bool IsActive
        {
            get
            {
              if (!_activeInitialized) return defaultActive;
              return _isActive;
            }
            private set => _isActive = value;
        }

        private bool _activeInitialized;
        private bool _isActive;


        //[FoldoutGroup("On Down")]
        
        public BoxCollider2D Collider => _collider;
        private BoxCollider2D _collider;

        private void OnValidate()
        {
            if (_collider == null) TryGetComponent(out _collider);
        }


        public void SetActive(bool active)
        {
            _activeInitialized = true;
            IsActive = active;
        }
        
         
        public void PushDownButton()
        {
            if (!IsActive && !Input.GetKey(KeyCode.LeftShift)) return;

            if (useAnimation) puffer.PuffIn();
        }

        public void LetGo()
        {
            if (!IsActive && !Input.GetKey(KeyCode.LeftShift)) return;
            if (useAnimation) puffer.PuffOut();
        }


        public void CompleteButtonPress()
        {
            if (!IsActive && !Input.GetKey(KeyCode.LeftShift)) return;
            pressButton.Invoke();
            

        }

    }
}