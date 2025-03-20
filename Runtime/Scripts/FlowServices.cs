using System;
using Holypastry.Bakery.Flow;
using UnityEngine;

/// <summary>
/// Provides various services related to scene management and transitions.
/// </summary>
public static partial class FlowServices
{
    /// <summary>
    /// Action to load a scene with the provided <see cref="SceneData"/>.
    /// </summary>
    public static Action<SceneData> LoadScene = (sceneData) => { };

    /// <summary>
    /// Function to check if the service is enabled.
    /// </summary>
    /// <returns>True if the service is enabled, otherwise false.</returns>
    public static Func<bool> IsEnabled = () => false;

    /// <summary>
    /// Function to wait until the service is ready.
    /// </summary>
    /// <returns>A <see cref="WaitUntil"/> object that waits until the service is ready.</returns>
    public static Func<WaitUntil> WaitUntilReady = () => new WaitUntil(() => true);

    /// <summary>
    /// Internal action to load the next scene.
    /// </summary>
    public static Action LoadNextScene = () => { };

    /// <summary>
    /// Internal function to get the name of the next scene.
    /// </summary>
    /// <returns>The name of the next scene.</returns>
    public static Func<string> NextSceneName = () => string.Empty;

    /// <summary>
    /// Internal function to get the current scene data.
    /// </summary>
    /// <returns>The current <see cref="SceneData"/>.</returns>
    public static Func<SceneData> GetCurrentScene = () => null;

    /// <summary>
    /// Internal function to get the scene data from a given scene name.
    /// </summary>
    /// <param name="name">The name of the scene.</param>
    /// <returns>The <see cref="SceneData"/> for the given scene name.</returns>
    public static Func<string, SceneData> GetSceneFromName = (name) => null;

    /// <summary>
    /// Action to handle visual transition out with the provided <see cref="SceneTransition"/>.
    /// </summary>
    public static Action<SceneTransition> FadeOut = (transition) => { };

    /// <summary>
    /// Action to handle visual transition in with the provided <see cref="SceneTransition"/>.
    /// </summary>
    public static Action<SceneTransition> FadeIn = (transition) => { };

    /// <summary>
    /// Function to check if a custom transition out is enabled.
    /// </summary>
    /// <returns>True if a custom transition out is enabled, otherwise false.</returns>
    public static Func<bool> IsCustomTransitionOut = () => false;
}



