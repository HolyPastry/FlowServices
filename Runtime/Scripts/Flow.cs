
using System;
using Eflatun.SceneReference;
using UnityEngine;

namespace Bakery
{
    public static class Flow
    {
        #region Public API

        public static class Events
        {
            public static Action OnEndSetup = delegate { };
            public static Action OnEndScenesLoading = delegate { };
            public static Action OnSceneUnloading = delegate { };
            public static Action OnLoadingStarted = delegate { };
            public static Action<float> OnLoadingProgress = delegate { };

        }

        public static Func<IFlowManager> Manager = UnregisterManager;

        public static Func<IFlowVisualTransition> Visuals = UnregisterVisual;



        #endregion



        #region Unregistering Functions

        internal static IFlowVisualTransition UnregisterVisual()
        {
            Debug.LogWarning("No Flow Visual registered. using Mock");
            _cachedVisualMock ??= new FlowVisualMock();
            Visuals = () => _cachedVisualMock;
            return _cachedVisualMock;
        }

        internal static IFlowManager UnregisterManager()
        {
            Debug.LogWarning("No Flow Manager registered. using Mock");
            _cachedManagerMock ??= new FlowMock();
            Manager = () => _cachedManagerMock;
            return _cachedManagerMock;
        }
        #endregion

        #region Mock Cache

        private static IFlowVisualTransition _cachedVisualMock;
        private static IFlowManager _cachedManagerMock;

        #endregion

        #region Mocks

        internal class FlowMock : IFlowManager
        {
            public bool Enabled => false;
            public SceneReference NextScene => null;
            public SceneReference CurrentScene => null;
            public WaitUntil WaitUntilReady => new(() => true);
            public WaitUntil WaitUntilEndOfSetup => new(() => true);

            public float FadeTime => throw new NotImplementedException();

            public void LoadNextScene() { }
            public void LoadScene(SceneReference sceneReference) { }
            public void SetDefaultFadeTime(float duration) { }
            public void RegisterSetup(SceneSetup setup) { }
        };
        internal class FlowVisualMock : IFlowVisualTransition
        {
            public bool Enabled => false;
            public float FadeDuration { get => 0f; set { } }
            public Coroutine FadeIn(float duration) => null;
            public Coroutine FadeOut(float duration) => null;
        }
        #endregion

        //Cleaning stuff in case cowboys are fast reloading in the editor
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void ResetStatics()
        {
            Events.OnEndSetup = delegate { };
            Events.OnEndScenesLoading = delegate { };
            Events.OnSceneUnloading = delegate { };
            Manager = UnregisterManager;
            Visuals = UnregisterVisual;

#if UNITY_EDITOR
            Debug.Log("[Flow] Static fields reset (domain reload skipped)");
#endif
        }
    }
}