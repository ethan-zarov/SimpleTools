using System.Collections;
using UnityEngine;

namespace Utilities
{
    public class Puffer : MonoBehaviour
    {
        [SerializeField] private float defaultScale;
        [SerializeField] private float puffScale;
        public float ActiveScale => puffScale;
    
        [SerializeField] private AnimationCurve puffInCurve;
        [SerializeField] private float puffInDuration;

        [SerializeField] private float midDelay;
    
    
        [SerializeField] private AnimationCurve puffOutCurve;
        [SerializeField] private float puffOutDuration;

        [SerializeField] private bool overridePrevious;
        public bool InPuffing { get; private set; }
    
        private bool _executePuffIn;
        private bool _executePuffOut;

        public void PuffIn()
        {
            PuffIn(0);
        }

        public void PuffOut()
        {
            PuffOut(0);
        }
        
        public Coroutine PuffFull(float delay = 0f, int loop = 0)
        {
            _executePuffIn = true;
            _executePuffOut = true;
            return PlayPuffRoutine(delay, loop);
        }

        public Coroutine PuffIn(float delay = 0f)
        {
            _executePuffIn = true;
            _executePuffOut = false;
            return PlayPuffRoutine(delay);
        }

        public Coroutine PuffOut(float delay = 0f)
        {
            if (_executePuffOut && _executePuffIn && !overridePrevious) return _puffRoutine;
            _executePuffIn = false;
            _executePuffOut = true;
        
            return PlayPuffRoutine(delay);
        }

        public void StopPuffing()
        {
            if (_puffRoutine != null) StopCoroutine(_puffRoutine);
        }
        
    
        

        private Coroutine _puffRoutine;
        private Coroutine PlayPuffRoutine(float d = 0f, int loop = 0)
        {
            if (_puffRoutine != null) StopCoroutine(_puffRoutine);
        
            InPuffing = false;
            if (!isActiveAndEnabled) return null;


        
            _puffRoutine = StartCoroutine(PuffRoutine(d, loop));
            return _puffRoutine;
        }

        private IEnumerator PuffRoutine(float delay = 0f, int loop = 0)
        {
            InPuffing = true;
            if (delay > .01f) yield return new WaitForSeconds(delay);
        
            float t = 0;
            float initialScale = transform.localScale.x;

            if (_executePuffIn)
            {
                while (t < 1f)
                {
                    var scale = Mathf.LerpUnclamped(initialScale, puffScale, puffInCurve.Evaluate(t));
                    transform.localScale = Vector3.one * scale;

                    t += Time.deltaTime / puffInDuration;
                    yield return null;
                }

                transform.localScale = Vector3.one * puffScale;
            }

            if (midDelay > .01f)  yield return new WaitForSecondsRealtime(midDelay);
        
            if (_executePuffOut)
            {
                t = 0;
                initialScale = transform.localScale.x;
            
                while (t < 1f)
                {
                    var scale = Mathf.LerpUnclamped(initialScale, defaultScale, puffOutCurve.Evaluate(t));
                    transform.localScale = Vector3.one * scale;

                    t += Time.deltaTime / puffOutDuration;
                    yield return null;
                }

                transform.localScale = Vector3.one * defaultScale;
            }

            if (loop == 0)
            {
            
                InPuffing = false;
                yield break;
            }
        
            loop--;
            yield return PuffRoutine(0, loop);
        }
    
    
    }
}