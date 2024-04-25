using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player Info")]
[System.Serializable]
public class PlayerInfo : ScriptableObject
{
    public int coins;
    public List<Vehicle> listVehicles;
}

[System.Serializable]
public class Vehicle
{
    public int id;
    public int price;
    public bool owned;
    public bool used;
}
