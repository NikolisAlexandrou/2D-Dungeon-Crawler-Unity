using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] GameObject obToFollow;
    [SerializeField] Camera cam;
    [SerializeField] float z;



    public GameObject getObjectToFollow()
    {
        return obToFollow;
    }

    public void setObjectToFollow(GameObject ob)
    {
        obToFollow = ob;
    }

    private void FixedUpdate()
    {
        if (obToFollow == null)
            return;
        //if (isFollowing == false)
        //    return;

        cam.transform.position = new Vector3(obToFollow.transform.position.x, obToFollow.transform.position.y, z);
    }

}
