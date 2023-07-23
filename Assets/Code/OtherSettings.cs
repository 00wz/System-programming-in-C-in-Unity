using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

namespace Data
{

    public class OtherSettings : NetworkBehaviour
    {

        [SerializeField] public string _playerName;

        [SerializeField] public List<Transform> spawnsPoints = new List<Transform>();

        void OnGUI()
        {
            GUILayout.BeginArea(new Rect(310, 10, 300, 300));
            if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer)
            {
                StartInputField();
                StatusName();
            }
            GUILayout.EndArea();
        }

        void StartInputField()
        {
            _playerName=GUILayout.TextField(_playerName, 5);
        }

        void StatusName()
        {
            GUILayout.Label("Name: " + _playerName);
        }
    }
}
