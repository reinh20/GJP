using UnityEngine;

// Definisikan status giliran
public enum GameState { PlayerTurn, EnemyTurn }

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameState state;

    [Header("Resource Info")]
    public int TP = 5;      // Action Point saat ini
    public int maxTP = 5;   // Maksimal AP per giliran
    public int turnCount = 1;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        // Mulai game
        StartNewPlayerTurn();
    }

    // --- FUNGSI UTAMA UNTUK GANTI GILIRAN ---
    // Pasang fungsi ini di Button UI "End Turn"
    public void EndPlayerTurn()
    {
        if (state != GameState.PlayerTurn) return;

        Debug.Log("--- PLAYER END TURN ---");
        state = GameState.EnemyTurn;

        // Simulasi logika musuh, lalu kembali ke Player
        Invoke("StartNewPlayerTurn", 1.0f);
    }

    public void StartNewPlayerTurn()
    {
        state = GameState.PlayerTurn;
        turnCount++;

        // 1. Reset TP Penuh Lagi
        TP = maxTP;

        // 2. RESET STATUS SEMUA KAPAL (PENTING!)
        ResetAllShips();

        Debug.Log($"--- TURN {turnCount} (PLAYER) --- TP Refilled.");
    }

    // Fungsi Mencari Semua Kapal dan Mereset 'hasMoved'
    void ResetAllShips()
    {
        // --- PERBAIKAN DI SINI ---
        // Menggunakan FindObjectsByType dengan SortMode.None (Lebih Cepat & Tidak Error)
        ShipMovementInputSystem[] allShips = FindObjectsByType<ShipMovementInputSystem>(FindObjectsSortMode.None);

        foreach (var ship in allShips)
        {
            ship.ResetAction(); // Panggil fungsi reset di script kapal
        }
    }
}