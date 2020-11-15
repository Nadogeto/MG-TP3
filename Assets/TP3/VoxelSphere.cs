using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelSphere : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //between -6 and 6
        int radius = 6;
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
                        //calls the makecube() function of voxeltools
                        VoxelTools.MakeCube(i, j, k);
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //hit A to make the cubes fall
        if (Input.GetKeyDown(KeyCode.A))
        {
            VoxelTools.MakeAllCubesFall();
        }
    }
}
