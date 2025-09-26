
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace Holypastry.Bakery.Flow
{
    public class FlowManager : Service
    {
        [Header("Settings")]
        [SerializeField] private int _targetFrameRate = 30;

        [Space]

        [Header("Transitions")]
        [SerializeField] private SceneTransition _preTransition;
        [SerializeField] private SceneTransition _postTransition;
        [SerializeField] private bool _customTransitionIn = false;
        [SerializeField] private bool _customTransitionOut = false;

        [Space]

        [Header("Scenes")]
        [SerializeField] private string _sceneDataFolderName = "Scenes";
        [SerializeField] private SceneData _nextSceneToLoad;
        [SerializeField] private List<Service> _servicesToLoadBeforeFadeIn = new();
        public List<SceneData> AdditionalScenesToLoad = new();
        private DataCollection<SceneData> _collection;

        private bool _isInTransitionZone = false;
        private Coroutine _routine;

        internal static Action EndSetup = delegate { };
        private bool _setupEnded;

        void Awake()
        {
            _collection = new DataCollection<SceneData>(_sceneDataFolderName);
        }

        void OnEnable()
        {
            FlowServices.IsEnabled = () => true;
            FlowServices.WaitUntilReady = () => WaitUntilReady;
            FlowServices.WaitUntilEndOfSetup = () => new WaitUntil(() => _setupEnded);

            FlowServices.LoadScene = TransitionOut;

            FlowServices.LoadNextScene = () => TransitionOut(_nextSceneToLoad);
            FlowServices.NextSceneName = GetNextSceneName;
            FlowServices.GetCurrentScene = GetCurrentScene;
            FlowServices.GetSceneFromName = GetSceneFromName;

            FlowServices.FadeIn = VisualTransitionIn;
            FlowServices.FadeOut = VisualTransitionOut;
            FlowServices.IsCustomTransitionOut = () => _customTransitionOut;

            EndSetup = () => _setupEnded = true;
        }

        void OnDisable()
        {
            FlowServices.IsEnabled = () => false;
            FlowServices.WaitUntilReady = () => new WaitUntil(() => true);
            FlowServices.WaitUntilEndOfSetup = () => new WaitUntil(() => true);
            FlowServices.LoadScene = delegate { };

            FlowServices.LoadNextScene = delegate { };
            FlowServices.NextSceneName = () => string.Empty;
            FlowServices.GetCurrentScene = () => null;
            FlowServices.GetSceneFromName = (name) => null;

            FlowServices.FadeIn = delegate { };
            FlowServices.FadeOut = delegate { };

            FlowServices.IsCustomTransitionOut = () => false;

            EndSetup = delegate { };
        }

        protected override IEnumerator Start()
        {
            Application.targetFrameRate = _targetFrameRate;
            TransitionIn(AdditionalScenesToLoad);
            yield break;
        }

        private void TryAndTransition(GameObject @object)
        {
            if (!_isInTransitionZone) return;

            TransitionOut(_nextSceneToLoad);
        }

        private void ExitTransitionZone()
        {
            _isInTransitionZone = false;
            FlowEvents.OnExitedTransitionZone.Invoke();
        }

        private void EnterTransitionZone(string label)
        {
            _isInTransitionZone = true;
            FlowEvents.OnEnteredTransitionZone.Invoke(label);
        }

        private void VisualTransitionIn(SceneTransition transition)
        {
            FlowEvents.OnFadeIn.Invoke(transition);
        }

        private void VisualTransitionOut(SceneTransition transition)
        {
            FlowEvents.OnFadeOut.Invoke(transition);
        }

        private string GetNextSceneName()
        {
            if (_nextSceneToLoad == null) return string.Empty;
            return _nextSceneToLoad.DisplayName;
        }




        private SceneData GetSceneFromName(string arg)
        {
            if (!_collection.Exists(x => x.name == arg))
            {
                Debug.LogWarning($"Scene {arg} not found in collection");
                return null;
            }
            return _collection.Find(x => x.name == arg);
        }

        private SceneData GetCurrentScene()
        {
            string sceneName = SceneManager.GetActiveScene().name;
            return GetSceneFromName(sceneName);
        }
        private void TransitionIn(List<SceneData> list)
        {
            if (_routine != null) return;
            _routine = StartCoroutine(LoadExtraScenesRoutine(list));
        }

        private void TransitionOut(SceneData scene)
        {
            if (_routine != null) return;
            FlowEvents.OnTransitionOut.Invoke(_postTransition);
            _routine = StartCoroutine(LoadSceneAsync(scene.name));
        }


        private IEnumerator LoadExtraScenesRoutine(List<SceneData> sceneList)
        {

            if (sceneList.Count == 0)
            {
                FlowEvents.OnFinishLoading.Invoke();
                _isReady = true;
                yield return new WaitForSeconds(_preTransition.TextDuration + _preTransition.FadeDuration);
                _routine = null;
                if (!_customTransitionIn)
                    FlowEvents.OnTransitionIn.Invoke(_preTransition);
                yield break;
            }

            List<AsyncOperation> asyncOperations = new();
            for (int i = 0; i < sceneList.Count; i++)
            {
                // if (sceneList[i].SceneReference.LoadedScene != null) continue;
                var scene = SceneManager.GetSceneByName(sceneList[i].name);
                if (scene.buildIndex == -1)
                {
                    var asyncOperation = SceneManager.LoadSceneAsync(sceneList[i].SceneReference.BuildIndex, LoadSceneMode.Additive);
                    // var asyncOperation = SceneManager.LoadSceneAsync(sceneList[i].name, LoadSceneMode.Additive);
                    asyncOperations.Add(asyncOperation);
                }
            }

            yield return new WaitUntil(() => asyncOperations.TrueForAll(x => x.isDone));

            SetActiveScene(sceneList);

            FlowEvents.OnFinishLoading.Invoke();
            _isReady = true;


            foreach (var service in _servicesToLoadBeforeFadeIn)
            {
                yield return service.WaitUntilReady;
            }
            if (!_customTransitionIn)
                FlowEvents.OnTransitionIn.Invoke(_preTransition);
            yield return new WaitForSeconds(_preTransition.TextDuration + _preTransition.FadeDuration);


            _routine = null;

        }

        private void SetActiveScene(List<SceneData> list)
        {

            foreach (var sceneData in list)
            {
                if (sceneData.isMain)
                {
                    var scene = SceneManager.GetSceneByName(sceneData.name);
                    if (scene.buildIndex == -1)
                        SceneManager.SetActiveScene(scene);
                    return;
                }
            }
        }

        private IEnumerator LoadSceneAsync(string sceneName)
        {
            yield return new WaitForSeconds(_postTransition.FadeDuration + _postTransition.TextDuration);
            var asyncOperation = SceneManager.LoadSceneAsync(sceneName);
            yield return new WaitUntil(() => asyncOperation.isDone);
            _routine = null;
        }


    }
}