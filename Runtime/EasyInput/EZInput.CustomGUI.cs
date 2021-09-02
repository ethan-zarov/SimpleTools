using UnityEngine;
using System.Collections.Generic;
using EthanZarov.SimpleTools;

namespace EthanZarov.EZInput
{
    /// - Handles any custom slider or UI interactions - ///
    public static partial class EZInput
    {
        /// <summary>
        /// Manages knob of a horizontal slider with multiple snapped positions.
        /// </summary>
        /// <param name="_transform">Transform of the managed knob.</param>
        /// <param name="indexReturn">The target index of snapped position list currently selected.</param>
        /// <param name="inMoveReturn">Returns true when slider is currently being changed by player.</param>
        /// <param name="targetPositions">Array of all possible target positions.</param>
        /// <param name="clampValues">Maximum left (x) and right (y) values the slider can go.</param>
        /// <param name="hitbox">The hitbox of the knob itself.</param>
        /// <param name="totalHitbox">The total hitbox of the slider that when touched will still trigger movement.</param>
        public static void SnappedSliderPosition(this Transform _transform, ref int indexReturn, ref bool inMoveReturn, Vector2[] targetPositions, Vector2 clampValues, BoxCollider hitbox, BoxCollider totalHitbox)
        {
            bool holdingDown;
            bool pressedFrame;
            Vector3 touchPos;

            DetectTouchesHeld(out holdingDown, out pressedFrame, out touchPos);
            Vector3 localTouchPos = touchPos.GlobalToLocalPos(_transform.GetLocalOffset());
            localTouchPos.z = 0;
            if (pressedFrame && EZI_Touched(totalHitbox, touchPos))
            {
                int bestIndex = 0;
                float bestDistance = 999;
                for (int i = 0; i < targetPositions.Length; i++)
                {
                    touchPos.y = _transform.position.y;
                    float distance = Vector2.Distance(localTouchPos, targetPositions[i]);
                    if (distance < bestDistance)
                    {
                        bestIndex = i;
                        bestDistance = distance;
                    }
                }
                indexReturn = bestIndex;
                inMoveReturn = true;
                _transform.localPosition = targetPositions[indexReturn];

            }
            else if (holdingDown && inMoveReturn)
            {
                Vector2 newPos = _transform.localPosition;

                newPos.x = Mathf.Clamp(localTouchPos.x, clampValues.x, clampValues.y);
                _transform.localPosition = newPos;

                int bestIndex = 0;
                float bestDistance = 999;
                for (int i = 0; i < targetPositions.Length; i++)
                {
                    float distance = Vector2.Distance(localTouchPos, targetPositions[i]);
                    if (distance < bestDistance)
                    {
                        bestIndex = i;
                        bestDistance = distance;
                    }
                }

                indexReturn = bestIndex;
                inMoveReturn = true;
            }
            else
            {
                int bestIndex = 0;
                float bestDistance = 999;
                for (int i = 0; i < targetPositions.Length; i++)
                {
                    float distance = Vector2.Distance(_transform.localPosition, targetPositions[i]);
                    if (distance < bestDistance)
                    {
                        bestIndex = i;
                        bestDistance = distance;
                    }
                }

                int indexStore = bestIndex;

                if (inMoveReturn)
                {
                    if (bestIndex == indexReturn)
                    {
                        inMoveReturn = false;
                        //AudioManager.main.PlaySelectSound(AudioManager.SelectionSound.HighSharp, .8f, .1f, 1, 0);
                    }
                }
                else
                {
                    indexReturn = indexStore;
                }


                Vector2 targetPosition = targetPositions[indexReturn];
                targetPosition.x = Mathf.Clamp(targetPosition.x, clampValues.x, clampValues.y);
                _transform.localPosition = Vector2.Lerp(_transform.localPosition, targetPosition, 15 * Time.deltaTime);

            }

        }

