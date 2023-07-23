﻿using Network;
using UnityEngine;
namespace Mechanics
{
    public class PlanetOrbit : NetworkMovableObject
    {
        protected override float _speed => _smoothTime;
        [SerializeField] private Transform _aroundPoint;
        [SerializeField] private float _smoothTime = .3f;
        [SerializeField] private float _circleInSecond = 1f;
        [SerializeField] private float _offsetSin = 1;
        [SerializeField] private float _offsetCos = 1;
        [SerializeField] private float _rotationSpeed;
        private float _dist;
        private float _currentAng;
        private Vector3 _currentPositionSmoothVelocity;
        private float _currentRotationAngle;
        private const float _circleRadians = Mathf.PI * 2; 
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if (IsServer)
            {
                _dist = (transform.position - _aroundPoint.position).magnitude;
                //Debug.Log("_dist= " + _dist);
            }
            //Initiate(UpdatePhase.FixedUpdate);
        }
        protected override void HasAuthorityMovement()
        {
            //Debug.Log("HasAuthorityMovement before " + gameObject.name);
            if (!IsServer)
            {
                return;
            }
            //Debug.Log("HasAuthorityMovement after " + gameObject.name);
            var p = _aroundPoint.position;
            p.x += Mathf.Sin(_currentAng) * _dist * _offsetSin;
            p.z += Mathf.Cos(_currentAng) * _dist * _offsetCos;
            transform.position = p;
            _currentRotationAngle += Time.deltaTime * _rotationSpeed;
            _currentRotationAngle = Mathf.Clamp(_currentRotationAngle, 0, 361);
            if (_currentRotationAngle >= 360)
            {
                _currentRotationAngle = 0;
            }
            transform.rotation = Quaternion.AngleAxis(_currentRotationAngle,
            transform.up);
            _currentAng += _circleRadians * _circleInSecond * Time.deltaTime;
            SendToServer();
        }
        protected override void SendToServer()
        {
            _serverPosition.Value = transform.position;
            _serverEuler.Value = transform.eulerAngles;
        }
        protected override void FromServerUpdate()
        {
            if (!IsClient)
            {
                return;
            }
            transform.position = Vector3.SmoothDamp(transform.position,_serverPosition.Value,
                ref _currentPositionSmoothVelocity, _speed);
            transform.rotation = Quaternion.Euler(_serverEuler.Value);
        }
    }
}
