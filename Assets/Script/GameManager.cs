using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int TP = 5;      // Turn Point saat ini
    public int maxTP = 5;   // Maksimal TP tiap turn

    void Awake()
    {
        // Biar cuma ada 1 GameManager di scene
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    // Reset TP untuk turn baru
    public void ResetTP()
    {
        TP = maxTP;
    }
}
