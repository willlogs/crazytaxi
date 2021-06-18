using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utils.Events
{
    public class ParticleController : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _particleSystem;

        public void StartIt()
        {
            _particleSystem.Play();
        }

        public void Stop()
        {
            _particleSystem.Stop();
        }
    }
}