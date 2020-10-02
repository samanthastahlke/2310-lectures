//(c) Samantha Stahlke 2020
//Created for INFR 2310.
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    public Camera cam;
    public Projectile projectile;
    public float spawnDist = 4.0f;

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Input.mousePosition;
            
            //Creates a world-space ray based on the given screen position and the camera's position.
            Ray spawnRay = cam.ScreenPointToRay(new Vector3(mousePos.x, mousePos.y, cam.nearClipPlane));
            Vector3 spawnPos = spawnRay.GetPoint(spawnDist);

            //Spawn a projectile.
            Projectile shot = Instantiate(projectile) as Projectile;
            //Use our calculated spawn position.
            shot.transform.position = spawnPos;

            //Launch velocity should follow the ray we generated to look right.
            Vector3 launchXZ = new Vector3(spawnRay.direction.x, 0.0f, spawnRay.direction.z);
            Vector3 launchVelocity = shot.shotSpeed * launchXZ.normalized;
            launchVelocity.y = shot.shotYSpeed;
            shot.launchVelocity = launchVelocity;
            shot.Fire();
        }
    }
}
