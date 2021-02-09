using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : EntityResource
{
    public void TakeDamage(float damage)
    {
        ChangeValue(-damage);
    }
}
