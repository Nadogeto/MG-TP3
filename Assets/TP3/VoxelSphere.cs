using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelSphere : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        int radius = 100;
        Vector3 center = Vector3.zero;

        for (int i = -radius; i < radius; i++)
        {
            for (int j = -radius; j < radius; j++)
            {
                for (int k = -radius; k < radius; k++)
                {
                    Vector3 position = new Vector3(i, j, k);
                    float distance = Vector3.Distance(position, center);
                    if ( distance < radius)
                    {
                        VoxelTools.MakeCube(i, j, k);
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
