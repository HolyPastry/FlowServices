
using System.Collections;
using UnityEngine;

namespace Bakery
{
    public class SceneSetup : MonoBehaviour
    {
        public IEnumerator Routine()
        {
            yield return Flow.Manager().WaitUntilReady;

            var scripts = GetComponentsInChildren<SceneSetupScript>();

            foreach (var script in scripts)
            {
                if (script.Disabled) continue;
                yield return script.Routine();
            }

        }
    }
}
