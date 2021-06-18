using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class OffsetFollower : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _duration;

    private Tweener _tweener;
    private Vector3 _offset;

    private void Start()
    {
        _offset = transform.position - _target.position;
    }

    private void Update()
    {
        _tweener?.Kill();
        _tweener = transform.DOMove(_target.position + _offset, _duration);
    }
}
