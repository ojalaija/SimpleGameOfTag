using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Essentials
    [SerializeField] private Transform cam;
    private CharacterController controller;
    private float turnSmoothTime = .1f;
    private float turnSmoothVelocity;
    private Animator animator;

    // Movement
    [SerializeField] private float walkSpeed;
    [SerializeField] private float sprintSpeed;
    private bool sprinting = true;
    private Vector2 movement;
    private float trueSpeed;

    // Jumping
    [SerializeField] private float jumpHeight;
    [SerializeField] private float gravity;
    private bool isGrounded;
    private Vector3 velocity;

    void Start()
    {
        trueSpeed = sprintSpeed;
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Keep animator with player
        animator.transform.localPosition = Vector3.zero;
        animator.transform.localEulerAngles = Vector3.zero;
        
        GroundCheck();
        Move();
        Jump();
        RoleChanging();
    }

    private void GroundCheck()
    {
        // Ground check for jumping
        isGrounded = Physics.CheckSphere(transform.position, 0.1f, 1);
        animator.SetBool("IsGrounded", isGrounded);

        if (isGrounded && velocity.y < 0) velocity.y = -1;
    }

    private static void RoleChanging()
    {
        // Change roles
        if (Input.GetKeyDown(KeyCode.R))
        {
            EventManager.OnChangeRoles();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    private void Jump()
    {
        //Jumping
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            velocity.y = Mathf.Sqrt((jumpHeight * 10) * -2f * gravity);
        }

        if (velocity.y > -20)
        {
            velocity.y += (gravity * 10) * Time.deltaTime;
        }

        controller.Move(velocity * Time.deltaTime);
    }

    private void Move()
    {
        // Walk or sprint
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            trueSpeed = walkSpeed;
            sprinting = false;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            trueSpeed = sprintSpeed;
            sprinting = true;
        }

        // Movement direction
        movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector3 direction = new Vector3(movement.x, 0, movement.y).normalized;

        // Move direction with camera
        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothTime, turnSmoothVelocity);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDirection.normalized * trueSpeed * Time.deltaTime);

            // Setting animations
            if (sprinting == true)
            {
                animator.SetFloat("Speed", 2);
            }
            else
            {
                animator.SetFloat("Speed", 1);
            }
        }
        else
        {
            animator.SetFloat("Speed", 0);
        }
    }
}
