using UnityEngine;
using System.Collections.Generic;
using EthanZarov.SimpleTools;

namespace EthanZarov.SimpleTools
{
    /// - Boolean whose true toggle operates on a timer or temporary value. - ///
    public class TemporaryBool
    {
        private float _timer;

        private readonly float _defaultTimerLength;

        public TemporaryBool() {  _defaultTimerLength = 1f; }
        public TemporaryBool(float typicalDuration) { _defaultTimerLength = typicalDuration; }
        

        public bool IsActive { get; private set; }
        public float TimeActive { get; private set; }

        public void Activate()
        {
            IsActive = true;
            _timer = _defaultTimerLength;

            TimeActive = 0;

        }
        public void Activate(float customDuration)
        {
            IsActive = true;
            _timer = customDuration;

            TimeActive = 0;
        }

        /// <summary>
        /// Overrides timer and automatically sets the TemporaryBool to false.
        /// </summary>
        public void OverrideDeactivate()
        {
            IsActive = false;
            _timer = -1;
        }


        /// <summary>
        /// (Should be) called once per frame.
        /// </summary> 
        public void Tick()
        {
            if (IsActive)
            {
                _timer -= Time.deltaTime;

                if (_timer < 0)
                {
                    IsActive = false;
                }
            }

            TimeActive += Time.deltaTime;
        }

        /// <summary>
        /// (Should be) called once per fixed frame.
        /// </summary> 
        public void FixedTick()
        {
            if (IsActive)
            {
                _timer -= Time.fixedDeltaTime;

                if (_timer < 0)
                {
                    IsActive = false;
                }
            }

            TimeActive += Time.fixedDeltaTime;
        }

        
        /// <summary>
        /// (Should be) called once per frame.
        /// </summary> 
        public void UnscaledTick()
        {
            if (IsActive)
            {
                _timer -= Time.unscaledDeltaTime;

                if (_timer < 0)
                {
                    IsActive = false;
                }
            }

            TimeActive += Time.unscaledDeltaTime;
        }

        /// <summary>
        /// (Should be) called once per fixed frame.
        /// </summary> 
        public void FixedUnscaledTick()
        {
            if (IsActive)
            {
                _timer -= Time.fixedUnscaledDeltaTime;

                if (_timer < 0)
                {
                    IsActive = false;
                }
            }

            TimeActive += Time.fixedUnscaledDeltaTime;
        }
    }
}
