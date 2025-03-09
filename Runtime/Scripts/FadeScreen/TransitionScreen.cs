

using System.Collections;
using KBCore.Refs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Holypastry.Bakery.Flow
{

    public class TransitionScreen : ValidatedMonoBehaviour
    {

        [SerializeField, Child] private Image _screen;
        [SerializeField, Child] private TextMeshProUGUI _text;

        [SerializeField]
        private Color _screenColor = new(0.14117647058f,
                                        0.11764705882f,
                                        0.11764705882f,
                                        1);
        private float _fadeDuration;
        private Color _colorClear;
        private Color _colorOpaque;

        void Awake()
        {
            _colorClear = new Color(_screenColor.r, _screenColor.g, _screenColor.b, 0);
            _colorOpaque = new Color(_screenColor.r, _screenColor.g, _screenColor.b, 1);
        }

        void OnEnable()
        {
            FlowEvents.OnTransitionOut += TransitionOut;
            FlowEvents.OnTransitionIn += TransitionIn;
            FlowEvents.OnFadeOut += TransitionOut;
            FlowEvents.OnFadeIn += TransitionIn;
        }

        void OnDisable()
        {
            FlowEvents.OnTransitionOut -= TransitionOut;
            FlowEvents.OnTransitionIn -= TransitionIn;
            FlowEvents.OnFadeOut -= TransitionOut;
            FlowEvents.OnFadeIn -= TransitionIn;
        }

        void Start()
        {
            _screen.color = _colorOpaque;
            _screen.enabled = true;
            if (!FlowServices.IsEnabled())
                _screen.enabled = false;
        }

        private void TransitionOut(SceneTransition transition)
        {
            StopAllCoroutines();
            _text.text = transition.Text;
            _fadeDuration = transition.FadeDuration;
            _screen.color = _colorClear;


            StartCoroutine(FadeRoutine(0, 1, 0, transition.TextDuration));



        }
        private void TransitionIn(SceneTransition transition)
        {
            StopAllCoroutines();
            _screen.color = _colorOpaque;
            _screen.enabled = true;

            _text.text = transition.Text;
            _fadeDuration = transition.FadeDuration;
            StartCoroutine(FadeRoutine(1, 0, transition.TextDuration, 0));


        }



        private IEnumerator FadeRoutine(int v1, int v2, float waitTimeBefore = 0, float waitTimeAfter = 0)
        {
            if (waitTimeBefore > 0) _text.enabled = true;

            yield return new WaitForSeconds(waitTimeBefore);
            _text.enabled = false;
            float timePerc = 0;
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
            if (waitTimeAfter > 0) _text.enabled = true;
            yield return new WaitForSeconds(waitTimeAfter);
            _text.enabled = false;
        }
    }
}