using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public class CopyPosition : MonoBehaviour
    {
        [SerializeField] private Transform _target;

        private void Update()
        {
            transform.position = _target.position;
        }
    }
}
