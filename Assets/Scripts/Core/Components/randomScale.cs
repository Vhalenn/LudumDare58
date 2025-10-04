using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class randomScale : MonoBehaviour
{
    public bool randRot;
    public float min, max;

    // Start is called before the first frame update
    void Awake()
    {
        float rand = Random.value;
        float sVal = min + rand * (max-min);
        transform.localScale = Vector3.one * sVal;

        if(randRot)
            transform.GetChild(0).localRotation = Quaternion.Euler(-90,rand * 360,0);
    }
}
