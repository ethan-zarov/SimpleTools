using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EthanZarov.EZInput
{
    public class EZI_Constants : MonoBehaviour
    {

        [SerializeField] bool receivesInput;
        [SerializeField] bool debugMode;

        public static bool pressed;
        public static Vector3 touchP;

        public static Collider[] colliders;


        [SerializeField] private Camera mainCamera;


        private void Awake()
        {
            if (colliders == null)
            {
                colliders = new Collider[5];
            }

            receivesInput = true;
        }

        private void Update()
        {
            if (receivesInput)
            {
                pressed = false;
                EZInput.DetectTouches(out pressed, out touchP, mainCamera);

                if (pressed)
                {
                    Physics.OverlapSphereNonAlloc(touchP, .1f, colliders); 
                }
                else ClearArray();



            }

            if (debugMode) DebugTouch();
        }

        void ClearArray()
        {
            for (int i = 0; i < 5; i++)
            {
                colliders[i] = null;
            }
        }


        void DebugTouch()
        {
            if (pressed)
            {
                string debugStr = "Touched at " + touchP + '\n' + "Hitboxes: ";
                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i] != null)
                    {
                        debugStr += colliders[i].transform.gameObject.name + " // ";
                    }
                }

                Debug.Log(debugStr);
            }
        }

    }
}
