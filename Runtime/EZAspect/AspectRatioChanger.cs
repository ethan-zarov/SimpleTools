using System.Collections.Generic;
using UnityEngine;

namespace Aspect_Ratio
{
    public enum AspectRatioUpdatePhase
    {
        Layout = 0,
        CameraFirst = 1,
        Align = 2,
        Position = 3,
        Size = 4,
        /// <summary>Viewport aligners after rough camera, before final camera post-fit.</summary>
        AlignPost = 5,
        CameraPost = 6,
        Fit = 7,
        Late = 8,
    }

    public readonly struct AspectRatioUpdateStep
    {
        public AspectRatioUpdateStep(AspectRatioUpdatePhase phase, int order)
        {
            Phase = phase;
            Order = order;
        }

        public AspectRatioUpdatePhase Phase { get; }
        public int Order { get; }
    }

    public abstract class AspectRatioChanger : MonoBehaviour
    {
        public static AspectRatioUpdatePhase CurrentPhase { get; internal set; }

        [SerializeField] protected AspectRatioUpdatePhase updatePhase = AspectRatioUpdatePhase.Align;
        [SerializeField] protected int updateOrder;

        public virtual AspectRatioUpdatePhase Phase => updatePhase;
        public virtual int Order => updateOrder;

        public virtual IEnumerable<AspectRatioUpdateStep> GetUpdateSteps()
        {
            yield return new AspectRatioUpdateStep(Phase, Order);
        }

        protected virtual void Awake()
        {
            AspectRatioManager.Register(this);
        }

        protected virtual void OnDestroy()
        {
            AspectRatioManager.Unregister(this);
        }

        public abstract void UpdateAspectRatio(Camera camera);
    }
}
