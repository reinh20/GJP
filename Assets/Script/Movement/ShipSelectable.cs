using UnityEngine;

public class ShipSelectable : MonoBehaviour
{
    public ShipMovementInputSystem movement;

    void Awake()
    {
        movement = GetComponent<ShipMovementInputSystem>();
    }
}
