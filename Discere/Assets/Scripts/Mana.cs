using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mana : EntityResource
{
    public float recovering = 4f;

    // Update is called once per frame
    void Update()
    {
        if (value < maxValue)
        {
            ChangeValue(recovering * Time.deltaTime, false);
        }
    }

    public void UseMana(float amount)
    {
        ChangeValue(-amount);
    }
}
