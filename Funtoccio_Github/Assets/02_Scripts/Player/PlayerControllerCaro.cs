using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using System.Collections;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
public class PlayerControllerCaro : MonoBehaviour
{
    
    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float jumpDuration = 1.0f;

    [SerializeField] private float TurnAngle;
    [HideInInspector] public bool isDebugThrow;

    [Header("__Please Implement manually")]
    public Transform spineTag;

    private float gravityValue = 0;
    private CharacterController controller;
    private PlayerInput playerInput;
    private Vector3 playerVelocity;
    private Vector2 wasdInput;
    private bool groundedPlayer;
    private bool isJumping = false;
    private float currentYPos;
    private InputAction moveAction;
    private InputAction jumpAction;


    PuppetAnimationController m_animController;

    private void Start()
    {
        m_animController = GetComponent<PuppetAnimationController>();
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
            m_animController.Cross.forward = movementVector;

            float multiplier = 1;
            m_animController.isFacingRight = false;

            if (movementVector.x > 0)
            {
                multiplier = -1;
                m_animController.isFacingRight = true;
            }

            m_animController.Cross.rotation = Quaternion.Euler(0, gameObject.transform.localRotation.eulerAngles.y - TurnAngle * multiplier, 0);
        }
        #endregion

        #region  //Actions//
        // Changes the height position of the player..
        if (jumpAction.triggered && !isJumping)
        {
            //playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            currentYPos = transform.position.y;
            isJumping = true;
            CrossJump();
        }
        #endregion

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    public void CrossJump()
    {
        transform.DOMoveY(currentYPos + jumpHeight, jumpDuration).SetEase(Ease.OutCirc).OnComplete(() =>
        {
            transform.DOMoveY(currentYPos, jumpDuration).SetEase(Ease.InCirc).OnComplete(() =>
            { 
                isJumping = false;
            });
        });

        transform.position = new Vector3(transform.position.x, currentYPos, transform.position.z);
        Debug.Log("currentPos is" + currentYPos + "and transform value is" + transform.position.y);

    }

    public void ChangeTagToPlayer(bool isToPlayer)
    {
        if (isToPlayer)
        {
            spineTag.gameObject.tag = "Player";
        }
        else
        {
            spineTag.gameObject.tag = "Untagged";
        }
        
    }


  






}
