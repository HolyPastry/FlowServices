using UnityEngine;


namespace Holypastry.Bakery.Flow
{
    public class FlowServiceTest : MonoBehaviour
    {
        void OnEnable()
        {
            FlowEvents.OnTransitionOut += OnTransitionOut;
            FlowEvents.OnTransitionIn += OnTransitionIn;
            FlowEvents.OnFadeOut += OnFadeOut;
            FlowEvents.OnFadeIn += OnFadeIn;
            FlowEvents.OnFinishLoading += OnFinishLoading;

        }

        void OnDisable()
        {
            FlowEvents.OnTransitionOut -= OnTransitionOut;
            FlowEvents.OnTransitionIn -= OnTransitionIn;
            FlowEvents.OnFadeOut -= OnFadeOut;
            FlowEvents.OnFadeIn -= OnFadeIn;
            FlowEvents.OnFinishLoading -= OnFinishLoading;
        }

        void OnTransitionOut(SceneTransition transition)
        {
            Debug.Log("Transitioning out...");
        }

        void OnTransitionIn(SceneTransition transition)
        {
            Debug.Log("Transitioning in...");
        }

        void OnFadeOut(SceneTransition transition)
        {
            Debug.Log("Fading out...");
        }

        void OnFadeIn(SceneTransition transition)
        {
            Debug.Log("Fading in...");
        }

        void OnFinishLoading()
        {
            Debug.Log("Finished loading!");
        }

    }
}