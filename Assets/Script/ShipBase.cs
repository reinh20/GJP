using UnityEngine;

public class ShipBase : MonoBehaviour
{
    // Stat kapal dasar (bisa di-overwrite oleh class turunan)
    public int moveRange = 3;
    public int moveCost = 1;
    public int maxHP = 10;
    public string shipName = "Base Ship";

    // Grid position
    public Vector2Int gridPos;

    // Mengupdate posisi grid jika perlu
    public void SetGridPos(Vector2Int newPos)
    {
        gridPos = newPos;
    }
}
