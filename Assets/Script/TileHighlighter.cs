using UnityEngine;
using System.Collections.Generic;

public class TileHighlighter : MonoBehaviour
{
    public static TileHighlighter Instance;

    private List<Tile> allTiles = new List<Tile>();

    void Awake()
    {
        Instance = this;

        // Ambil semua tile dari scene
        Tile[] tiles = FindObjectsOfType<Tile>();
        allTiles.AddRange(tiles);
         Instance = this;
    Debug.Log("TileHighlighter Awake!");
    }

    public void HighlightTilesInRange(Vector2Int center, int range)
    {
        ClearAllHighlights();

        foreach (Tile t in allTiles)
        {
            int dist = Mathf.Abs(t.gridPos.x - center.x) + Mathf.Abs(t.gridPos.y - center.y);

            if (dist <= range)
                t.Highlight();
        }
    }

    public void ClearAllHighlights()
    {
        foreach (Tile t in allTiles)
            if (t.isHighlighted)
                t.ResetColor();
    }
    public void RegisterTile(Tile tile)
{
    if (!allTiles.Contains(tile))
        allTiles.Add(tile);
}

}
