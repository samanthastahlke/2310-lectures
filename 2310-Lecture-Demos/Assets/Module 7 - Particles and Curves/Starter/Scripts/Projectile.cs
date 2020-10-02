//(c) Samantha Stahlke 2020
//Created for INFR 2310.
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    //What height should the projectile despawn at?
    public float despawnY = -100.0f;
    //How fast should the projectile travel in the XZ plane?
    public float shotSpeed = 10.0f;
    //How much of a boost should the projectile get in traveling upwards?
    public float shotYSpeed = 5.0f;
    public Vector3 launchVelocity { get; set; }

    void Update()
    {
        if (transform.position.y < despawnY)
            Destroy(this.gameObject);
    }

    public void Fire()
    {
        this.GetComponent<Rigidbody>().AddForce(launchVelocity, ForceMode.Impulse);
    }
}
