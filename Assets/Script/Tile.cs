using UnityEngine;

public class Tile : MonoBehaviour
{
    public Vector2Int gridPos;

    private Renderer rend;
    private Color baseColor;
    public bool isHighlighted = false;

    void Awake()
    {
        rend = GetComponent<Renderer>();
        baseColor = rend.material.color;
        TileHighlighter.Instance?.RegisterTile(this);
    }

    public void Highlight()
    {
        rend.material.SetColor("_BaseColor", Color.yellow);
        isHighlighted = true;
    }

    public void ResetColor()
    {
        rend.material.color = baseColor;
        isHighlighted = false;
    }
}
