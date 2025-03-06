using System.Collections;
using UnityEngine;

namespace Utilities
{
    public class Mover : MonoBehaviour
    {
        [SerializeField] private bool useLocalPosition;
        [SerializeField] private float duration;
        public float Duration => duration;
        [SerializeField] private AnimationCurve curve;

        public void Stop()
        {
            if (_activeRoutine != null) StopCoroutine(_activeRoutine);
        }
        
        public Coroutine SetPosition(Vector3 position, bool immediate = false, bool adjustSizing = false)
        {
            if (_activeRoutine != null) StopCoroutine(_activeRoutine);
            if (!isActiveAndEnabled) immediate = true;

            if (immediate)
            {
                if (useLocalPosition) transform.localPosition = position;
                else transform.position = position;
                return null;
            }
            
            _activeRoutine = StartCoroutine(MoveRoutine(position, adjustSizing));
            return _activeRoutine;
        }

        private Coroutine _activeRoutine;
        private IEnumerator MoveRoutine(Vector3 targetPosition, bool adjustSizing)
        {
            var initPosition = useLocalPosition ? transform.localPosition : transform.position;
            var initRotation = useLocalPosition ? transform.localRotation : transform.rotation;
            var initScale = transform.localScale;

            float t = 0;

            while (t < 1)
            {
                var pos = Vector3.Lerp(initPosition, targetPosition, curve.Evaluate(t));
                var rot = Quaternion.Lerp(initRotation, Quaternion.identity, curve.Evaluate(t));
                var scale = Vector3.Lerp(initScale, Vector3.one, curve.Evaluate(t));
                if (useLocalPosition)
                {
                    transform.localPosition = pos;
                    transform.localRotation = rot;
                    if (adjustSizing) transform.localScale = scale;
                }
                else
                {
                    transform.position = pos;
                    transform.rotation = rot;
                    if (adjustSizing) transform.localScale = scale;
                }
                
                t += Time.deltaTime / duration;
                yield return null;

            }

            if (useLocalPosition)
            {
                transform.localPosition = targetPosition;
                transform.localRotation = Quaternion.identity;
                if (adjustSizing) transform.localScale =  Vector3.one;
            }
            else
            {
                transform.position = targetPosition;
                transform.rotation = Quaternion.identity;
                if (adjustSizing) transform.localScale = Vector3.one;
            }
        }
    }
}