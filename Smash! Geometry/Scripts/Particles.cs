using UnityEngine;

public class Particles : MonoBehaviour
{
    ParticleSystem part;
    [SerializeField] GameObject sparkle;
    public ParticleCollisionEvent[] collisionEvents;

    void Start()
    {
        collisionEvents = new ParticleCollisionEvent[16];
        part = gameObject.GetComponent<ParticleSystem>();
    }

    private void OnParticleCollision(GameObject other)
    {

        int safeLength = part.GetSafeCollisionEventSize();
        if (collisionEvents.Length < safeLength)
            collisionEvents = new ParticleCollisionEvent[safeLength];
        int numCollisionEvents = part.GetCollisionEvents(other, collisionEvents);
        int i = 0;

        while (i <= numCollisionEvents)
        {
            Vector3 pos = collisionEvents[i].intersection;
            Instantiate(sparkle, pos, Quaternion.identity);
            i++;
        }
    }


}
