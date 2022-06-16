using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    
   [SerializeField] private float playerSpeed = 2.0f;
   [SerializeField] private float jumpHeight = 1.0f;
   [SerializeField] private float gravityValue = -9.81f;

    private CharacterController controller;
    private PlayerInput playerInput;
    private Vector3 playerVelocity;
    Vector2 wasdInput;
    private bool groundedPlayer;

    private InputAction moveAction;
    private InputAction jumpAction;


    private void Start()
    {
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();

        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
    }

    void Update()
    {
        Move();
    }

    public void Move() 
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        if (wasdInput != null)
        {
            wasdInput = moveAction.ReadValue<Vector2>();
        }
        Vector3 movementVector = new Vector3(wasdInput.x, 0, 0);

        controller.Move(movementVector * Time.deltaTime * playerSpeed);

        #region  //character Rotation//

        if (movementVector != Vector3.zero)
        {
            gameObject.transform.forward = movementVector;
        }
        #endregion

        #region  //character Jump//
        // Changes the height position of the player..
        if (jumpAction.triggered && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }
        #endregion

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    
}
