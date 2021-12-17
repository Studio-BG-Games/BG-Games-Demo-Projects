using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovementRB : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] Transform cam;
    [SerializeField] float turnTime = 0.1f;
    [SerializeField] float speed = 6f;
    [SerializeField] float jumpForce = 10f;
    [SerializeField] float extraHeight = 0.01f;

    [HideInInspector]
    public bool inputDisabled = false;

    CapsuleCollider collider;

    bool isGrounded = false;
    bool canJump = false;

    float turnSmoothVelocity;

    Joystick joystick;
    Rigidbody rb;

    void Start()
    {
        collider = GetComponent<CapsuleCollider>();
        //distToGround = GetComponent<CapsuleCollider>().bounds.extents.y;
        joystick = FindObjectOfType<Joystick>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            Jump();

        //change animator state for air
        if (!isGrounded && Mathf.Abs(rb.velocity.y) > 3f)
        {
            animator.SetBool("Air", true);
        }
        else
        {
            animator.SetBool("Air", false);
        }

        //rigidbidy never sleep
        if (rb.IsSleeping())
        {
            rb.WakeUp();
        }

        if (Physics.Raycast(collider.bounds.center, Vector3.down, collider.bounds.extents.y + extraHeight))
            canJump = true;
        else
            canJump = false;
    }

    void FixedUpdate()
    {
        if(!inputDisabled)
            Move();
    }

    private void OnCollisionStay(Collision col)
    {
        foreach (ContactPoint p in col.contacts)
        {
            Vector3 bottom = collider.bounds.center - (Vector3.up * collider.bounds.extents.y);
            Vector3 curve = bottom + (Vector3.up * collider.radius);

            Vector3 dir = curve - p.point;
            //Debug.DrawLine(p.point, p.point + p.normal, Color.blue, 0.25f);

            if (dir.y > 0f)
            {
                isGrounded = true;

                Vector3 contactDir = p.point - curve;
                float angle = Vector3.Angle(contactDir, Vector3.up);

                angle = 180f - Mathf.Abs(angle);

                if (angle > 35f)
                {
                    //Debug.Log("fall???");
                }
            }
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }

    public void Move()
    {
        Vector3 direction = new Vector3(joystick.Horizontal, 0f, joystick.Vertical);

        if (direction.magnitude >= 0.1f)
        {
            animator.SetBool("Moving", true);

            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;


            moveDir = new Vector3(moveDir.x * speed, rb.velocity.y, moveDir.z * speed);
            rb.velocity = moveDir;
        }
        else
            animator.SetBool("Moving", false);
    }

    public void Jump()
    {
        if (canJump || isGrounded)
        {
            animator.SetTrigger("Jump");
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    public void DisableInput(float sec)
    {
        StartCoroutine(InputDisabled(sec));
    }

    IEnumerator InputDisabled(float sec)
    {
        inputDisabled = true;
        yield return new WaitForSeconds(sec);
        inputDisabled = false;
    }
}
