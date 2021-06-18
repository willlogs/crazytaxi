using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

namespace Car
{
    public class CarController : MonoBehaviour
    {
        public UnityEvent onStartAcc, onStopAcc, onStartTurn, onStopTurn, onStartDrifting, onStopDrifting;

        [SerializeField] private Rigidbody _rigidBody;
        [SerializeField] private float _acceleration = 4, _rotationDuration = 0.4f, _speedCap = 20;
        [SerializeField] private Camera _cam;

        private Vector3 _lastPosition;
        private bool _hasLastPosition, _isTurning, _isMoving, _isDrifting;
        private Vector3 _newDirection;

        private Tweener _rotationTween;

        private void Update()
        {
            CheckInput();
            Rotate();
            CheckDrifting();
            LimitateVelocity();
        }

        private void CheckDrifting()
        {
            Vector3 sampleVel = _rigidBody.velocity;
            sampleVel.y = 0;
            if (sampleVel.magnitude > 3)
            {
                Vector3 directionDiff = sampleVel.normalized + transform.forward;
                float degree = Mathf.Atan2(directionDiff.y, directionDiff.x) * Mathf.Rad2Deg;
                print(degree);
                if (Mathf.Abs(degree) > 10)
                {
                    StartDrifting();
                }
                else
                {
                    StopDrifting();
                }
            }
        }

        private void LimitateVelocity()
        {
            if (_rigidBody.velocity.magnitude > _speedCap)
            {
                _rigidBody.velocity = _rigidBody.velocity.normalized * _speedCap;
            }
        }

        private void Rotate()
        {
            Vector3 vel = _newDirection;
            vel.y = 0;
            if (vel.magnitude > 0.1f)
            {
                _rotationTween?.Kill();
                _rotationTween = DOTween.To(() => transform.forward, (x) => { transform.forward = x; }, -vel.normalized, _rotationDuration);
            }
        }

        private void CheckInput()
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

                    Vector3 newDiff = transform.forward - _newDirection;
                    float diffDegree = Mathf.Atan2(newDiff.y, newDiff.x) * Mathf.Rad2Deg;
                    if (Mathf.Abs(diffDegree) > 45)
                    {
                        StartTurning();
                    }
                    else
                    {
                        StopTurning();
                    }

                    _rigidBody.velocity += transform.forward * -_acceleration * Time.deltaTime;
                }
                else
                {
                    if (!_isMoving)
                    {
                        _isMoving = true;
                        _lastPosition = _cam.ScreenToViewportPoint(Input.mousePosition);
                        _hasLastPosition = true;
                        StartMoving();
                    }
                }
            }
            else
            {
                _hasLastPosition = false;
                StopMoving();
                StopTurning();
            }
        }

        private void StartDrifting()
        {
            if (!_isDrifting)
            {
                onStartDrifting?.Invoke();
                _isDrifting = true;
            }
        }

        private void StopDrifting()
        {
            if (_isDrifting)
            {
                onStopDrifting?.Invoke();
                _isDrifting = false;
            }
        }

        private void StopMoving()
        {
            if (_isMoving)
            {
                _isMoving = false;
                onStartAcc?.Invoke();
            }
        }

        private void StartMoving()
        {
            onStartAcc?.Invoke();
        }

        private void StartTurning()
        {
            if (!_isTurning)
            {
                _isTurning = true;
                onStartTurn?.Invoke();
            }
        }

        private void StopTurning()
        {
            if (_isTurning)
            {
                _isTurning = false;
                onStopTurn?.Invoke();
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