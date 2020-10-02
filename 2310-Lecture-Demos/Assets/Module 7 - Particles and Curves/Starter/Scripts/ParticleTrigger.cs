//(c) Samantha Stahlke 2020
//Created for INFR 2310.
using UnityEngine;

public class ParticleTrigger : MonoBehaviour
{
    public ParticleSystem particles;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Projectile") && particles != null)
        {
            particles.Play();
        }
    }
}
