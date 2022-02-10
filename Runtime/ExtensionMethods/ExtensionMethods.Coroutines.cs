using UnityEngine;
using System.Collections;
using UnityEngine.UI;


namespace EthanZarov.SimpleTools
{
    public static partial class ExtensionMethods
    {

        #region SpriteRenderers
        
        /// <summary>
        /// Shifts the color of the SpriteRenderer over a period of time.
        /// </summary>
        /// <param name="spriteRenderer">SpriteRenderer whose colors to shift.</param>
        /// <param name="targetColor">Color to shift to.</param>
        /// <param name="duration">Duration of the shift.</param>
        /// <param name="delay">Delay before starting the shift.</param>
        /// <param name="useUnscaledTime">Should the coroutine use scaled or unscaled delta time? False by default.</param>
        /// <param name="coroutineHost">What MonoBehaviour should "host" the coroutine? If unset, the function will attempt to find a suitable MonoBehaviour attached to the referenced SpriteRenderer.</param>
        /// <returns>Returns the Coroutine being executed.</returns>
        public static Coroutine ShiftColorOverTime(this SpriteRenderer spriteRenderer, Color targetColor, float duration = 1f, float delay = 0f, bool useUnscaledTime = false, MonoBehaviour coroutineHost = null)
        {
            if (duration <= 0) return null;
            
            MonoBehaviour mb;
            if (coroutineHost == null)
            {
                mb = spriteRenderer.gameObject.GetComponent<MonoBehaviour>();
                if (mb == null)
                {
                    Debug.LogError("Error calling ShiftColor on SpriteRenderer attached to " + spriteRenderer.gameObject.name +
                                   ". Please make sure that there is a MonoBehaviour attached to it, or that coroutineHost in this function is assigned.");
                    return null;
                }
            }
            else
            {
                mb = coroutineHost;
            }
            
            return mb.StartCoroutine(ShiftColor_SpriteRenderer(spriteRenderer, spriteRenderer.color, targetColor, duration, AnimationCurve.Linear(0,0,1,1), delay, useUnscaledTime));
        }

        /// <summary>
        /// Shifts the color of the SpriteRenderer over a period of time.
        /// </summary>
        /// <param name="spriteRenderer">SpriteRenderer whose colors to shift.</param>
        /// <param name="initialColor">Starting color to shift from.</param>
        /// <param name="targetColor">Color to shift to.</param>
        /// <param name="duration">Duration of the shift.</param>
        /// <param name="curve">AnimationCurve of the shift.</param>
        /// <param name="delay">Delay before starting the shift.</param>
        /// <param name="useUnscaledTime">Should the coroutine use scaled or unscaled delta time? False by default.</param>
        /// <param name="coroutineHost">What MonoBehaviour should "host" the coroutine? If unset, the function will attempt to find a suitable MonoBehaviour attached to the referenced SpriteRenderer.</param>
        /// <returns>Returns the Coroutine being executed.</returns>
        public static Coroutine ShiftColorOverTime(this SpriteRenderer spriteRenderer, Color initialColor, Color targetColor, float duration, AnimationCurve curve, float delay = 0f, bool useUnscaledTime = false, MonoBehaviour coroutineHost = null)
        {
            if (duration <= 0) return null;

            MonoBehaviour mb;
            if (coroutineHost == null)
            {
                mb = spriteRenderer.gameObject.GetComponent<MonoBehaviour>();
                if (mb == null)
                {
                    Debug.LogError("Error calling ShiftColor on Graphic attached to " + spriteRenderer.gameObject.name +
                                   ". Please make sure that there is a MonoBehaviour attached to it, or that coroutineHost in this function is assigned.");
                    return null;
                }
            }
            else
            {
                mb = coroutineHost;
            }

            return mb.StartCoroutine(ShiftColor_SpriteRenderer(spriteRenderer, initialColor, targetColor, duration, curve, delay, useUnscaledTime));
        }
        
        #endregion

        #region Graphics
        
