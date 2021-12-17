using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementCC : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] Transform cam;

    public float speed = 6f;
    public float turnTime = 0.1f;
    public float jumpHeight = 1f;
    public float gravity = -9.81f;

    float turnSmoothVelocity;
    bool jumping = false;

    Joystick joystick;
    CharacterController controller;
    Rigidbody rb;
    Vector3 velocity;


    private void Start()
    {
        joystick = FindObjectOfType<Joystick>();
        controller = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (velocity.y < -5f)
        {
            animator.SetBool("Air", true);
        }
        else
        {
            animator.SetBool("Air", false);
        }

        Vector3 direction = new Vector3(joystick.Horizontal, 0f, joystick.Vertical);

        if (direction.magnitude >= 0.1f)
        {
            animator.SetBool("Moving", true);
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir * speed * Time.deltaTime);
        }
        else
            animator.SetBool("Moving", false);

        // Gravity
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = 0f;
        }

        if (jumping)
        {
            velocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravity);
            jumping = false;
        }
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    public void Jump()
    {
        if (controller.isGrounded)
        {
            animator.SetTrigger("Jump");
            jumping = true;
        }
    }
}
