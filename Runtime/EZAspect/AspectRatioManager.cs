using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aspect_Ratio
{
    public class AspectRatioManager : MonoBehaviour
    {
        [SerializeField] private bool constantUpdate = true;
        public static float Aspect { get; private set; }
        private Camera _cam;
        [Space]

        private static HashSet<AspectRatioChanger> _aspectRatioChangers;
        private static HashSet<AspectRatioChanger> _aspectRatioChangersLate;
        private float _currentAspectRatio;
        
        [SerializeField] private bool secondaryUpdate;

        public static void Register(AspectRatioChanger aspectRatioChanger, bool lateUpdate = false)
        {
            if (lateUpdate)
            {
                _aspectRatioChangersLate ??= new HashSet<AspectRatioChanger>();
                _aspectRatioChangersLate.Add(aspectRatioChanger);
            }
            else
            {
                _aspectRatioChangers ??= new HashSet<AspectRatioChanger>();
                _aspectRatioChangers.Add(aspectRatioChanger);
            }
        }


        private void Awake()
        {
            
            _cam = Camera.main;
            _aspectRatioChangers = new HashSet<AspectRatioChanger>();
            _aspectRatioChangersLate = new HashSet<AspectRatioChanger>();
        }

        private void Start()
        {
            UpdateAspectRatio();
            if (secondaryUpdate) StartCoroutine(SecondaryUpdate());
        }

        private void Update()
        {

            if (constantUpdate || Math.Abs(_currentAspectRatio - _cam.aspect) > .001f || Time.frameCount < 10) UpdateAspectRatio();

            
        }

        private IEnumerator SecondaryUpdate()
        {
            yield return new WaitForSeconds(.25f);
            yield return null;
            UpdateAspectRatio();
        }

        public void UpdateAspectRatio()
        {
            _currentAspectRatio = _cam.aspect;
            Aspect = _currentAspectRatio;
            
            
            foreach (var aspectRatioChanger in _aspectRatioChangers)
            {
                aspectRatioChanger.UpdateAspectRatio(_cam);
            }
            
            foreach (var aspectRatioChanger in _aspectRatioChangersLate)
            {
                aspectRatioChanger.UpdateAspectRatio(_cam);
            }
        }
    
    }
}