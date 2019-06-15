using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogoPlayback : MonoBehaviour {

    ParticleSystem [] ps;
    float  []lifeTime;
    const float MAX_TIME = 2.0f;
	// Use this for initialization
	void OnEnable () {
        ps = GetComponentsInChildren<ParticleSystem>();
        lifeTime = new float[ps.Length];
        for(int i = 0; i < ps.Length; i++)
        {
            lifeTime[i] = 0.0f ;
        }
	}
	
	// Update is called once per frame
	void Update () {
        ps[0].Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        for(int i = ps.Length-1; i>=0; i--)
        {
            ps[i].Play(false);
            float delta = ps[i].main.useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
            lifeTime[i] += delta ;
            float curTime = MAX_TIME - lifeTime[i];

            ps[i].Simulate(curTime, false, false, false);
        }
	}
}
