
using System.Collections;
using UnityEngine;
namespace Holypastry.Bakery.Flow
{
    public abstract class SceneSetupScript : MonoBehaviour
    {
        [SerializeField] private bool _disable = false;
        public WaitUntil WaitUntilEnded => new(() => _ended);
        private bool _ended;

        protected abstract IEnumerator Routine();

        protected void EndScript()
        {
            _ended = true;
        }

        public void Run()
        {
            if (_disable)
            {
                _ended = true;
                return;
            }
            _ended = false;
            StartCoroutine(Routine());
        }
    }
}
