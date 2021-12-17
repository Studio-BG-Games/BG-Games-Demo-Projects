using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AILevel2 : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    [SerializeField] float jumpForce = 10f;
    List<GameObject> hexagons = new List<GameObject>();
    List<GameObject> hexagonsJump = new List<GameObject>();

    Vector3 destination = Vector3.zero;
    Rigidbody rb;
    Animator animator;

    float border = 80f;
    bool fall = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (destination == Vector3.zero)
            NewDestination();

        if (Mathf.Abs(rb.velocity.y) < 2f)
        {
            if (fall)
            {
                NewDestination();
                fall = false;
            }

            if (Vector3.Distance(transform.position, destination) < 2f)
            {
                NewDestination();
            }
            Move();
            animator.SetBool("Air", false);
            animator.SetBool("Moving", true);
        }
        else
        {
            animator.SetBool("Moving", false);
            animator.SetBool("Air", true);
        }

        CheckIfFall();

        CheckDeath();
    }

    private void CheckDeath()
    {
        if (transform.position.y < -35)
        {
            Destroy(gameObject);
        }
    }

    void CheckHexagones()
    {
        hexagons.Clear();
        Collider[] hitColliders = Physics.OverlapSphere(transform.position + Vector3.up, 5);
        foreach (Collider collider in hitColliders)
        {
            if (collider.gameObject.tag == "Hex" && !collider.gameObject.GetComponent<HexDisabler>().destroying)
                hexagons.Add(collider.gameObject);
        }
    }

    void CheckHexagonesForJump()
    {
        hexagonsJump.Clear();
        Collider[] hitColliders = Physics.OverlapSphere(transform.position + Vector3.up, 10);
        foreach (Collider collider in hitColliders)
        {
            if (collider.gameObject.tag == "Hex" && !collider.gameObject.GetComponent<HexDisabler>().destroying)
            { 
                if(!hexagons.Contains(collider.gameObject))
                    hexagonsJump.Add(collider.gameObject);
            }
        }
    }

    void NewDestination()
    {
        CheckHexagones();
        if (hexagons.Count < 3)
        {
            CheckHexagonesForJump();
            int jump = Random.Range(0, 2);
            //If there no near hexagons bot must jump
            if (hexagons.Count == 0)
                jump = 1;

            switch (jump)
            {
                case 0:
                    if (hexagons.Count > 0)
                    {
                        int rnd = Random.Range(0, hexagons.Count);
                        destination = hexagons[rnd].transform.position;
                    }
                    break;
                case 1:
                    if (hexagonsJump.Count > 0)
                    {
                        int rnd = Random.Range(0, hexagonsJump.Count);
                        destination = hexagonsJump[rnd].transform.position;
                        Jump();
                    }
                    break;
                default: break;
            }
        }
        else
        {
            if (hexagons.Count > 0)
            {
                int rnd = Random.Range(0, hexagons.Count);
                destination = hexagons[rnd].transform.position;
            }
        }
    }

    private void Jump()
    {
        animator.SetTrigger("Jump");
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private void CheckIfFall()
    {
        if (transform.position.y < border)
        {
            fall = true;
            border /= 2;
        }
    }

    private void Move()
    {
        Vector3 dir = (destination - transform.position).normalized;
        dir.y = 0f;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 10f);
        rb.velocity = new Vector3(dir.x * speed, rb.velocity.y, dir.z * speed);
    }

    private void OnDrawGizmos()
    {
        /*
        Gizmos.DrawWireSphere(transform.position + Vector3.up, 5);
        Gizmos.DrawWireSphere(transform.position + Vector3.up, 10);
        */
    }
}