using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    public GameObject hexPrefab;
    public int width = 10;
    public int height = 10;
    public float hexRadius = 1f;
    public float gap = 0.0f;

    void Start()
    {
        GenerateHexGrid();
    }

    void GenerateHexGrid()
    {
        float hexWidth = Mathf.Sqrt(3) * hexRadius;
        float hexHeight = 2f * hexRadius;

        float xDist = hexWidth + gap;
        float yDist = hexHeight * 0.75f;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float xPos = x * xDist;

                if (y % 2 == 1)
                {
                    xPos += xDist * 0.5f;
                }

                float yPos = y * yDist;

                Vector3 pos = new Vector3(xPos, 0, yPos);

                GameObject hex = Instantiate(hexPrefab, pos, Quaternion.identity);
                hex.transform.rotation = Quaternion.Euler(90, 0, 0);
                hex.transform.SetParent(this.transform);

                // ==========================================================
                // BAGIAN DI BAWAH INI ADALAH TAMBAHAN YANG ANDA BUTUHKAN
                // AGAR COST DAN JARAK BISA DIHITUNG
                // ==========================================================

                // 1. Ubah nama objek di Hierarchy biar rapi (Contoh: Hexagon 0,0)
                hex.name = $"Hexagon {x},{y}";

                // 2. Ambil script 'Tile' dari prefab yang baru dibuat
                Tile tileScript = hex.GetComponent<Tile>();

                if (tileScript != null)
                {
                    // 3. ISI KOORDINATNYA!
                    // Ini langkah paling krusial. Kita masukkan 'x' dan 'y' loop
                    // ke dalam variabel gridPos milik Tile.
                    tileScript.gridPos = new Vector2Int(x, y);
                }
                else
                {
                    Debug.LogError("ERROR: Prefab Hexagon Anda belum dipasangi script 'Tile'!");
                }
                // ==========================================================
            }
        }
    }
}