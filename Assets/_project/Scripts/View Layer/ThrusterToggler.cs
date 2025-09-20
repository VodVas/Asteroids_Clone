using UnityEngine;

namespace AsteroidsClone
{
    public class ThrusterToggler : MonoBehaviour
    {
        [SerializeField] private GameObject thruster;

        public void SetThrusterActive(bool active)
        {
            if (thruster != null)
                thruster.SetActive(active);
        }
    }
}