        /// <summary>
        /// Shifts the color of the Graphic over a period of time.
        /// </summary>
        /// <param name="graphic">Graphic whose colors to shift.</param>
        /// <param name="targetColor">Color to shift to.</param>
        /// <param name="duration">Duration of the shift.</param>
        /// <param name="delay">Delay before starting the shift.</param>
        /// <param name="useUnscaledTime">Should the coroutine use scaled or unscaled delta time? False by default.</param>
        /// <param name="coroutineHost">What MonoBehaviour should "host" the coroutine? If unset, the function will attempt to find a suitable MonoBehaviour attached to the referenced Graphic.</param>
        /// <returns>Returns the Coroutine being executed.</returns>
        public static Coroutine ShiftColorOverTime(this Graphic graphic, Color targetColor, float duration = 1f, float delay = 0f, bool useUnscaledTime = false, MonoBehaviour coroutineHost = null)
        {
            if (duration <= 0) return null;
            
            MonoBehaviour mb;
            if (coroutineHost == null)
            {
                mb = graphic.gameObject.GetComponent<MonoBehaviour>();
                if (mb == null)
                {
                    Debug.LogError("Error calling ShiftColor on Graphic attached to " + graphic.gameObject.name +
                                   ". Please make sure that there is a MonoBehaviour attached to it, or that coroutineHost in this function is assigned.");
                    return null;
                }
            }
            else
            {
                mb = coroutineHost;
            }
            
            return mb.StartCoroutine(ShiftColor_Graphic(graphic, graphic.color, targetColor, duration, AnimationCurve.Linear(0,0,1,1), delay, useUnscaledTime));
        }

        /// <summary>
        /// Shifts the color of the Graphic over a period of time.
        /// </summary>
        /// <param name="graphic">Graphic whose colors to shift.</param>
        /// <param name="initialColor">Starting color to shift from.</param>
        /// <param name="targetColor">Color to shift to.</param>
        /// <param name="duration">Duration of the shift.</param>
        /// <param name="curve">AnimationCurve of the shift.</param>
        /// <param name="delay">Delay before starting the shift.</param>
        /// <param name="useUnscaledTime">Should the coroutine use scaled or unscaled delta time? False by default.</param>
        /// <param name="coroutineHost">What MonoBehaviour should "host" the coroutine? If unset, the function will attempt to find a suitable MonoBehaviour attached to the referenced Graphic.</param>
        /// <returns>Returns the Coroutine being executed.</returns>
        public static Coroutine ShiftColorOverTime(this Graphic graphic, Color initialColor, Color targetColor, float duration, AnimationCurve curve, float delay = 0f, bool useUnscaledTime = false, MonoBehaviour coroutineHost = null)
        {
            if (duration <= 0) return null;

            MonoBehaviour mb;
            if (coroutineHost == null)
            {
                mb = graphic.gameObject.GetComponent<MonoBehaviour>();
                if (mb == null)
                {
                    Debug.LogError("Error calling ShiftColor on Graphic attached to " + graphic.gameObject.name +
                                   ". Please make sure that there is a MonoBehaviour attached to it, or that coroutineHost in this function is assigned.");
                    return null;
                }
            }
            else
            {
                mb = coroutineHost;
            }

            return mb.StartCoroutine(ShiftColor_Graphic(graphic, initialColor, targetColor, duration, curve, delay, useUnscaledTime));
        }

        #endregion

        #region Transforms

        /// <summary>
        /// Shifts the position of a Transform over a period of time.
        /// </summary>
        /// <param name="transform">Transform whose position to shift.</param>
        /// <param name="targetPosition">Position to shift to.</param>
        /// <param name="duration">Duration of the shift.</param>
        /// <param name="delay">Delay before starting the shift.</param>
        /// <param name="useUnscaledTime">Should the coroutine use scaled or unscaled delta time? False by default.</param>
        /// <param name="useLocalPosition">Should the positions be relative to the local space of "transform?"</param>
        /// <param name="coroutineHost">What MonoBehaviour should "host" the coroutine? If unset, the function will attempt to find a suitable MonoBehaviour attached to the referenced Transform.</param>
        /// <returns>Returns the Coroutine being executed.</returns>
        public static Coroutine ShiftPositionOverTime(this Transform transform, Vector3 targetPosition, float duration, float delay = 0f, bool useUnscaledTime = false, bool useLocalPosition = false, MonoBehaviour coroutineHost = null)
        {
            if (duration <= 0) return null;

            MonoBehaviour mb;
            if (coroutineHost == null)
            {
                mb = transform.gameObject.GetComponent<MonoBehaviour>();
                if (mb == null)
                {
                    Debug.LogError("Error calling ShiftPosition on Transform attached to " + transform.gameObject.name +
                                   ". Please make sure that there is a MonoBehaviour attached to it, or that coroutineHost in this function is assigned.");
                    return null;
                }
            }
            else
            {
                mb = coroutineHost;
            }

            return mb.StartCoroutine(ShiftPosition_Transform(transform, useLocalPosition ? transform.localPosition : transform.position, targetPosition, duration, AnimationCurve.Linear(0,0,1,1), delay, useUnscaledTime, useLocalPosition));
        }
        
