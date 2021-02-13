using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderResult : MonoBehaviour
{
    public bool Collided;

    private void OnTriggerStay(Collider other)
    {
        Collided = true;
    }

    private void OnTriggerExit(Collider other)
    {
        Collided = false;
    }
}
