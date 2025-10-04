using UnityEngine;
using UnityEngine.InputSystem;

public class ControlManager : MonoBehaviour
{
    //public enum KeyboardType { QWERTY, AZERTY };
    //public KeyboardType keyType;

    //public InputMaster controls;
    [SerializeField] private PlayerInput playerInput;

    [Header("Elements")]
    public GameManager game;
    public Player player;
    public CameraRotation camRotation;

    [Header("Storage")]
    [SerializeField] private float isGamepad;

    void Start()
    {
        //controls = new InputMaster();
        //controls.Enable();

        if(player) // PLAYER
        {
            playerInput.actions["Move"].performed += ctx => player.controlInput = Vector2.ClampMagnitude(ctx.ReadValue<Vector2>(), 1f);
            playerInput.actions["Move"].canceled += ctx => player.controlInput = Vector2.zero;
            //controls.Player.Move.performed += ctx => player.controlInput = Vector2.ClampMagnitude(ctx.ReadValue<Vector2>(), 1f);
            //controls.Player.Move.canceled += _ => player.controlInput = Vector2.zero;
            //controls.Player.Jump.performed += _ => player.desiredJump |= true;
        }

        if(game)
        {
            playerInput.actions["Quit"].started += ctx => game.OnEscape();

            playerInput.actions["Interact"].started += ctx => game.PlayerActionPressed();
        }

        if(camRotation)
        {
            playerInput.actions["Look"].performed += ctx => camRotation.controlInput = ctx.ReadValue<Vector2>();
            playerInput.actions["Look"].canceled += ctx => camRotation.controlInput = Vector2.zero;
        }

        //controls.Menu.Escape.started += _ => TogglePauseMenu();
    }

}
