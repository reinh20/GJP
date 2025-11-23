using UnityEngine;

public class ShipMovementInputSystem : MonoBehaviour
{
    public ShipBase ship;
    private bool isMoving = false;
    private Vector3 targetPos;

    [Header("Settings")]
    public float moveSpeed = 5f;

    // Pastikan ini diisi 1 atau lebih di inspector jika ShipBase tidak punya variabel cost sendiri
    public int defaultMoveCost = 1;

    void Start()
    {
        ship = GetComponent<ShipBase>();
        if (ship == null) Debug.LogError("ShipBase tidak ditemukan!");

        Debug.Log($"{ship.shipName} Start Grid: {ship.gridPos} | TP Awal: {GameManager.Instance.TP}");
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

                // Debug setelah sampai
                Debug.Log($"Sampai di tujuan. Sisa TP: {GameManager.Instance.TP}");
            }
        }
    }

    public void MoveToTile(Tile tile)
    {
        if (isMoving) return; // Cegah spam klik saat sedang jalan

        // 1. HITUNG JARAK HEXAGON YANG BENAR
        int distance = GetHexDistance(ship.gridPos, tile.gridPos);

        // 2. HITUNG COST
        // Ambil cost dari ship (jika ada) atau pakai default
        int costPerTile = (ship.moveCost > 0) ? ship.moveCost : defaultMoveCost;
        int totalCost = distance * costPerTile;

        Debug.Log($"Jarak: {distance} Tile | Cost: {totalCost} TP | TP Saat Ini: {GameManager.Instance.TP}");

        // 3. CEK RANGE (Opsional)
        if (distance > ship.moveRange)
        {
            Debug.LogWarning($"Kejauhan! Jarak {distance}, Range Kapal {ship.moveRange}");
            return;
        }

        // 4. CEK APAKAH TP CUKUP
        if (GameManager.Instance.TP >= totalCost)
        {
            // Kurangi TP
            GameManager.Instance.TP -= totalCost;

            // Pindah Visual
            if (tile.unitSocket != null)
            {
                targetPos = tile.unitSocket.position;
            }
            else
            {
                targetPos = tile.transform.position;
                targetPos.y = transform.position.y;
            }

            // Update Data Grid
            ship.SetGridPos(tile.gridPos);
            isMoving = true;
        }
        else
        {
            Debug.LogError($"Gagal Pindah! Butuh {totalCost} TP, cuma punya {GameManager.Instance.TP} TP.");
        }
    }

    // --- RUMUS MATEMATIKA HEXAGON (Odd-Row Offset) ---
    // Fungsi ini mengubah koordinat Grid biasa menjadi jarak Hexagon yang akurat
    private int GetHexDistance(Vector2Int a, Vector2Int b)
    {
        // Jika game Anda hanya membolehkan pindah 1 tile per giliran (hanya tetangga),
        // cukup return 1. Tapi jika bisa loncat jauh, gunakan rumus di bawah:

        // Konversi Offset Coordinates ke Axial Coordinates
        int a_q = a.x - (a.y - (a.y & 1)) / 2;
        int a_r = a.y;

        int b_q = b.x - (b.y - (b.y & 1)) / 2;
        int b_r = b.y;

        // Hitung jarak Axial
        return (Mathf.Abs(a_q - b_q) + Mathf.Abs(a_q + a_r - b_q - b_r) + Mathf.Abs(a_r - b_r)) / 2;
    }
}