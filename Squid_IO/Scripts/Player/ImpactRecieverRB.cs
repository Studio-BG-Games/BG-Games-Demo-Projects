using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImpactRecieverRB : MonoBehaviour
{
    Vector3 impact = Vector3.zero;
    [SerializeField] Rigidbody rb;
    [SerializeField] float force = 100f;
    [SerializeField] float axeForce = 60f;
    [SerializeField] float ballForce = 20f;

    [Space]
    [SerializeField] Text trapTxt;
    [SerializeField] Text ballTxt;

    private void OnCollisionEnter(Collision collision)
    {
        if (!GetComponent<PlayerMovementRB>().inputDisabled)
        { 
            if (collision.collider.tag == "Trap")
            {
                GetComponent<PlayerMovementRB>().DisableInput(1f);
                Vector3 dir = (transform.position - collision.contacts[0].point).normalized;

                AddImpact(dir, force);
            }
            else if (collision.collider.tag == "Ball")
            {
                GetComponent<PlayerMovementRB>().DisableInput(0.5f);

                Vector3 dir = (transform.position - collision.contacts[0].point).normalized;

                AddImpact(dir, ballForce);
            }
            else if (collision.collider.tag == "Axe")
            {
                GetComponent<PlayerMovementRB>().DisableInput(0.5f);

                Vector3 dir = (transform.position - collision.contacts[0].point).normalized;

                AddImpact(dir, axeForce);
            }
        }
    }

    // call this function to add an impact force:
    public void AddImpact(Vector3 dir, float force)
    {
        dir.Normalize();
        dir.y = 0.1f;
        impact = dir.normalized * force;
        rb.AddRelativeForce(impact, ForceMode.VelocityChange);
    }

    public void ChangeBallForce(float force)
    {
        ballTxt.text = "Ball: " + force.ToString();
        ballForce = force;
    }

    public void ChangeTrapForce(float force)
    {
        trapTxt.text = "Ball: " + force.ToString();
        this.force = force;
    }
}
