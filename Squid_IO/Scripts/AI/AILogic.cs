using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AILogic : MonoBehaviour
{
    [SerializeField] Transform destination;
    [SerializeField] float trapForce = 10f;
    [SerializeField] float axeForce = 8f;
    [SerializeField] float ballForce = 5f;

    [Header("NavMesh Components")]
    [SerializeField] AgentLinkMover mover;

    NavMeshAgent agent;
    Animator animator;
    CapsuleCollider collider;
    Rigidbody rb;
    Vector3 respawnPoint;
    Vector3 impact = Vector3.zero;
    bool canMove = true, notJump = true, isGrounded = false;
    float turnSmoothVelocity;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<CapsuleCollider>();
        respawnPoint = transform.position;
        agent = GetComponent<NavMeshAgent>();
        agent.updatePosition = false;
        agent.SetDestination(destination.position);
    }

    private void Update()
    {
        Move();

        if (!isGrounded && Mathf.Abs(rb.velocity.y) > 4f)
        {
            animator.SetBool("Air", true);
        }
        else
        {
            animator.SetBool("Air", false);
        }

        if (agent.isOnOffMeshLink && notJump)
        { 
            animator.SetTrigger("Jump");
            StartCoroutine(WaitJumpEnd());
        }

        CheckRespawn();
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
            }
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }

    private void Move()
    {
        agent.nextPosition = transform.position;

        if (!canMove)
            return;

        if (Mathf.Abs(rb.velocity.x) > 2f || Mathf.Abs(rb.velocity.z) > 2f)
            animator.SetBool("Moving", true);
        else
            animator.SetBool("Moving", false);

        agent.nextPosition = transform.position;
        Debug.DrawRay(transform.position, agent.desiredVelocity, Color.blue);
        rb.velocity = new Vector3(agent.desiredVelocity.x, rb.velocity.y, agent.desiredVelocity.z);
    }

    private void CheckRespawn()
    {
        if (transform.position.y < -50f)
        {
            transform.position = respawnPoint;
            agent.SetDestination(destination.position);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Trap")
        {
            StartCoroutine(InputDisabled(1f));

            Vector3 dir = (transform.position - collision.contacts[0].point).normalized;

            AddImpact(dir, trapForce);
        }
        else if (collision.collider.tag == "Ball")
        {
            StartCoroutine(InputDisabled(1f));

            Vector3 dir = (transform.position - collision.contacts[0].point).normalized;

            AddImpact(dir, trapForce);
        }
        else if (collision.collider.tag == "Fan")
        {
            mover.StopAllCoroutines();
            mover.enabled = false;
            agent.enabled = false;
            StartCoroutine(InputDisabled(1f));
        }
        else if (collision.collider.tag == "Axe")
        {
            StartCoroutine(InputDisabled(1f));

            Vector3 dir = (transform.position - collision.contacts[0].point).normalized;

            AddImpact(dir, trapForce);
        }
    }

    public void AddImpact(Vector3 dir, float force)
    {
        dir.Normalize();
        dir.y = 0.1f;
        impact = dir.normalized * force;
        rb.AddRelativeForce(impact, ForceMode.VelocityChange);
    }

    IEnumerator InputDisabled(float sec)
    {
        canMove = false;
        yield return new WaitForSeconds(sec);
        canMove = true;
    }

    IEnumerator WaitJumpEnd()
    {
        notJump = false;
        yield return new WaitForSeconds(mover.m_Time + 0.1f);
        notJump = true;
    }
}
