using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Game
{
    public class Passenger : MonoBehaviour
    {
        public UnityEvent onCollide;

        [SerializeField] private Collider _collider;
        [SerializeField] private Transform _particle;

        public void Collide()
        {
            onCollide?.Invoke();
            _collider.enabled = false;
            _particle.parent = null;
            Destroy(_particle.gameObject, 1);
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }
    }
}