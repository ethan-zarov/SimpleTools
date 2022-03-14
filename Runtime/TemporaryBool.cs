using UnityEngine;
using System.Collections.Generic;
using EthanZarov.SimpleTools;

namespace EthanZarov.SimpleTools
{
    /// - Boolean whose true toggle operates on a timer or temporary value. - ///
    /// - Functions with "Tick" decrease the timer til zero, setting the TemporaryBool back to false. - ///
    /// -
    public class TemporaryBool
    {
        private float _timer;

        private readonly float _defaultTimerLength;

        public TemporaryBool() {  _defaultTimerLength = 1f; }
        public TemporaryBool(float typicalDuration) { _defaultTimerLength = typicalDuration; }


        private bool _permanentlyActive;
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
        /// Activates the TemporaryBool until it's manually turned off.
        /// </summary>
        public void PermaActivate()
        {
            _permanentlyActive = true;
            IsActive = true;
        }

        /// <summary>
        /// Overrides timer and automatically sets the TemporaryBool to false.
        /// </summary>
        public void OverrideDeactivate()
        {
            IsActive = false;
            _permanentlyActive = false;
            _timer = -1;
        }


        /// <summary>
        /// Ticks down the timer by Time.deltaTime, setting the TemporaryBool to false when it reaches zero.
        /// </summary> 
        public void Tick()
        {
            TimeActive += Time.deltaTime;
            if (_permanentlyActive) return;
            if (IsActive)
            {
                _timer -= Time.deltaTime;

                if (_timer < 0)
                {
                    IsActive = false;
                }
            }

        }

        /// <summary>
        /// Ticks down the timer by Time.fixedDeltaTime, setting the TemporaryBool to false when it reaches zero.
        /// </summary>  
        public void FixedTick()
        {
            TimeActive += Time.fixedDeltaTime;
            if (_permanentlyActive) return;
            if (IsActive)
            {
                _timer -= Time.fixedDeltaTime;

                if (_timer < 0)
                {
                    IsActive = false;
                }
            }

        }

        
        /// <summary>
        /// Ticks down the timer by Time.unscaledDeltaTime, setting the TemporaryBool to false when it reaches zero.
        /// </summary> 
        public void UnscaledTick()
        {
            TimeActive += Time.unscaledDeltaTime;
            if (_permanentlyActive) return;
            if (IsActive)
            {
                _timer -= Time.unscaledDeltaTime;

                if (_timer < 0)
                {
                    IsActive = false;
                }
            }

        }

        /// <summary>
        /// Ticks down the timer by Time.fixedUnscaledDeltaTime, setting the TemporaryBool to false when it reaches zero.
        /// </summary> 
        public void FixedUnscaledTick()
        {
            TimeActive += Time.fixedUnscaledDeltaTime;
            if (_permanentlyActive) return;
            if (IsActive)
            {
                _timer -= Time.fixedUnscaledDeltaTime;

                if (_timer < 0)
                {
                    IsActive = false;
                }
            }

        }
    }
}
