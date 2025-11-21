using Eflatun.SceneReference;
using UnityEngine;

namespace Bakery
{
    public interface IFlowManager
    {
        bool Enabled { get; } //=> false; }
        float DefaultFadeTime { get; }
        SceneReference NextScene { get; }// => null; }
        SceneReference CurrentScene { get; }// => null; }
        WaitUntil WaitUntilReady { get; }// => new(() => true); }
        WaitUntil WaitUntilEndOfSetup { get; }// => new(() => true); }
        void LoadScene(SceneReference sceneReference);// { }
        void LoadNextScene();// { }
        void SetDefaultFadeTime(float duration);// { }




        //     FlowServices.FadeIn = VisualTransitionIn;
        //     FlowServices.FadeOut = VisualTransitionOut;
        //     FlowServices.IsCustomTransitionOut = () => _customTransitionOut;

        //     EndSetup = () => _setupEnded = true;
    }
}
