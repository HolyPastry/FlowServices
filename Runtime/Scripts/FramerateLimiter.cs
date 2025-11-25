using UnityEngine;


namespace Bakery
{
    public class FramerateLimiter : MonoBehaviour
    {
        [SerializeField] private int _targetFrameRate = 30;

        void Awake()
        {
            Application.targetFrameRate = _targetFrameRate;
        }
    }
}