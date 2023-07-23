using Characters;
using Data;
using System;
using Unity.Netcode;
using UnityEngine;
namespace Main
{
    public class SolarSystemNetworkManager : NetworkManager,IDisposable
    {
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
            if (this.LocalClientId!=playerControllerId) return;
            var player = SpawnManager.GetPlayerNetworkObject(playerControllerId);

            var spawnPoint = SettingsContainer.Instance?.OtherSettings.spawnsPoints[0];
            if (spawnPoint != null)
                player.transform.position = spawnPoint.position;
            /*player.GetComponent<ShipController>().PlayerName = 
                SettingsContainer.Instance?.OtherSettings._playerName;*/
        }
    }
}
