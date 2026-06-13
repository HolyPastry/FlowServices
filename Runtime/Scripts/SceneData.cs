
using Eflatun.SceneReference;
using UnityEngine;


namespace Bakery.Flow
{
    [CreateAssetMenu(fileName = "SceneData", menuName = "Bakery/Flow/SceneData")]
    public class SceneData : ScriptableObject
    {
        public string DisplayName;
        public bool isMain = false;

        public SceneReference SceneReference;
    }
}
