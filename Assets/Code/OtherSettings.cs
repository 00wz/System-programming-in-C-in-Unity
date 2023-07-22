using System.Collections.Generic;
using UnityEngine;

namespace Data
{

    public class OtherSettings : MonoBehaviour
    {

        [SerializeField] public string _playerName;

        [SerializeField] public List<Transform> spawnsPoints = new List<Transform>();
    }
}
