using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleGrid : MonoBehaviour {

    ParticleSystem ps;
    public Vector3 bounds = new Vector3(25.0f, 25.0f, 25.0f);
    public Vector3Int resolution = new Vector3Int(10, 10, 10);
    void OnEnable()
    {
        ps = GetComponent<ParticleSystem>();
        Vector3 scale;
        Vector3 boundHalf = bounds / 2.0f;

        scale.x = bounds.x / resolution.x;
        scale.y = bounds.y / resolution.y;
        scale.z = bounds.z / resolution.z;
        Vector3 position;
        ParticleSystem.EmitParams ep = new ParticleSystem.EmitParams();
        for (int i = 0; i < resolution.x; i++)
        {
            for (int j = 0; j < resolution.y; j++)
            {
                for (int k = 0; k < resolution.z; k++)
                {
                  

                    position.x = (i * scale.x) - boundHalf.x;
                    position.y = (j * scale.y) - boundHalf.y;
                    position.z = (k * scale.z) - boundHalf.z;
                    ep.position = position;
                    ps.Emit(ep, 1);
                }
            }
        }
    }

}
