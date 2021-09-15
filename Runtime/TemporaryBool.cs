using UnityEngine;
using System.Collections.Generic;
using EthanZarov.SimpleTools;

namespace EthanZarov.SimpleTools
{
    /// - Boolean whose true toggle operates on a timer or temporary value. - ///
    public class TemporaryBool
    {
        
        private bool value;
        private float timer;
        float timeActive;

        private float defaultTimerLength; //

        public TemporaryBool() {  defaultTimerLength = .5f; }
        public TemporaryBool(float typicalDuration) { defaultTimerLength = typicalDuration; }



        public bool IsActive()
        {
            return value;
        }

        public void Activate()
        {
            value = true;
            timer = defaultTimerLength;

            timeActive = 0;

        }
        public void Activate(float customDuration)
        {
            value = true;
            timer = customDuration;

            timeActive = 0;
        }

        /// <summary>
        /// Overrides timer and automatically sets the TemporaryBool to false.
        /// </summary>
        public void OverrideDeactivate()
        {
            value = false;
            timer = -1;
        }


        public float TimeActive()
        {
            return timeActive;
        }

        /// <summary>
        /// (Should be) called once per frame.
        /// </summary> 
        public void Tick()
        {
            if (value)
            {
                timer -= Time.deltaTime;

                if (timer < 0)
                {
                    value = false;
                }
            }

            timeActive += Time.deltaTime;
        }


    }
}
