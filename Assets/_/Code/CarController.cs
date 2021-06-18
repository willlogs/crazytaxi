using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

namespace Car
{
    public class CarController : MonoBehaviour
    {
        public UnityEvent onStartAcc, onStopAcc, onTurn, onStopTurn;

        [SerializeField] private Rigidbody _rigidBody;
        [SerializeField] private float _speed = 4, _rotationDuration = 0.4f;
        [SerializeField] private Camera _cam;

        private Vector3 _lastPosition;
        private bool _hasLastPosition;
        private Vector3 _newDirection;

        private Tweener _rotationTween;

        private void Update()
        {
            if (Input.GetMouseButton(0))
            {
                if (_hasLastPosition)
                {
                    Vector3 currPosition = _cam.ScreenToViewportPoint(Input.mousePosition);
                    Vector3 diff = (currPosition - _lastPosition);
                    diff.z = 0;
                    float degree = Mathf.Atan2(diff.x, diff.y) * Mathf.Rad2Deg;
                    _newDirection = Quaternion.Euler(0, degree, 0) * Vector3.forward;

                    _rigidBody.velocity = transform.forward * -_speed;
                }
                else
                {
                    _lastPosition = _cam.ScreenToViewportPoint(Input.mousePosition);
                    _hasLastPosition = true;
                }
            }
            else
            {
                _hasLastPosition = false;
            }

            Vector3 vel = _newDirection;
            vel.y = 0;
            if (vel.magnitude > 0.1f)
            {
                _rotationTween?.Kill();
                _rotationTween = DOTween.To(() => transform.forward, (x) => { transform.forward = x; }, -vel.normalized, _rotationDuration);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position - transform.forward);
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, transform.position + _newDirection.normalized);
        }
    }
}