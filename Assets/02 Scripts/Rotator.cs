using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public float rotationSpeed = 60f;
    Transform tr;

    private void Start()
    {
        tr = GetComponent<Transform>();
    }

    void Update()
    {
        tr.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);        
    }
}
