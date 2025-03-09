using System;
using Holypastry.Bakery.Flow;

/// <summary>
/// Static class containing events related to flow transitions in the game.
/// </summary>
public static partial class FlowEvents
{
    /// <summary>
    /// Event triggered when a scene transition starts to transition out.
    /// </summary>
    public static Action<SceneTransition> OnTransitionOut;

    /// <summary>
    /// Event triggered when a scene transition starts to transition in.
    /// </summary>
    public static Action<SceneTransition> OnTransitionIn;

    /// <summary>
    /// Event triggered when a scene transition starts to fade out.
    /// </summary>
    public static Action<SceneTransition> OnFadeOut;

    /// <summary>
    /// Event triggered when a scene transition starts to fade in.
    /// </summary>
    public static Action<SceneTransition> OnFadeIn;

    /// <summary>
    /// Event triggered when the loading process is finished.
    /// </summary>
    public static Action OnFinishLoading;

    /// <summary>
    /// Event triggered when the game is over.
    /// </summary>
    public static Action OnGameOver;

    /// <summary>
    /// Event triggered when entering a transition zone.
    /// </summary>
    public static Action<string> OnEnteredTransitionZone;

    /// <summary>
    /// Event triggered when exiting a transition zone.
    /// </summary>
    public static Action OnExitedTransitionZone;
}





