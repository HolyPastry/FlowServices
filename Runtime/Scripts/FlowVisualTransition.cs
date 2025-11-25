
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Bakery
{

    public class FlowVisualTransition : MonoBehaviour, IFlowVisualTransition
    {
        public bool Enabled => true;
        public float FadeDuration { get => _fadeDuration; set => _fadeDuration = value; }

        [SerializeField] private Image _screen;
        [SerializeField] private LoadingScreen _loadingScreenPrefab;
        [SerializeField]
        private Color _screenColor = new(0.14117647058f,
                                        0.11764705882f,
                                        0.11764705882f,
                                        1);

        [SerializeField] private float _fadeDuration;
        private static LoadingScreen _loadingScreenInstance;
        private Coroutine _routine;
        private float _loadStartTime;

        void Awake()
        {
            _screen.color = new Color(_screenColor.r, _screenColor.g, _screenColor.b, 1);
            Flow.Visuals = () => this;
        }

        void OnEnable()
        {
            Flow.Events.OnEndSetup += OnSceneReady;
            Flow.Events.OnSceneUnloading += OnSceneUnloading;
            Flow.Events.OnLoadingStarted += OnLoadingStarted;
            Flow.Events.OnLoadingProgress += OnLoadingProgress;
        }

        void OnDisable()
        {
            Flow.Events.OnEndSetup -= OnSceneReady;
            Flow.Events.OnSceneUnloading -= OnSceneUnloading;
            Flow.Events.OnLoadingStarted -= OnLoadingStarted;
            Flow.Events.OnLoadingProgress -= OnLoadingProgress;
        }

        private void OnLoadingProgress(float progress)
        {
            if (_loadingScreenInstance != null)
            {
                _loadingScreenInstance.Progress = progress;
            }
        }

        private void OnLoadingStarted()
        {
            if (_loadingScreenPrefab != null)
            {
                _loadingScreenInstance = Instantiate(_loadingScreenPrefab);
                FadeIn(_fadeDuration);
                _loadStartTime = Time.time;
            }
        }

        void OnDestroy()
        {
            Flow.Visuals = Flow.UnregisterVisual;
        }

        private void OnSceneReady()
        {
            if (_routine == null)
            {
                _routine = StartCoroutine(TransitionInRoutine());
            }
        }

        private IEnumerator TransitionInRoutine()
        {

            if (_loadingScreenInstance != null)
            {
                float minLoadingTime = _loadingScreenInstance.MinLoadingTime;
                minLoadingTime -= Time.time - _loadStartTime;
                if (minLoadingTime < 0) minLoadingTime = 0;
                yield return new WaitForSeconds(minLoadingTime);
                yield return FadeOut(_fadeDuration);
                Destroy(_loadingScreenInstance.gameObject);
                _loadingScreenInstance = null;
            }
            FadeIn(_fadeDuration);
        }

        private void OnSceneUnloading()
            => StartCoroutine(TransitionOutRoutine());
        private IEnumerator TransitionOutRoutine()
        {
            yield return FadeOut(_fadeDuration);
        }

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

        public Coroutine FadeIn(float duration)
        {

            _fadeDuration = duration;
            return StartCoroutine(FadeRoutine(0));
        }

        public Coroutine FadeOut(float duration)
        {
            _fadeDuration = duration;
            return StartCoroutine(FadeRoutine(1));
        }
    }
}