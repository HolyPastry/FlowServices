
using System.Collections;
using UnityEngine;


namespace Holypastry.Bakery.Flow
{
    public class TestService : Service
    {
        protected override IEnumerator Start()
        {
            StartCoroutine(base.Start());
            yield return WaitUntilReady;
            Debug.Log("TestService is ready!");
        }
    }
}