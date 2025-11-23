using UnityEngine;

public class ShipMovementInputSystem : MonoBehaviour
{
    [Header("References")]
    public ShipBase ship; // Referensi ke data status kapal (Range, Cost, dll)

    [Header("Movement Settings")]
    public float moveSpeed = 8f; // Kecepatan animasi gerak
    public int defaultMoveCost = 1; // Biaya default jika di ShipBase 0

    [Header("Status (Debug)")]
    public bool isMoving = false; // Sedang animasi jalan?
    public bool hasMoved = false; // Sudah jalan giliran ini? (Limit 1x)

    // --- TAMBAHAN BARU: Status Gerakan Pertama ---
    public bool isFirstMove = true; // True = Belum pernah dipindah (Deployment)
    // ---------------------------------------------

    private Vector3 targetPos;

    void Start()
    {
        // Otomatis cari komponen ShipBase di object yang sama
        ship = GetComponent<ShipBase>();

        if (ship == null)
        {
            Debug.LogError($"[ERROR] {gameObject.name} tidak punya script 'ShipBase'!");
            this.enabled = false; // Matikan script biar gak error beruntun
        }
    }

    void Update()
    {
        // Logika Animasi Pergerakan (Smooth Movement)
        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

            // Cek apakah sudah sampai (jarak sangat dekat)
            if (Vector3.Distance(transform.position, targetPos) < 0.05f)
            {
                transform.position = targetPos; // Snap ke posisi presisi
                isMoving = false;

                Debug.Log($"Kapal {ship.shipName} sampai di tujuan.");
            }
        }
    }

    // Fungsi Utama: Dipanggil oleh SelectionManager saat klik Tile
    public void MoveToTile(Tile tile)
    {
        // --- 1. VALIDASI KONDISI ---

        // A. Jangan bisa gerak kalau lagi animasi jalan
        if (isMoving) return;

        // B. Cek apakah kapal ini SUDAH bergerak di giliran ini?
        if (hasMoved)
        {
            Debug.LogWarning($"🚫 Kapal {ship.shipName} sudah bergerak giliran ini! Tunggu giliran depan.");
            return;
        }

        // --- 2. PERHITUNGAN MATEMATIKA ---

        // Hitung jarak Hexagon (Logic Catur/Board Game)
        int distance = GetHexDistance(ship.gridPos, tile.gridPos);

        // --- 3. HITUNG COST (MODIFIKASI: FIRST MOVE FREE) ---

        // Ambil cost normal
        int normalCost = (ship.moveCost > 0) ? ship.moveCost : defaultMoveCost;

        // Jika ini gerakan pertama (Placement), GRATIS. Jika bukan, bayar normal.
        int totalCost = (isFirstMove) ? 0 : normalCost;

        // ----------------------------------------------------

        // --- 4. CEK RULES GAME ---

        // Cek Range (Jangkauan)
        if (!isFirstMove && distance > ship.moveRange)
        {
            Debug.Log($"❌ Kejauhan! Jarak: {distance}, Max Range: {ship.moveRange}");
            return;
        }

        // Cek TP (Apakah poin cukup?)
        if (GameManager.Instance.TP < totalCost)
        {
            Debug.Log($"❌ TP Tidak Cukup! Butuh {totalCost}, Punya {GameManager.Instance.TP}");
            return;
        }

        // --- 5. EKSEKUSI PERPINDAHAN ---

        // Bayar TP (Akan 0 jika isFirstMove == true)
        GameManager.Instance.TP -= totalCost;

        // Tentukan Posisi Target (Visual)
        if (tile.unitSocket != null)
        {
            // Jika Tile punya Socket, pakai posisi itu (Paling Akurat)
            targetPos = tile.unitSocket.position;
        }
        else
        {
            // Fallback: Pakai posisi Tile tapi pertahankan tinggi (Y) kapal
            targetPos = new Vector3(tile.transform.position.x, transform.position.y, tile.transform.position.z);
        }

        // Update Data Grid (Logika)
        ship.SetGridPos(tile.gridPos);

        // --- UPDATE STATUS ---
        hasMoved = true;      // Kunci untuk giliran ini (tetap berlaku limit 1x)
        isMoving = true;      // Mulai animasi

        // Matikan status First Move, supaya gerakan berikutnya BAYAR
        if (isFirstMove)
        {
            isFirstMove = false;
            Debug.Log("✅ Deployment Selesai. Gerakan berikutnya akan berbayar.");
        }
        // ---------------------

        Debug.Log($"✅ {ship.shipName} bergerak ke {tile.gridPos}. Cost: {totalCost}. Sisa TP: {GameManager.Instance.TP}");
    }

    // Dipanggil oleh GameManager saat ganti giliran (StartNewTurn)
    public void ResetAction()
    {
        hasMoved = false;
        // PENTING: Jangan reset isFirstMove di sini! 
        // Deployment cuma sekali seumur hidup, bukan tiap turn.
    }

    // --- RUMUS JARAK HEXAGON (Axial Distance) ---
    private int GetHexDistance(Vector2Int a, Vector2Int b)
    {
        int a_q = a.x - (a.y - (a.y & 1)) / 2;
        int a_r = a.y;
        int b_q = b.x - (b.y - (b.y & 1)) / 2;
        int b_r = b.y;

        return (Mathf.Abs(a_q - b_q) + Mathf.Abs(a_q + a_r - b_q - b_r) + Mathf.Abs(a_r - b_r)) / 2;
    }
}