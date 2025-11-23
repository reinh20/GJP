using UnityEngine;

public class Submarine : ShipBase
{
    void Start()
    {
        shipName = "Scout";
        moveRange = 5;
        moveCost = 2;
        maxHP = 6;
    }
}
