using UnityEngine;

public class ShipBase : MonoBehaviour
{
    // Stat kapal dasar (bisa di-overwrite oleh class turunan)
    public int moveRange;
    public int moveCost;
    public int maxHP;
    public string shipName = "Base Ship";

    // Grid position
    public Vector2Int gridPos;

    // Mengupdate posisi grid jika perlu
    public void SetGridPos(Vector2Int newPos)
    {
        gridPos = newPos;
    }
}
