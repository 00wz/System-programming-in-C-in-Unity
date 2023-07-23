using System;
using Unity.Netcode;
using UnityEngine;

namespace Network
{
    public abstract class NetworkMovableObject : NetworkBehaviour
    {
        protected abstract float _speed { get; }
        protected Action _onUpdateAction { get; set; }
        protected Action _onFixedUpdateAction { get; set; }
        protected Action _onLateUpdateAction { get; set; }
        protected Action _onPreRenderActionAction { get; set; }
        protected Action _onPostRenderAction { get; set; }
        /*
        [SyncVar] protected Vector3 _serverPosition;
        [SyncVar] protected Vector3 _serverEuler;
        */
        protected NetworkVariable<Vector3> _serverPosition= new NetworkVariable<Vector3>();
        protected NetworkVariable<Vector3> _serverEuler = new NetworkVariable<Vector3>();
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            //Debug.Log("OnNetworkSpawn() " + gameObject.name);
            Initiate();
        }
        protected virtual void Initiate(UpdatePhase updatePhase =
        UpdatePhase.Update)
        {
            switch (updatePhase)
            {
                case UpdatePhase.Update:
                    _onUpdateAction += Movement;
                    break;
                case UpdatePhase.FixedUpdate:
                    _onFixedUpdateAction += Movement;
                    break;
                case UpdatePhase.LateUpdate:
                    _onLateUpdateAction += Movement;
                    break;
                case UpdatePhase.PostRender:
                    _onPostRenderAction += Movement;
                    break;
                case UpdatePhase.PreRender:
                    _onPreRenderActionAction += Movement;
                    break;
            }
        }
        private void Update()
        {
            _onUpdateAction?.Invoke();
        }
        private void LateUpdate()
        {
            _onLateUpdateAction?.Invoke();
        }
        private void FixedUpdate()
        {
            _onFixedUpdateAction?.Invoke();
        }
        private void OnPreRender()
        {
            _onPreRenderActionAction?.Invoke();
        }
        private void OnPostRender()
        {
            _onPostRenderAction?.Invoke();
        }
        protected virtual void Movement()
        {
            if (IsOwner)
            {
                //Debug.Log("isOwner movement "+gameObject.name);
                HasAuthorityMovement();
            }
            else
            {
                //Debug.Log("another movement "+ gameObject.name);
                FromServerUpdate();
            }
        }
        protected abstract void HasAuthorityMovement();
        protected abstract void FromServerUpdate();
        protected abstract void SendToServer();

        public override void OnDestroy()
        {
            _onUpdateAction = null;
            _onFixedUpdateAction = null;
            _onLateUpdateAction = null;
            _onPreRenderActionAction = null;
            _onPostRenderAction = null;
            base.OnDestroy();
    }
    }
}