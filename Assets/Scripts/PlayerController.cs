using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public int playerId = 1;

    private Controls controls;
    private OverDriveDriftController carController;
    private Vector2 moveInput;

    private void Awake()
    {
        controls = new Controls();
        carController = GetComponent<OverDriveDriftController>();
    }

    private void OnEnable()
    {
        controls.Enable();

        if (playerId == 1)
        {
            controls.Player1.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
            controls.Player1.Move.canceled += ctx => moveInput = Vector2.zero;
        }
        else if (playerId == 2)
        {
            controls.Player2.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
            controls.Player2.Move.canceled += ctx => moveInput = Vector2.zero;
        }
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    private void Update()
    {
        carController.SetInputVector(moveInput);
    }
}
