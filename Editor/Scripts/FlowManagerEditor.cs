using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;


namespace Holypastry.Bakery.Flow
{
    [CustomEditor(typeof(FlowManager))]
    public class FlowManagerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Add Scenes"))
            {
                if (Application.isPlaying)
                {
                    Debug.LogWarning("Cannot add scenes while in play mode.");
                    return;
                }
                var flowManager = (FlowManager)target;

                foreach (var sceneData in flowManager.AdditionalScenesToLoad)
                {
                    var scene = GetSceneObject(sceneData.name);
                    if (scene == null) continue;
                    EditorSceneManager.OpenScene(scene.path, OpenSceneMode.Additive);
                }
            }
        }
        protected EditorBuildSettingsScene GetSceneObject(string sceneObjectName)
        {
            if (string.IsNullOrEmpty(sceneObjectName))
            {
                return null;
            }

            foreach (var editorScene in EditorBuildSettings.scenes)
            {
                if (editorScene.path.IndexOf(sceneObjectName) != -1)
                {
                    return editorScene;
                }
            }
            Debug.LogWarning("Scene [" + sceneObjectName + "] cannot be used. Add this scene to the 'Scenes in the Build' in build settings.");
            return null;
        }
    }

}
#endif