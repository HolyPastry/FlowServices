using UnityEngine;
using UnityEngine.UI;

namespace Bakery
{
    public class LoadingScreen : MonoBehaviour
    {

        [SerializeField] private GameObject _content;

        void Start()
        {
            _content.SetActive(true);
        }
        internal float MinLoadingTime => 3f;
        internal float Progress { get; set; } = 0f;
    }
}