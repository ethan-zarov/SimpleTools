using UnityEngine;
using UnityEngine.Events;

namespace Utilities
{
    public class GameButton : MonoBehaviour
    {
        public bool IsActive;
        [SerializeField] private bool useAnimation = true;
        [SerializeField] private Puffer puffer;
        [Space]
        [Space] [SerializeField] private UnityEvent pressButton;

        public BoxCollider2D Collider => _collider;
        private BoxCollider2D _collider;

        private void OnValidate()
        {
            if (_collider == null) TryGetComponent(out _collider);
        }


        public void SetActive(bool active)
        {
            IsActive = active;
        }
        
         
        public void PushDownButton()
        {
            if (!IsActive) return;

            if (useAnimation) puffer.PuffIn();
        }

        public void LetGo()
        {
            if (!IsActive) return;
            if (useAnimation) puffer.PuffOut();
        }


        public void CompleteButtonPress()
        {
            if (!IsActive) return;
            pressButton.Invoke();
            

        }

    }
}