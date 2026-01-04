

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Eflatun.SceneReference;

using UnityEngine;
using UnityEngine.SceneManagement;


namespace Bakery
{
    public class FlowManager : MonoBehaviour, IFlowManager
    {

        [SerializeField] private SceneReference _nextSceneToLoad;

        [Tooltip("First scene listed here becomes\nthe active scene after loading")]

        [SerializeField] private List<SceneReference> _scenesToLoad = new();

        public bool Enabled => true;
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

        void Awake()
            => Flow.Manager = () => this;

        void OnDestroy()
            => Flow.Manager = Flow.UnregisterManager;

        void Start()
            => StartCoroutine(LoadExtraScenesRoutine(_scenesToLoad));


        private IEnumerator LoadExtraScenesRoutine(List<SceneReference> sceneList)
        {
            Flow.Events.OnLoadingStarted.Invoke();
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
            Flow.Events.OnLoadingProgress.Invoke(asyncOperations.Sum(x => x.progress) / asyncOperations.Count);

            yield return new WaitUntil(() => asyncOperations.TrueForAll(x => x.isDone));

            SetActiveScene(sceneList[0]);
            StartCoroutine(FinalizeLoadingRoutine());
        }

        private IEnumerator FinalizeLoadingRoutine()
        {

            Flow.Events.OnEndScenesLoading.Invoke();
            _isReady = true;
            if (_sceneSetup != null)
                yield return StartCoroutine(_sceneSetup.Routine());
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
            if (Flow.Visuals().Enabled)
                yield return new WaitForSeconds(Flow.Visuals().FadeDuration);
            SceneManager.LoadSceneAsync(sceneReference.BuildIndex, LoadSceneMode.Single);
        }

        public void RegisterSetup(SceneSetup setup)
        {
            if (_sceneSetup != null)
            {
                Debug.LogWarning("A SceneSetup is already registered. Overriding with the new one.");
            }
            _sceneSetup = setup;
        }
    }
}