using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

namespace Data
{

    public class OtherSettings : MonoBehaviour
    {

        [SerializeField] public string _playerName;

        [SerializeField] public List<Transform> spawnsPoints = new List<Transform>();
    }
}
