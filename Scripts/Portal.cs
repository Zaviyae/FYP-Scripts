using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public Portal nextLevelPortal;
    public bool levelPortal;
    Vector3 startSize;
    Quaternion startRotation;

    public Transform spawnPos;

    private bool Enlarge;

    public Camera VRCam;

    public Transform childRotationTransform;

    private void Start()
    {
        childRotationTransform = GetComponentInChildren<RotateMe>().transform;
        startSize = childRotationTransform.localScale;
        startRotation = childRotationTransform.rotation;

        VRCam = Camera.main;

    }

    public void LookedAt()
    {
        Enlarge = true;
    }

    public void LookedAway()
    {
        Enlarge = false;
    }

    private void Update()
    {
        if (Enlarge && childRotationTransform.localScale.x < startSize.x * 2)
        {
            childRotationTransform.localScale = Vector3.Lerp(childRotationTransform.localScale, childRotationTransform.localScale * 2, 1.5f * Time.deltaTime);

            childRotationTransform.LookAt(VRCam.transform);


        }
        else
        {
            if (transform.localScale != startSize && !Enlarge)
            {
                childRotationTransform.localScale = Vector3.Lerp(childRotationTransform.localScale, startSize, 2f * Time.deltaTime);

                childRotationTransform.rotation = Quaternion.Lerp(childRotationTransform.rotation, startRotation, 1f * Time.deltaTime);

            }
        }
    }
}
