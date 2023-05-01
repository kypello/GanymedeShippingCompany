using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DestinationRegistry", menuName = "DestinationRegistry")]
public class DestinationRegistry : ScriptableObject
{
    [System.Serializable]
    public struct OrbitalSystem {
        public string name;
        public string[] locations;
    }

    public OrbitalSystem[] systems;
}
