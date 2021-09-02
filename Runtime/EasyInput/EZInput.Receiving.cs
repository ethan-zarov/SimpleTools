using UnityEngine;
using System.Collections.Generic;
using EthanZarov.SimpleTools;

namespace EthanZarov.EZInput
{
    /// - Handles any and all incoming touch input - ///
    public static partial class EZInput
    {
        /// <summary>
        /// Detects whether the player touched the screen at any position.
        /// </summary>
        /// <returns>Returns true when pressed.</returns>
        public static bool DetectPress()
        {

            if (Input.GetMouseButtonDown(0))
            {
                return true;
            }
            else if (Input.touchCount > 0)
            {
                for (int i = 0; i < Input.touchCount; i++)
                {
                    if (Input.GetTouch(i).phase == TouchPhase.Began) return true;
                }

            }

            return false;
        }

        /// <summary>
        /// Detects when and where the screen is touched from Camera.main on desktop/mobile.
        /// </summary>
        /// <param name="touched">Outputs true if screen is touched/clicked on this frame.</param>
        /// <param name="touchP">The output position of touch.</param>
        public static void DetectTouches(out bool touched, out Vector3 touchP)
        {
            if (Input.touchCount > 0 || Input.GetMouseButtonDown(0))
            {
                bool pressedFrame = false;
                Vector3 touchPos = Vector3.zero;
                if (Input.touchCount > 0)
                {
                    Touch touch;
                    for (int i = 0; i < Input.touchCount; i++)
                    {
                        touch = Input.GetTouch(i);
                        if (touch.phase == TouchPhase.Began)
                        {
                            touchPos = Camera.main.ScreenToWorldPoint(touch.position);
                            pressedFrame = true;
                        }
                    }

                }
                else
                {
                    touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    pressedFrame = true;
                }

                if (pressedFrame)
                {
                    touchPos.z = 0;
                    touched = true;
                    touchP = touchPos;
                }
                else
                {
                    touched = false;
                    touchP = Vector3.zero;
                }
            }
            else
            {
                touched = false;
                touchP = Vector3.zero;
            }
        }
        /// <summary>
        /// Detects when and where the screen is touched from "usedCamera" on desktop/mobile.
        /// </summary>
        /// <param name="touched">Outputs true if screen is touched/clicked on this frame.</param>
        /// <param name="touchP">The output position of touch.</param>
        /// <param name="usedCamera">The camera to look for touch position from.</param>
        public static void DetectTouches(out bool touched, out Vector3 touchP, Camera usedCamera)
        {
            if (Input.touchCount > 0 || Input.GetMouseButtonDown(0))
            {
                bool pressedFrame = false;
                Vector3 touchPos = Vector3.zero;
                if (Input.touchCount > 0)
                {
                    Touch touch;
                    for (int i = 0; i < Input.touchCount; i++)
                    {
                        touch = Input.GetTouch(i);
                        if (touch.phase == TouchPhase.Began)
                        {
                            touchPos = usedCamera.ScreenToWorldPoint(touch.position);
                            pressedFrame = true;
                        }
                    }
                }
                else
                {
                    touchPos = usedCamera.ScreenToWorldPoint(Input.mousePosition);
                    pressedFrame = true;
                }

                if (pressedFrame)
                {
                    touchPos.z = 0;
                    touched = true;
                    touchP = touchPos;
                }
                else
                {
                    touched = false;
                    touchP = Vector3.zero;
                }
            }
            else
            {
                touched = false;
                touchP = Vector3.zero;
            }
        }

        /// <summary>
        /// Detects touch (mouse or touchscreen) position, and state, whether just touched or currently being held.
        /// </summary>
        /// <param name="touchHeld">Returns true whenever mouse is down or finger is on screen.</param>
        /// <param name="touchBegan">Returns true only on the first frame mouse is clicked on screen is touched.</param>
        /// <param name="touchP">The touch position to return. Vector3.zero when nothing touched.</param>
        public static void DetectTouchesHeld(out bool touchHeld, out bool touchBegan, out Vector3 touchP)
        {
            if (Input.GetMouseButton(0) || Input.touchCount > 0)
            {
                touchHeld = true;
            }
            else
            {
                touchHeld = false;
            }


            if (Input.touchCount > 0 || Input.GetMouseButton(0))
            {
                bool pressedFrame = false;
                Vector3 touchPos = Vector3.zero;
                if (Input.touchCount > 0)
                {
                    Touch touch = Input.GetTouch(0);
                    if (touch.phase == TouchPhase.Began)
                    {

                        pressedFrame = true;
                    }

                    touchPos = Camera.main.ScreenToWorldPoint(touch.position);
                }
                else
                {
                    touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    if (Input.GetMouseButtonDown(0))
                    {
                        pressedFrame = true;
                    }
                }

                if (pressedFrame)
                {
                    touchPos.z = 0;
                    touchBegan = true;
                    touchP = touchPos;
                }
                else if (touchHeld)
                {
                    touchBegan = false;
                    touchPos.z = 0;
                    touchP = touchPos;
                }
                else
                {
                    touchBegan = false;
                    touchP = Vector3.zero;
                }
            }
            else
            {
                touchBegan = false;
                touchP = Vector3.zero;
            }
        }
        /// <summary>
        /// Detects touch (mouse or touchscreen) position, and state, whether just touched or currently being held.
        /// </summary>
        /// <param name="touchHeld">Returns true whenever mouse is down or finger is on screen.</param>
        /// <param name="touchBegan">Returns true only on the first frame mouse is clicked on screen is touched.</param>
        /// <param name="touchP">The touch position to return. Vector3.zero when nothing touched.</param>
        /// <param name="usedCamera">The camera to detect touches from.</param>
        public static void DetectTouchesHeld(out bool touchHeld, out bool touchBegan, out Vector3 touchP, Camera usedCamera)
        {
            if (Input.GetMouseButton(0) || Input.touchCount > 0)
            {
                touchHeld = true;
            }
            else
            {
                touchHeld = false;
            }


            if (Input.touchCount > 0 || Input.GetMouseButton(0))
            {
                bool pressedFrame = false;
                Vector3 touchPos = Vector3.zero;
                if (Input.touchCount > 0)
                {
                    Touch touch = Input.GetTouch(0);
                    if (touch.phase == TouchPhase.Began)
                    {

                        pressedFrame = true;
                    }

                    touchPos = usedCamera.ScreenToWorldPoint(touch.position);
                }
                else
                {
                    touchPos = usedCamera.ScreenToWorldPoint(Input.mousePosition);
                    if (Input.GetMouseButtonDown(0))
                    {
                        pressedFrame = true;
                    }
                }

                if (pressedFrame)
                {
                    touchPos.z = 0;
                    touchBegan = true;
                    touchP = touchPos;
                }
                else if (touchHeld)
                {
                    touchBegan = false;
                    touchPos.z = 0;
                    touchP = touchPos;
                }
                else
                {
                    touchBegan = false;
                    touchP = Vector3.zero;
                }
            }
            else
            {
                touchBegan = false;
                touchP = Vector3.zero;
            }
        }


    }
}
