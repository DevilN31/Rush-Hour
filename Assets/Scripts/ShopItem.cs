using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShopItem : ScriptableObject {

    public string carName = "Car name goes here";
    public int unlockScore = 100;
    public string description = "Car description";
    public bool isBought = false;
}
