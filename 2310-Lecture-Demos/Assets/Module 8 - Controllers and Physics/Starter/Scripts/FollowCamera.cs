//(c) Samantha Stahlke 2020
//Created for INFR 2310.
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public float followDistance = 4.0f;
    public float height = 5.0f;
    public Vector3 offset = Vector3.zero;
    public bool fixedHeight = false;
    public GameObject target;

    void LateUpdate()
    {
        Vector3 lookPos = target.transform.position + offset;

        if (fixedHeight)
            lookPos.y = offset.y;

        Vector3 followPos = lookPos - followDistance * transform.forward;
        followPos.y = height;

        transform.position = followPos;
        transform.LookAt(lookPos);
    }
}
