using Main;
using Mechanics;
using Network;
using UI;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Networking;
namespace Characters
{
    public class ShipController : NetworkMovableObject
    {
        /*public string PlayerName
        {
            get => _playerName.Value.ToString();
            set
                { 
                _playerName.Value = value;
                //gameObject.name = _playerName.Value.ToString();
                }
            }*/
        protected override float _speed => _shipSpeed;
        [SerializeField] private Transform _cameraAttach;
        private CameraOrbit _cameraOrbit;
        private PlayerLabel _playerLabel;
        private float _shipSpeed;
        private Rigidbody _rb;
        //[SyncVar] private string _playerName;
        private NetworkVariable<FixedString64Bytes> _playerName = new NetworkVariable<FixedString64Bytes>();
        
        private void OnGUI()
        {
            if (_cameraOrbit == null)
            {
                return;
            }
            _cameraOrbit.ShowPlayerLabels(_playerLabel);
        }

        /*private void Start()
        {
            gameObject.name = _playerName.Value.ToString();
        }*/
        /*private void Awake()
        {
            _playerName.OnValueChanged += ChangeName;
        }*/

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            if (IsOwner)
            {
                Debug.Log("OnNetworkSpawn() override " + gameObject.name + " isOwner=" + IsOwner);
                _rb = GetComponent<Rigidbody>();
                if (_rb == null)
                {
                    return;
                }

                _cameraOrbit = FindObjectOfType<CameraOrbit>();
                _cameraOrbit.Initiate(_cameraAttach == null ? transform :
                _cameraAttach);
                _playerLabel = GetComponentInChildren<PlayerLabel>();


                SetNameServerRpc(SettingsContainer.Instance?.OtherSettings._playerName);
                //Initiate(UpdatePhase.FixedUpdate);
            }
            gameObject.name = _playerName.Value.ToString();
        }
        private void ChangeName(FixedString64Bytes old, FixedString64Bytes current)
        {
            gameObject.name = current.ToString();
        }
        [ServerRpc] private void SetNameServerRpc(string name)
        {
            _playerName.Value = name;
        }
        protected override void HasAuthorityMovement()
        {
            //Debug.Log(_playerName.Value.ToString());
            //Debug.Log("HasAuthorityMovement");
            var spaceShipSettings =
            SettingsContainer.Instance?.SpaceShipSettings;
            if (spaceShipSettings == null)
            {
                return;
            }
            var isFaster = Input.GetKey(KeyCode.LeftShift);
            var speed = spaceShipSettings.ShipSpeed;
            var faster = isFaster ? spaceShipSettings.Faster : 1.0f;
            _shipSpeed = Mathf.Lerp(_shipSpeed, speed * faster,
            SettingsContainer.Instance.SpaceShipSettings.Acceleration);
            var currentFov = isFaster
            ? SettingsContainer.Instance.SpaceShipSettings.FasterFov
            : SettingsContainer.Instance.SpaceShipSettings.NormalFov;
            _cameraOrbit.SetFov(currentFov,
            SettingsContainer.Instance.SpaceShipSettings.ChangeFovSpeed);
            var velocity =
            _cameraOrbit.transform.TransformDirection(Vector3.forward) * _shipSpeed;
            _rb.velocity = velocity;// * Time.deltaTime;
            if (!Input.GetKey(KeyCode.C))
            {
                var targetRotation = Quaternion.LookRotation(
                Quaternion.AngleAxis(_cameraOrbit.LookAngle,
                -transform.right) *
                velocity);
                transform.rotation = Quaternion.Slerp(transform.rotation,
                targetRotation, Time.deltaTime * speed);
            }
        }
        protected override void FromServerUpdate() { }
        protected override void SendToServer() { }

        private void LateUpdate()
        {
            if (!IsClient) { return; }
            _cameraOrbit?.CameraMovement();
        }
    }
}
