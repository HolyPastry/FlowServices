
using System.Collections;
using UnityEngine;
namespace Holypastry.Bakery.Flow
{
    public abstract class SceneSetupScript : MonoBehaviour
    {
        [SerializeField] private bool _runInEditorOnly = false;
        [SerializeField] private bool _disable = false;
        public bool Disabled => _disable;
        public bool RunInEditorOnly => _runInEditorOnly;
        public abstract IEnumerator Routine();
    }
}
