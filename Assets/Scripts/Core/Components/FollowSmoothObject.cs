using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowSmoothObject : MonoBehaviour
{
    public Transform parent;
    [SerializeField] float speed = 0.1f;

    private Vector3 targetPos = Vector3.right*100;
    private Vector3 oldPos, newPos, objPos;

    [Header("Position")]
    [SerializeField] bool followPositionX = true;
    [SerializeField] bool followPositionY = true;
    [SerializeField] bool followPositionZ = true;

    [SerializeField] Vector3 offsetPos;

    private void OnEnable()
    {
        oldPos = Vector3.up * -50; // To force update -> Don't work lol
    }

    // Update is called once per frame
    void LateUpdate()
    {
        targetPos = parent.position;

        newPos = targetPos;        

        //if (newPos != oldPos)
        {

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

            Vector3 smoothPos = Vector3.Lerp(transform.position, objPos, speed);
            transform.position = Vector3.MoveTowards(transform.position, smoothPos, 1);

            oldPos = objPos;
        }
    }

}
