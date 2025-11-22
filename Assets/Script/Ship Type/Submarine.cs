using UnityEngine;

public class Submarine : ShipBase
{
    void Start()
    {
        shipName = "Submarine";
        moveRange = 5;
        moveCost = 2;
        maxHP = 6;
    }
}
