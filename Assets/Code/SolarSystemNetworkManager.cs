using System;
using Unity.Netcode;
using Unity.Networking.Transport;
using UnityEngine;
namespace Main
{
    public class SolarSystemNetworkManager : NetworkManager,IDisposable
    {
        [SerializeField] private string _playerName;

        public void Dispose()
        {
            OnClientConnectedCallback -= OnServerAddPlayer;
        }

        private void Awake()
        {
            OnClientConnectedCallback += OnServerAddPlayer;
        }
        private void OnServerAddPlayer(ulong playerControllerId)
        {
            var player = SpawnManager.GetPlayerNetworkObject(playerControllerId);

            player.GetComponent<ShipController>().PlayerName = _playerName;
        }
    }
}
