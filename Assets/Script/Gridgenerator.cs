using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    public GameObject hexPrefab;
    public int width = 10;
    public int height = 10;
    public float hexRadius = 1f;
    public float gap = 0.0f; // Tambahan opsional untuk jarak antar hex

    void Start()
    {
        GenerateHexGrid();
    }

    void GenerateHexGrid()
    {
        // --- PERBAIKAN DIMENSI (Matematika Heksagon) ---
        // Untuk Pointy Topped (Sudut menghadap atas/bawah):
        // Lebar (Horizontal) = sqrt(3) * radius
        // Tinggi (Vertikal) = 2 * radius

        float hexWidth = Mathf.Sqrt(3) * hexRadius;
        float hexHeight = 2f * hexRadius;

        // Tambahkan gap jika perlu
        float xDist = hexWidth + gap;
        float yDist = hexHeight * 0.75f; // Jarak vertikal antar baris adalah 3/4 tinggi

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float xPos = x * xDist;

                // --- PERBAIKAN OFFSET ---
                // Setiap baris ganjil digeser setengah dari lebar heksagon
                if (y % 2 == 1)
                {
                    xPos += xDist * 0.5f;
                }

                // Posisi Y
                float yPos = y * yDist; // Menggunakan yDist yang sudah dikali 0.75

                Vector3 pos = new Vector3(xPos, 0, yPos);

                GameObject hex = Instantiate(hexPrefab, pos, Quaternion.identity);

                // Pastikan rotasi sesuai dengan prefab Anda
                hex.transform.rotation = Quaternion.Euler(90, 0, 0);

                // Opsional: Jadikan hierarki rapi
                hex.transform.SetParent(this.transform);
            }
        }
    }
}