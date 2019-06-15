using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleGrid : MonoBehaviour {

    ParticleSystem ps;
    public Vector3 bounds = new Vector3(25.0f, 25.0f, 25.0f);
    public Vector3Int resolution = new Vector3Int(10, 10, 10);
    public ParticleSystem.Particle[] pss;
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
        pss = new ParticleSystem.Particle[1000];
        ps.GetParticles(pss);
    }
    public bool m_bBox = true;
    public bool m_bSphere = false;
    public bool m_dirty = false;
    private void Update()
    {
        if (m_dirty)
        {
            if (m_bBox)
            {
                Vector3 scale;
                Vector3 boundHalf = bounds / 2.0f;

                scale.x = bounds.x / resolution.x;
                scale.y = bounds.y / resolution.y;
                scale.z = bounds.z / resolution.z;
                Vector3 position;
                for (int i = 0; i < resolution.x; i++)
                {
                    for (int j = 0; j < resolution.y; j++)
                    {
                        for (int k = 0; k < resolution.z; k++)
                        {


                            position.x = (i * scale.x) - boundHalf.x;
                            position.y = (j * scale.y) - boundHalf.y;
                            position.z = (k * scale.z) - boundHalf.z;
                            pss[i * 100 + j * 10 + k].position = position;
                        }
                    }
                }
            }
            else if(m_bSphere)
            {
                float radius = 15;
                int len = pss.Length;
                for(int si = 0; si < 10; si++)
                {
                    float radiusCircle = radius / (si+1);
                    for (int sj = 0; sj < len/10; sj++)
                    {
                        pss[100*si + sj].position = new Vector3(Mathf.Cos(Mathf.PI * sj * 2 / 100) * radiusCircle,
                                        Mathf.Sin(Mathf.PI * sj * 2 / 100) * radiusCircle,
                                        si);
                    }
                }
            }
            m_dirty = false;
            ps.SetParticles(pss,pss.Length);
        }
    }

    public void transBox()
    {
        m_bBox = true;
        m_bSphere = false;
        m_dirty = true;
    }
    public void transSphere()
    {
        m_bBox = false;
        m_bSphere = true;
        m_dirty = true;
    }
}
