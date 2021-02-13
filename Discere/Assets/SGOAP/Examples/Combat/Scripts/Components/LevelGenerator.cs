using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public GameObject Cube;
    public GameObject Grenade;

    public float Cols = 20;
    public float Rows = 20;

    public int MaxCubes = 10;
    public int MaxGrenade = 3;

    private void Awake()
    {
        for (int y = 0; y < Rows; y++)
        {
            for (int x = 0; x < Cols; x++)
            {
                if (Random.Range(0, 5) == 0 && MaxCubes != 0)
                {
                    var position = transform.position + new Vector3(x, 0.5f, y);
                    var cube = Instantiate(Cube);
                    cube.transform.position = position;
                    MaxCubes--;
                    continue;
                }

                if (Random.Range(0, 5) == 0 && MaxGrenade != 0)
                {
                    var position = transform.position + new Vector3(x, 0, y);
                    var grenade = Instantiate(Grenade);
                    grenade.transform.position = position;
                    MaxGrenade--;
                }
            }
        }
    }
}
