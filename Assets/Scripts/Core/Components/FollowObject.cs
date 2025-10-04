using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    public Transform parent;
    [SerializeField] bool stepped;

    private Vector3 targetPos = Vector3.right*100;
    private Vector3 oldPos, newPos, objPos;

    [Header("Position")]
    public bool followPositionX;
    public bool followPositionY;
    public bool followPositionZ;

    public Vector3 offsetPos;

    // Update is called once per frame
    void LateUpdate()
    {
        targetPos = parent.position;
        if(stepped) targetPos = StepVector3(targetPos);

        newPos = targetPos;        

        if (newPos != oldPos)
        {
            if(stepped) newPos = StepVector3(newPos);

            if (followPositionX && followPositionY && followPositionZ)
            {
                objPos = newPos + offsetPos;
            }
            else
            {
                objPos = transform.position;

                if (followPositionX)
                    objPos.x = newPos.x + offsetPos.x;

                if (followPositionY)
                    objPos.y = newPos.y + offsetPos.y;

                if (followPositionZ)
                    objPos.z = newPos.z + offsetPos.z;               
            }

            transform.position = objPos;
        }

        oldPos = objPos;
    }

    Vector3 StepVector3(Vector3 v){
        v.x = Mathf.Round(v.x);
        v.y = Mathf.Round(v.y);
        v.z = Mathf.Round(v.z);

        return v;
    }
}
