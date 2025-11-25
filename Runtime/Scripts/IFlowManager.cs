using Eflatun.SceneReference;
using UnityEngine;

namespace Bakery
{
    public interface IFlowManager
    {
        bool Enabled { get; } //=> false; }
        SceneReference NextScene { get; }// => null; }
        SceneReference CurrentScene { get; }// => null; }
        WaitUntil WaitUntilReady { get; }// => new(() => true); }
        WaitUntil WaitUntilEndOfSetup { get; }// => new(() => true); }
        void LoadScene(SceneReference sceneReference);// { }
        void LoadNextScene();// { }
        void RegisterSetup(SceneSetup setup);// { }

    }
}
