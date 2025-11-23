using UnityEngine;

public class ShipMovementInputSystem : MonoBehaviour
{
    public ShipBase ship;       // <--- ini adalah kapal induk / turunan
    private bool isMoving = false;
    private Vector3 targetPos;

    public float moveSpeed = 5f;

    void Start()
    {
        ship = GetComponent<ShipBase>();
        if (ship == null)
            Debug.LogError("ShipBase tidak ditemukan!");

        // Debug posisi awal
        Debug.Log($"{ship.shipName} start grid = {ship.gridPos}");
    }

    void Update()
    {
        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPos) < 0.05f)
            {
                transform.position = targetPos;
                isMoving = false;
            }
        }

    }

    public void MoveToTile(Tile tile)
    {
        int distance = Mathf.Abs(tile.gridPos.x - ship.gridPos.x) +
                        Mathf.Abs(tile.gridPos.y - ship.gridPos.y);
        int cost = distance * ship.moveCost;

        // Ship has moveRange limitation
        if (GameManager.Instance.TP >= cost)
        {
            GameManager.Instance.TP -= cost;

            // --- BAGIAN BARU (LEBIH SIMPEL) ---

            // Kita langsung ambil posisi socket. 
            // Karena socket sudah kita atur tingginya di Prefab, kapal akan pas posisinya.
            if (tile.unitSocket != null)
            {
                targetPos = tile.unitSocket.position;
            }
            else
            {
                // Fallback (jaga-jaga lupa pasang socket)
                targetPos = tile.transform.position;
                targetPos.y = transform.position.y;
                Debug.LogWarning($"Tile {tile.name} lupa dipasang UnitSocket!");
            }

            // ----------------------------------

            ship.SetGridPos(tile.gridPos);
            isMoving = true;
        }
        else
        {
            Debug.Log("Tidak cukup TP untuk bergerak!");
        }
    }
}
