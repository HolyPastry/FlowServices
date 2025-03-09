
using System.Collections;
using UnityEngine;


namespace Holypastry.Bakery.Flow
{
    public class Service : MonoBehaviour
    {
        protected bool _isReady;
        public WaitUntil WaitUntilReady => new(() => _isReady);

        protected virtual IEnumerator Start()
        {
            yield return FlowServices.WaitUntilReady();
            _isReady = true;
        }

    }
}