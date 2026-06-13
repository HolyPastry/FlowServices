using System.Collections;
using UnityEngine;


namespace Bakery
{
    public class FlowServiceTest : MonoBehaviour
    {
        // void OnEnable()
        // {
        //     Flow.Events.OnEndScenesLoading += OnFinishLoading;
        //     Flow.Events.OnEndSetup += OnEndSetup;
        //     Flow.Events.OnSceneUnloading += OnSceneUnloading;
        //     Flow.Events.OnLoadingStarted += OnLoadingStarted;
        //     Flow.Events.OnLoadingProgress += OnLoadingProgess;

        // }
        // void OnDisable()
        // {
        //     Flow.Events.OnEndScenesLoading -= OnFinishLoading;
        //     Flow.Events.OnEndSetup -= OnEndSetup;
        //     Flow.Events.OnSceneUnloading -= OnSceneUnloading;
        //     Flow.Events.OnLoadingStarted -= OnLoadingStarted;
        //     Flow.Events.OnLoadingProgress -= OnLoadingProgess;

        // }
        // void OnLoadingProgess(float progress)
        // {
        //     Debug.Log($"Loading progress: {progress}");
        // }

        // void OnLoadingStarted()
        // {
        //     Debug.Log("Loading started!");
        // }

        // void OnFinishLoading()
        // {
        //     Debug.Log("Finished loading!");
        // }
        // void OnEndSetup()
        // {
        //     Debug.Log("End of setup!");
        // }
        // void OnSceneUnloading()
        // {
        //     Debug.Log("Scene unloading!");
        // }

        // IEnumerator Start()
        // {
        //     Debug.Log("FlowServiceTest:Scene Start Loading!");
        //     yield return Flow.Manager().WaitUntilReady;
        //     Debug.Log("FlowServiceTest: Manager is ready!");
        //     yield return Flow.Manager().WaitUntilEndOfSetup;
        //     Debug.Log("FlowServiceTest: Setup ended!");
        //     yield return new WaitForSeconds(10f);
        //     Flow.Manager().LoadNextScene();

        // }

    }
}