        public static void SliderPosition(this Transform _transform, ref bool inMoveReturn, Vector2 clampValues, BoxCollider hitbox, BoxCollider totalHitbox)
        {
            bool holdingDown;
            bool pressedFrame;
            Vector3 touchPos;

            DetectTouchesHeld(out holdingDown, out pressedFrame, out touchPos);
            Vector3 localTouchPos = touchPos.GlobalToLocalPos(_transform.GetLocalOffset());
            localTouchPos.z = 0;
            if (pressedFrame && totalHitbox.EZI_Touched(touchPos))
            {
                inMoveReturn = true;
                Vector3 localPos = _transform.localPosition;
                localPos.x = Mathf.Clamp(localTouchPos.x, clampValues.x, clampValues.y);

                _transform.localPosition = localPos;

            }
            else if (holdingDown && inMoveReturn)
            {
                Vector2 newPos = _transform.localPosition;

                newPos.x = Mathf.Clamp(localTouchPos.x, clampValues.x, clampValues.y);
                _transform.localPosition = newPos;
            }
            else
            {
                if (inMoveReturn)
                {
                    inMoveReturn = false;
                    //AudioManager.main.PlaySelectSound(AudioManager.SelectionSound.HighSharp, .8f, .1f, 1, 0);

                }


            }

        }

        public static void VerticalScrollSlider(this Transform _transform, ref Vector2 initTouchP, ref Vector2 initLocalPos,
        ref bool inMoveReturn, Vector2 clampValues, BoxCollider hitbox, ref float yVelocity, ref float initialTime, float[] targetYPositions)
        {
            bool holdingDown;
            bool pressedFrame;
            Vector3 touchPos;

            DetectTouchesHeld(out holdingDown, out pressedFrame, out touchPos);
            Vector3 localTouchPos = touchPos.GlobalToLocalPos(_transform.GetLocalOffset());
            localTouchPos.z = 0;
            if (pressedFrame && hitbox.EZI_Touched(touchPos))
            {
                inMoveReturn = true;
                initTouchP = localTouchPos;
                initLocalPos = _transform.localPosition;
                initialTime = Time.time;
                //MMVibrationManager.TransientHaptic(.45f, .7f);
                //AudioManager.main.PlaySelectSound(AudioManager.SelectionSound.HighSharp, .6f, .1f, 1, 0);
            }
            else if (holdingDown && inMoveReturn)
            {


                float yDifference = localTouchPos.y - initTouchP.y;
                Vector2 newPos = initLocalPos;
                newPos.y = Mathf.Clamp(newPos.y + yDifference, clampValues.x, clampValues.y);
                _transform.localPosition = newPos;
            }
            else
            {
                if (inMoveReturn)
                {
                    inMoveReturn = false;


                    yVelocity = _transform.localPosition.y - initLocalPos.y;

                    yVelocity = Mathf.Sign(yVelocity) * Mathf.Clamp(Mathf.Abs(yVelocity), 0, 4.5f);

                }

                if (Mathf.Abs(yVelocity) > .4f)
                {
                    float yAdd = yVelocity * Time.deltaTime * 2;

                    Vector2 newPos = _transform.localPosition;
                    newPos.y += yAdd;
                    if (newPos.y < clampValues.x || newPos.y > clampValues.y)
                    {
                        yVelocity *= .1f;
                        newPos.y = Mathf.Clamp(newPos.y, clampValues.x, clampValues.y);
                    }

                    _transform.localPosition = newPos;
                    yVelocity = Mathf.Lerp(yVelocity, 0, 1 - Mathf.Pow(.002f, Time.deltaTime));
                }
                else
                {
                    float tY = _transform.localPosition.y;
                    int closestIndex = 0;
                    float closestDistance = 999;
                    for (int i = 0; i < targetYPositions.Length; i++)
                    {
                        if (Mathf.Abs(tY - targetYPositions[i]) < closestDistance)
                        {
                            closestDistance = Mathf.Abs(tY - targetYPositions[i]);
                            closestIndex = i;
                        }
                    }
                    if (closestDistance > .04f)
                    {
                        Vector2 newPos = _transform.localPosition;
                        newPos.y = Mathf.Lerp(tY, targetYPositions[closestIndex], .01f);

                        if (closestDistance * .9f < .04f)
                        {
                            newPos.y = targetYPositions[closestIndex];

                            //AudioManager.main.PlaySelectSound(AudioManager.SelectionSound.HighSharp, .3f, .1f, 1, 0);
                            //MMVibrationManager.TransientHaptic(.45f, .7f);
                        }
                        _transform.localPosition = newPos;
                    }
                }

            }

        }
    }
}
