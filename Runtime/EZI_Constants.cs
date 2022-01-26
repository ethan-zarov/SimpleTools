using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EthanZarov.EZInput
{
    public class EZI_Constants : MonoBehaviour
    {

        
        [Tooltip("Main camera which static variables Pressed and TouchP are based upon. Other cameras that want DetectTouches need to call EZInput functions manually."), SerializeField] 
        private Camera mainCamera;
        
        [Tooltip("When disabled, input will not be detected."), SerializeField]
        private bool receivesInput = true;
        
        [Tooltip("Max amount of colliders that a single frame can register as touched. Default is 5."), SerializeField]
        private int maxColliders = 5;

        
        [Space]
        [Tooltip("If enabled, each frame will print out touch position and what is being touched."), SerializeField] 
        private bool debugMode;


        /// <summary>
        /// If true, mouse/mobile input was pressed this frame.
        /// </summary>
        public static bool Pressed;
        /// <summary>
        /// Most recently logged position of mouse/touch.
        /// NOTE: This only logs TouchP when mouse is clicked or when touch is clicked.
        /// Does not account for dragging or mouse movement while click is up.
        /// </summary>
        public static Vector3 TouchP;
        /// <summary>
        /// All colliders touched. Mainly internal, used for detecting whether touched.
        /// </summary>
        public static Collider[] Colliders;


        private void Awake()
        {
            if (Colliders == null)
            {
                Colliders = new Collider[5];
            }

            receivesInput = true;
        }

        private void Update()
        {
            if (receivesInput)
            {
                Pressed = false;
                EZInput.DetectTouches(out Pressed, out TouchP, mainCamera);

                if (Pressed)
                {
                    Physics.OverlapSphereNonAlloc(TouchP, .1f, Colliders); 
                }
                else ClearArray();



            }

            if (debugMode) DebugTouch();
        }

        private static void ClearArray()
        {
            for (var i = 0; i < 5; i++)
            {
                Colliders[i] = null;
            }
        }

        private static void DebugTouch()
        {
            if (!Pressed) return;
            var debugStr = "Touched at " + TouchP + '\n' + "Hitboxes: ";
            foreach (var t in Colliders)
            {
                if (t != null)
                {
                    debugStr += t.transform.gameObject.name + " // ";
                }
            }

                
            Debug.Log(debugStr);
        }

        /// <summary>
        /// Turn on/off input registering.
        /// </summary>
        /// <param name="inputOn"></param>
        public void AllowInput(bool inputOn)
        {
            receivesInput = inputOn;
            Pressed = false;
            TouchP = Vector3.one * -1000f;
        }
    }
}
