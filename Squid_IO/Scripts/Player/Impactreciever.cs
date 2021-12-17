using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Impactreciever : MonoBehaviour
{
    float mass = 3f; // defines the character mass
    Vector3 impact = Vector3.zero;
    private CharacterController character;
 
    private void Start()
    {
        character = GetComponent<CharacterController>();
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.tag == "Trap")
        {
            AddImpact(hit.normal, 100f);
        }
    }

    // call this function to add an impact force:
    public void AddImpact(Vector3 dir, float force)
    {
        dir.Normalize();
        if (dir.y < 0) dir.y = -dir.y; // reflect down force on the ground
        impact += dir.normalized * force / mass;
    }

    private void Update()
    {
        // apply the impact force:
        if (impact.magnitude > 0.2) 
            character.Move(impact * Time.deltaTime);
        // consumes the impact energy each cycle:
        impact = Vector3.Lerp(impact, Vector3.zero, 5 * Time.deltaTime);
    }
}
