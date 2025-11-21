
using System;
using System.Collections;
using System.Collections.Generic;
using Eflatun.SceneReference;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace Bakery
{
    public class FlowManager : MonoBehaviour, IFlowManager
    {
        [Header("Settings")]
        [SerializeField] private int _targetFrameRate = 30;

        [Space]

        [Header("Transitions")]
        [SerializeField] private float _defaultFadeDuration = 1f;
        [Space]

        [Header("Scenes")]

        [SerializeField] private SceneReference _nextSceneToLoad;
        [SerializeField] private List<SceneReference> _scenesToLoad = new();

        public bool Enabled => true;
        public float DefaultFadeTime => _defaultFadeDuration;

        public SceneReference NextScene => _nextSceneToLoad;

        public SceneReference CurrentScene => SceneManager.GetActiveScene().IsValid()
                                                    ? new SceneReference(SceneManager.GetActiveScene().name)
                                                    : null;

        public WaitUntil WaitUntilReady => new(() => _isReady);

        public WaitUntil WaitUntilEndOfSetup => new(() => _setupEnded);

        public IEnumerable<SceneReference> AdditionalScenesToLoad => _scenesToLoad;

        private bool _isReady = false;
        private bool _setupEnded = false;
        private SceneSetup _sceneSetup;

        void Awake() =>
            _sceneSetup = FindObjectOfType<SceneSetup>();

        void OnEnable() =>
            Flow.Manager = () => this;

        void OnDisable() =>
            Flow.Manager = Flow.UnregisterManager;

        IEnumerator Start()
        {
            Application.targetFrameRate = _targetFrameRate;
            StartCoroutine(LoadExtraScenesRoutine(_scenesToLoad));
            yield break;
        }

        private IEnumerator LoadExtraScenesRoutine(List<SceneReference> sceneList)
        {

            if (sceneList.Count == 0)
            {
                StartCoroutine(FinalizeLoadingRoutine());
                yield break;
            }

            List<AsyncOperation> asyncOperations = new();
            for (int i = 0; i < sceneList.Count; i++)
            {
                var scene = SceneManager.GetSceneByName(sceneList[i].Name);
                if (scene.buildIndex == -1)
                {
                    var asyncOperation =
                        SceneManager.LoadSceneAsync(sceneList[i].BuildIndex
                                                , LoadSceneMode.Additive);
                    asyncOperations.Add(asyncOperation);
                }
            }
            yield return new WaitUntil(() => asyncOperations.TrueForAll(x => x.isDone));

            SetActiveScene(sceneList[0]);
            StartCoroutine(FinalizeLoadingRoutine());
        }

        private IEnumerator FinalizeLoadingRoutine()
        {
            Flow.Events.OnEndScenesLoading.Invoke();
            _isReady = true;
            if (_sceneSetup != null)
                yield return _sceneSetup.Routine();
            _setupEnded = true;
            Flow.Events.OnEndSetup.Invoke();
            yield break;
        }

        private void SetActiveScene(SceneReference sceneReference)
        {
            var scene = SceneManager.GetSceneByName(sceneReference.Name);
            if (scene.buildIndex != -1)
                SceneManager.SetActiveScene(scene);
        }

        public void LoadScene(SceneReference sceneReference)
        => StartCoroutine(LoadSceneRoutine(sceneReference));

        public void LoadNextScene()
        => StartCoroutine(LoadSceneRoutine(_nextSceneToLoad));

        private IEnumerator LoadSceneRoutine(SceneReference sceneReference)
        {
            Flow.Events.OnSceneUnloading.Invoke();
            yield return new WaitForSeconds(_defaultFadeDuration);
            SceneManager.LoadSceneAsync(sceneReference.BuildIndex, LoadSceneMode.Single);
        }

        public void SetDefaultFadeTime(float duration)
        {
            _defaultFadeDuration = duration;
        }
    }
}