        /// <summary>
        /// Shifts the position of a Transform over a period of time.
        /// </summary>
        /// <param name="transform">Transform whose position to shift.</param>
        /// <param name="initialPosition">Starting position to shift from.</param>
        /// <param name="targetPosition">Position to shift to.</param>
        /// <param name="duration">Duration of the shift.</param>
        /// <param name="curve">AnimationCurve of the shift.</param>
        /// <param name="delay">Delay before starting the shift.</param>
        /// <param name="useUnscaledTime">Should the coroutine use scaled or unscaled delta time? False by default.</param>
        /// <param name="useLocalPosition">Should the positions be relative to the local space of "transform?"</param>
        /// <param name="coroutineHost">What MonoBehaviour should "host" the coroutine? If unset, the function will attempt to find a suitable MonoBehaviour attached to the referenced Transform.</param>
        /// <returns>Returns the Coroutine being executed.</returns>
        public static Coroutine ShiftPositionOverTime(this Transform transform, Vector3 initialPosition, Vector3 targetPosition, float duration, AnimationCurve curve, float delay = 0f, bool useUnscaledTime = false, bool useLocalPosition = false, MonoBehaviour coroutineHost = null)
        {
            if (duration <= 0) return null;

            MonoBehaviour mb;
            if (coroutineHost == null)
            {
                mb = transform.gameObject.GetComponent<MonoBehaviour>();
                if (mb == null)
                {
                    Debug.LogError("Error calling ShiftPosition on Transform attached to " + transform.gameObject.name +
                                   ". Please make sure that there is a MonoBehaviour attached to it, or that coroutineHost in this function is assigned.");
                    return null;
                }
            }
            else
            {
                mb = coroutineHost;
            }

            return mb.StartCoroutine(ShiftPosition_Transform(transform, initialPosition, targetPosition, duration, curve, delay, useUnscaledTime, useLocalPosition));
        }
        
        /// <summary>
        /// Shifts the rotation of a Transform over a period of time.
        /// </summary>
        /// <param name="transform">Transform whose rotation to shift.</param>
        /// <param name="targetRotation">Rotation to shift to.</param>
        /// <param name="duration">Duration of the shift.</param>
        /// <param name="delay">Delay before starting the shift.</param>
        /// <param name="useUnscaledTime">Should the coroutine use scaled or unscaled delta time? False by default.</param>
        /// <param name="useLocalRotation">Should the rotations be relative to the local space of "transform?"</param>
        /// <param name="coroutineHost">What MonoBehaviour should "host" the coroutine? If unset, the function will attempt to find a suitable MonoBehaviour attached to the referenced Transform.</param>
        /// <returns>Returns the Coroutine being executed.</returns>
        public static Coroutine ShiftRotationOverTime(this Transform transform, Quaternion targetRotation, float duration, float delay = 0f, bool useUnscaledTime = false, bool useLocalRotation = false, MonoBehaviour coroutineHost = null)
        {
            if (duration <= 0) return null;

            MonoBehaviour mb;
            if (coroutineHost == null)
            {
                mb = transform.gameObject.GetComponent<MonoBehaviour>();
                if (mb == null)
                {
                    Debug.LogError("Error calling ShiftRotation on Transform attached to " + transform.gameObject.name +
                                   ". Please make sure that there is a MonoBehaviour attached to it, or that coroutineHost in this function is assigned.");
                    return null;
                }
            }
            else
            {
                mb = coroutineHost;
            }

            return mb.StartCoroutine(ShiftRotation_Transform(transform, useLocalRotation ? transform.localRotation : transform.rotation, targetRotation, duration, AnimationCurve.Linear(0,0,1,1), delay, useUnscaledTime, useLocalRotation));
        }
        
