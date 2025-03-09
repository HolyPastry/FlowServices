
using System.Collections;
using UnityEngine;

namespace Holypastry.Bakery.Flow
{
    public class SceneSetup : MonoBehaviour
    {

        IEnumerator Start()
        {
            yield return FlowServices.WaitUntilReady();
            var scripts = GetComponentsInChildren<SceneSetupScript>();

            foreach (var script in scripts)
            {
                script.Run();
                yield return script.WaitUntilEnded;
            }

        }
    }
}
