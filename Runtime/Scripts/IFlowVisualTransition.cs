using UnityEngine;

namespace Bakery
{
    public interface IFlowVisualTransition
    {
        bool Enabled { get; }
        float FadeDuration { get; set; }
        Coroutine FadeIn(float duration);
        Coroutine FadeOut(float duration);
    }
}
