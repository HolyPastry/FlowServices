
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
        }

        public static Func<IFlowManager> Manager = UnregisterManager;

        public static Func<IFlowVisuals> Visuals = UnregisterVisual;

        #endregion



        #region Unregistering Functions

        internal static IFlowVisuals UnregisterVisual()
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

        private static IFlowVisuals _cachedVisualMock;
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

            public float DefaultFadeTime => throw new NotImplementedException();

            public void LoadNextScene() { }
            public void LoadScene(SceneReference sceneReference) { }
            public void SetDefaultFadeTime(float duration) { }
        };
        internal class FlowVisualMock : IFlowVisuals
        {
            public void FadeIn(float duration) { }
            public void FadeOut(float duration) { }
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