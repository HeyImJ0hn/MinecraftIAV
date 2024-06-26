using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floors : MonoBehaviour
{
    private void SetDefault()
    {
        foreach (Transform child in transform)
        {
            child.transform.position = new Vector3(child.transform.position.x, -2, child.transform.position.z);   
        }
    }
    public void SetAtRandom()
    {
        SetDefault();

        foreach(Transform child in transform)
        {
            if(Random.value < 0.5f)
            {
                child.transform.position += Vector3.up;
            }
        }
    }
}
