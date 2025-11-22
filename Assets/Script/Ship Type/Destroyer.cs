using UnityEngine;

public class Destroyer : ShipBase
{
    void Start()
    {
        shipName = "Destroyer";
        moveRange = 4;
        moveCost = 1;
        maxHP = 8;
    }
}
