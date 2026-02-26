
using System.Collections;
using UnityEngine;
namespace Bakery
{
    public abstract class SceneSetupScript : MonoBehaviour
    {
        [SerializeField] private bool _disable = false;
        [SerializeField] private bool _runInEditModeOnly = false;
        public bool Disabled => _disable;
        public bool RunInEditModeOnly => _runInEditModeOnly;
        public abstract IEnumerator Routine();
    }
}
