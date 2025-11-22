using UnityEngine;

public class ShipMovementInputSystem : MonoBehaviour
{
    private ShipBase ship;       // <--- ini adalah kapal induk / turunan
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
                isMoving = false;
        }
    }

    public void MoveToTile(Tile tile)
    {
        int distance = Mathf.Abs(tile.gridPos.x - ship.gridPos.x) +
                        Mathf.Abs(tile.gridPos.y - ship.gridPos.y);

        // Ship has moveRange limitation
        if (distance > ship.moveRange)
        {
            Debug.Log($"{ship.shipName} tidak bisa bergerak sejauh itu!");
            return;
        }

        int cost = distance * ship.moveCost;

        Debug.Log($"[{ship.shipName}] TP sebelum: {GameManager.Instance.TP}");
        Debug.Log($"Distance: {distance} | Cost: {cost}");

        if (GameManager.Instance.TP >= cost)
        {
            GameManager.Instance.TP -= cost;
            Debug.Log($"[{ship.shipName}] TP sesudah: {GameManager.Instance.TP}");

            ship.SetGridPos(tile.gridPos);
            targetPos = tile.transform.position;
            isMoving = true;
        }
        else
        {
            Debug.Log($"{ship.shipName} tidak cukup TP!");
        }
    }
}
