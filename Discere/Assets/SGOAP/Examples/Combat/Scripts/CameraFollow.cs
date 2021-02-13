using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform Target;

    private Vector3 _startPosition;
    private Vector3 _difference;

    private void Awake()
    {
        _startPosition = transform.position;
        _difference = Target.position - _startPosition;
    }

    private void Update()
    {
        var newPosition = Target.position - _difference;
        
        transform.position = Vector3.Lerp(transform.position, newPosition, 8 * Time.deltaTime);
    }

    public void SetTarget()
    {

    }
}
