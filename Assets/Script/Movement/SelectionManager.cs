using UnityEngine;
using UnityEngine.InputSystem;

public class SelectionManager : MonoBehaviour
{
    public static SelectionManager Instance;

    private InputSystem controls;
    private Camera cam;

    [HideInInspector] 
    public ShipMovementInputSystem selectedShip;

    void Awake()
    {
        Instance = this;
        controls = new InputSystem();
        cam = Camera.main;
    }

    void OnEnable()
    {
        controls.Player.Enable();
        controls.Player.Click.performed += OnClick;
    }

    void OnDisable()
    {
        controls.Player.Click.performed -= OnClick;
        controls.Player.Disable();
    }

    private void OnClick(InputAction.CallbackContext ctx)
    {
        DetectClick();
    }

    void DetectClick()
    {
        Vector2 mousePos = controls.Player.Point.ReadValue<Vector2>();
        Ray ray = cam.ScreenPointToRay(mousePos);
        RaycastHit hit;

        if (!Physics.Raycast(ray, out hit))
            return;

        // 1. Jika klik kapal → pilih kapal
        ShipSelectable ship = hit.collider.GetComponent<ShipSelectable>();
        if (ship != null)
        {
            SelectShip(ship.movement);
            return;
        }

        // 2. Jika klik tile → gerakkan kapal yang terpilih
        Tile tile = hit.collider.GetComponent<Tile>();
        if (tile != null && selectedShip != null)
        {
            selectedShip.MoveToTile(tile);
        }
    }

    void SelectShip(ShipMovementInputSystem ship)
    {
        selectedShip = ship;
        Debug.Log("Ship selected: " + ship.name);
    }
}
