using UnityEngine;

namespace AsteroidsClone
{
    public class ThrusterToggler : MonoBehaviour
    {
        [SerializeField] private GameObject _thruster;

        public void SetThrusterActive(bool active)
        {
            if (_thruster != null)
                _thruster.SetActive(active);
        }
    }
}