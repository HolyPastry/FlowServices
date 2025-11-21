


using System.Collections;
using KBCore.Refs;
using UnityEngine;
using UnityEngine.UI;

namespace Bakery
{

    public class TransitionScreen : MonoBehaviour, IFlowVisuals
    {
        [SerializeField, Child] private Image _screen;

        [SerializeField]
        private Color _screenColor = new(0.14117647058f,
                                        0.11764705882f,
                                        0.11764705882f,
                                        1);
        private float _fadeDuration;

        void OnEnable()
        {
            Flow.Visuals = () => this;
            Flow.Events.OnEndSetup += SceneFadeIn;
            Flow.Events.OnSceneUnloading += SceneFadeOut;
        }

        void OnDisable()
        {
            Flow.Visuals = Flow.UnregisterVisual;
            Flow.Events.OnEndSetup -= SceneFadeIn;
            Flow.Events.OnSceneUnloading -= SceneFadeOut;
        }

        private void SceneFadeIn() =>
            FadeIn(Flow.Manager().DefaultFadeTime);

        private void SceneFadeOut() =>
            FadeOut(Flow.Manager().DefaultFadeTime);


        void Start()
        {
            _screen.color = new Color(_screenColor.r, _screenColor.g, _screenColor.b, 1);
            _screen.enabled = Flow.Manager().Enabled;
        }

        private IEnumerator FadeRoutine(float v2)
        {
            float timePerc = 0;
            float v1 = _screen.color.a;
            while (true)
            {
                _screen.color = new Color(_screenColor.r,
                                         _screenColor.g,
                                         _screenColor.b,
                                        Mathf.Lerp(v1, v2, timePerc));
                timePerc += Time.deltaTime / _fadeDuration;
                yield return null;
                if (timePerc > 1) break;
            }
            _screen.color = new Color(_screenColor.r,
                                         _screenColor.g,
                                         _screenColor.b,
                                         v2);
        }

        public void FadeIn(float duration)
        {
            _fadeDuration = duration;
            StartCoroutine(FadeRoutine(0));
        }

        public void FadeOut(float duration)
        {
            _fadeDuration = duration;
            StartCoroutine(FadeRoutine(1));
        }
    }
}