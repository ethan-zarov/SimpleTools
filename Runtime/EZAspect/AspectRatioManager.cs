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
        [SerializeField] private bool secondaryUpdate = true;

        private static AspectRatioManager _instance;
        private static readonly List<ChangerEntry> _entries = new();
        private static int _registrationCounter;
        private static bool _needsSort;
        private float _currentAspectRatio;
        private int _lastScreenWidth;
        private int _lastScreenHeight;
        private bool _refitCamera = true;
        private bool _forceUpdate;
        private Coroutine _deferredRefreshRoutine;

        private struct ChangerEntry : IComparable<ChangerEntry>
        {
            public AspectRatioChanger Changer;
            public AspectRatioUpdatePhase Phase;
            public int Order;
            public int RegistrationIndex;

            public int CompareTo(ChangerEntry other)
            {
                int phaseCompare = Phase.CompareTo(other.Phase);
                if (phaseCompare != 0) return phaseCompare;

                int orderCompare = Order.CompareTo(other.Order);
                if (orderCompare != 0) return orderCompare;

                return RegistrationIndex.CompareTo(other.RegistrationIndex);
            }
        }

        /// <summary>Fired after each aspect-ratio pipeline run (both passes).</summary>
        public static event Action OnPipelineComplete;

        public static void Refresh()
        {
            if (_instance == null) return;
            _instance._refitCamera = true;
            _instance._forceUpdate = true;
            _instance.UpdateAspectRatio();
        }

        public static void ScheduleRefresh()
        {
            if (_instance == null) return;

            _instance._refitCamera = true;
            _instance._forceUpdate = true;
            _instance.UpdateAspectRatio();

            if (_instance._deferredRefreshRoutine != null)
                _instance.StopCoroutine(_instance._deferredRefreshRoutine);
            _instance._deferredRefreshRoutine = _instance.StartCoroutine(_instance.DeferredRefresh());
        }

        public static void Register(AspectRatioChanger aspectRatioChanger)
        {
            if (aspectRatioChanger == null) return;

            foreach (var step in aspectRatioChanger.GetUpdateSteps())
            {
                _entries.Add(new ChangerEntry
                {
                    Changer = aspectRatioChanger,
                    Phase = step.Phase,
                    Order = step.Order,
                    RegistrationIndex = _registrationCounter++,
                });
            }

            _needsSort = true;
        }

        public static void Unregister(AspectRatioChanger aspectRatioChanger)
        {
            if (aspectRatioChanger == null) return;

            for (int i = _entries.Count - 1; i >= 0; i--)
            {
                if (_entries[i].Changer == aspectRatioChanger)
                    _entries.RemoveAt(i);
            }
        }

        private void Awake()
        {
            _instance = this;
            _cam = Camera.main;
        }

        private void Start()
        {
            ScheduleRefresh();
            if (secondaryUpdate) StartCoroutine(SecondaryUpdate());
        }

        private void Update()
        {
            if (_cam == null) _cam = Camera.main;
            if (_cam == null) return;

            if (HasViewportChanged()) _refitCamera = true;

            if (constantUpdate || _forceUpdate || _refitCamera)
                UpdateAspectRatio();
        }

        private IEnumerator DeferredRefresh()
        {
            yield return null;
            _refitCamera = true;
            _forceUpdate = true;
            UpdateAspectRatio();

            yield return new WaitForEndOfFrame();
            _refitCamera = true;
            _forceUpdate = true;
            UpdateAspectRatio();

            _deferredRefreshRoutine = null;
        }

        private IEnumerator SecondaryUpdate()
        {
            yield return new WaitForSeconds(.25f);
            ScheduleRefresh();
        }

        public void UpdateAspectRatio()
        {
            if (_cam == null) _cam = Camera.main;
            if (_cam == null) return;

            _currentAspectRatio = GetEffectiveAspect(_cam);
            Aspect = _currentAspectRatio;
            _lastScreenWidth = Screen.width;
            _lastScreenHeight = Screen.height;

            if (_needsSort)
            {
                _entries.Sort();
                _needsSort = false;
            }

            // Pass 1: full pipeline including camera.
            RunPipeline(includeCamera: true);

            // Pass 2: re-align and re-fit to the final camera without moving it again.
            RunPipeline(includeCamera: false);

            _refitCamera = false;
            _forceUpdate = false;

            OnPipelineComplete?.Invoke();
        }

        private void RunPipeline(bool includeCamera)
        {
            for (int i = 0; i < _entries.Count; i++)
            {
                var entry = _entries[i];
                if (!includeCamera && IsCameraPhase(entry.Phase))
                    continue;

                AspectRatioChanger.CurrentPhase = entry.Phase;
                entry.Changer.UpdateAspectRatio(_cam);
            }
        }

        private bool HasViewportChanged()
        {
            if (Screen.width != _lastScreenWidth || Screen.height != _lastScreenHeight)
                return true;

            float effective = GetEffectiveAspect(_cam);
            return Math.Abs(_currentAspectRatio - effective) > .001f;
        }

        private static float GetEffectiveAspect(Camera cam)
        {
            if (Screen.height > 0)
            {
                float screenAspect = (float)Screen.width / Screen.height;
                if (cam != null && cam.aspect > 0.001f)
                    return screenAspect;
                return screenAspect;
            }

            return cam != null && cam.aspect > 0.001f ? cam.aspect : 1f;
        }

        private static bool IsCameraPhase(AspectRatioUpdatePhase phase)
        {
            return phase == AspectRatioUpdatePhase.CameraFirst || phase == AspectRatioUpdatePhase.CameraPost;
        }
    }
}