        /// <summary>
        /// Shifts the rotation of a Transform over a period of time.
        /// </summary>
        /// <param name="transform">Transform whose rotation to shift.</param>
        /// <param name="initialRotation">Starting rotation to shift from.</param>
        /// <param name="targetRotation">Rotation to shift to.</param>
        /// <param name="duration">Duration of the shift.</param>
        /// <param name="curve">AnimationCurve of the shift.</param>
        /// <param name="delay">Delay before starting the shift.</param>
        /// <param name="useUnscaledTime">Should the coroutine use scaled or unscaled delta time? False by default.</param>
        /// <param name="useLocalRotation">Should the rotations be relative to the local space of "transform?"</param>
        /// <param name="coroutineHost">What MonoBehaviour should "host" the coroutine? If unset, the function will attempt to find a suitable MonoBehaviour attached to the referenced Transform.</param>
        /// <returns>Returns the Coroutine being executed.</returns>
        public static Coroutine ShiftRotationOverTime(this Transform transform, Quaternion initialRotation, Quaternion targetRotation, float duration, AnimationCurve curve, float delay = 0f, bool useUnscaledTime = false, bool useLocalRotation = false, MonoBehaviour coroutineHost = null)
        {
            if (duration <= 0) return null;

            MonoBehaviour mb;
            if (coroutineHost == null)
            {
                mb = transform.gameObject.GetComponent<MonoBehaviour>();
                if (mb == null)
                {
                    Debug.LogError("Error calling ShiftRotation on Transform attached to " + transform.gameObject.name +
                                   ". Please make sure that there is a MonoBehaviour attached to it, or that coroutineHost in this function is assigned.");
                    return null;
                }
            }
            else
            {
                mb = coroutineHost;
            }

            return mb.StartCoroutine(ShiftRotation_Transform(transform, initialRotation, targetRotation, duration, curve, delay, useUnscaledTime, useLocalRotation));
        }
        
        
        #endregion
        
        


        #region Base Enumerators
        private static IEnumerator ShiftColor_SpriteRenderer(SpriteRenderer sr, Color initialColor, Color targetColor,
            float duration, AnimationCurve curve, float delay, bool useUnscaledTime)
        {
            if (delay > 0.001f)
            {
                if (useUnscaledTime) yield return new WaitForSecondsRealtime(delay);
                else yield return new WaitForSeconds(delay);
            }
            
            float t = 0;
            while (t < 1f)
            {
                sr.color = Color.Lerp(initialColor, targetColor, curve.Evaluate(t));
                
                t += (useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime) / duration;
                yield return new WaitForEndOfFrame();
            }

            sr.color = targetColor;
        }
        
        private static IEnumerator ShiftColor_Graphic(Graphic graphic, Color initialColor, Color targetColor,
            float duration, AnimationCurve curve, float delay, bool useUnscaledTime)
        {
            if (delay > 0.001f)
            {
                if (useUnscaledTime) yield return new WaitForSecondsRealtime(delay);
                else yield return new WaitForSeconds(delay);
            }
            
            float t = 0;
            while (t < 1f)
            {
                graphic.color = Color.Lerp(initialColor, targetColor, curve.Evaluate(t));
                
                t += (useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime) / duration;
                yield return new WaitForEndOfFrame();
            }

            graphic.color = targetColor;
        }
        
        
        private static IEnumerator ShiftPosition_Transform(Transform transform, Vector3 initialPosition, Vector3 targetPosition,
            float duration, AnimationCurve curve, float delay, bool useUnscaledTime, bool useLocalPosition)
        {
            if (delay > 0.001f)
            {
                if (useUnscaledTime) yield return new WaitForSecondsRealtime(delay);
                else yield return new WaitForSeconds(delay);
            }

            float t = 0;
            while (t < 1f)
            {
                if (useLocalPosition) transform.localPosition = Vector3.Lerp(initialPosition, targetPosition, curve.Evaluate(t));
                else transform.position = Vector3.Lerp(initialPosition, targetPosition, curve.Evaluate(t));
                
                t += (useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime) / duration;
                yield return new WaitForEndOfFrame();
            }

            if (useLocalPosition) transform.localPosition = targetPosition;
            else transform.position = targetPosition;
        }
        
                
        private static IEnumerator ShiftRotation_Transform(Transform transform, Quaternion initialRotation, Quaternion targetRotation,
            float duration, AnimationCurve curve, float delay, bool useUnscaledTime, bool useLocalPosition)
        {
            if (delay > 0.001f)
            {
                if (useUnscaledTime) yield return new WaitForSecondsRealtime(delay);
                else yield return new WaitForSeconds(delay);
            }

            float t = 0;
            while (t < 1f)
            {
                if (useLocalPosition) transform.localRotation = Quaternion.Lerp(initialRotation, targetRotation, curve.Evaluate(t));
                else transform.rotation = Quaternion.Lerp(initialRotation, targetRotation, curve.Evaluate(t));
                
                t += (useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime) / duration;
                yield return new WaitForEndOfFrame();
            }

            if (useLocalPosition) transform.localRotation = targetRotation;
            else transform.rotation = targetRotation;
        }
        

        

        #endregion

    }

}