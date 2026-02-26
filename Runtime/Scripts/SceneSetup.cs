
using System;
using System.Collections;
using UnityEngine;

namespace Bakery
{
    public class SceneSetup : MonoBehaviour
    {
        void OnEnable()
        {
            Flow.Events.OnEndScenesLoading += Register;
        }
        void OnDisable()
        {
            Flow.Events.OnEndScenesLoading -= Register;
        }

        private void Register()
        {
            Flow.Manager().RegisterSetup(this);
        }

        public IEnumerator Routine()
        {
            yield return Flow.Manager().WaitUntilReady;

            var scripts = GetComponentsInChildren<SceneSetupScript>();

            foreach (var script in scripts)
            {
                if (script.Disabled) continue;
                if (script.RunInEditModeOnly && !Application.isEditor) continue;
                yield return script.Routine();
            }

        }
    }
}
