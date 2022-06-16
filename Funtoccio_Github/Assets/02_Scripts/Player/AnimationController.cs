using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AnimationController : MonoBehaviour
{
    Vector2 wasdInput;
    private PlayerInput playerInput;
    private Animator animator;

    private InputAction moveAction;
    private InputAction jumpAction;
    //private InputAction interactAction;


    // Start is called before the first frame update
    private void Awake()
    {
        animator = gameObject.GetComponent<Animator>();

    }
    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();

        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
        //interactAction = playerInput.actions["Interact"];
    }

    void Update()
    {
        if (wasdInput != null)
        {
            wasdInput = moveAction.ReadValue<Vector2>();
        }
        else wasdInput = Vector2.zero;
        Vector3 movementVector = new Vector3(wasdInput.x, 0, 0);
        setAnimations(movementVector.x);

    }

    private void setAnimations(float isRunning)
    {
        //Walk//

        if (isRunning != 0)
        {
            animator.SetBool("isRunning", true);
        }
        else animator.SetBool("isRunning", false);

        //Jump//

        if (jumpAction.triggered )
        {
            animator.SetTrigger("jumpPressed");
        }
    }
}
