using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Centering : MonoBehaviour
{
    [SerializeField] private Transform pivot;
    [SerializeField] private CapsuleCollider myCollider;

    private Vector3 vector;

    private void OnValidate()
    {
        myCollider = GetComponent<CapsuleCollider>();
    }
    void Start()
    {
        FindTeleportPivotAndTarget();
        vector.y = myCollider.center.y;
    }
    
    void Update()
    {
        vector.x = pivot.localPosition.x;
        vector.z = pivot.localPosition.z;

        myCollider.center = vector;
    }

    private void FindTeleportPivotAndTarget()
    {
        foreach (var cam in Camera.allCameras)
        {
            if(!cam.enabled){continue;}
            if(cam.stereoTargetEye != StereoTargetEyeMask.Both){continue;}

            pivot = cam.transform;
        }
    }
}
