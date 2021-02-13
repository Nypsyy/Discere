using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private Camera _cam;

    void Update()
    {
        _cam = _cam ?? Camera.main;

        transform.LookAt(_cam.transform.position);
        transform.right = -transform.right;
    }
}
