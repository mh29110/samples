using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReverseParticleSystem : MonoBehaviour {

    ParticleSystem[] psArr;
    float[] psTimesArr;
    public float startTime = 2.0f;
    public float speedScale = 1.0f;

    private void Init()
    {
        psArr = GetComponentsInChildren<ParticleSystem>(false);
        psTimesArr = new float[psArr.Length];
    }

    void OnEnable () {
        if (psArr == null)
        {
            Init();
        }

        for (int i = 0; i < psArr.Length; i++)
        {
            psTimesArr[i] = 0.0f;
        }
    }
	
	// Update is called once per frame
	void Update () {
        psArr[0].Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);//先循环都停了，不然乱

        for (int i = psArr.Length - 1; i >= 0; i--)
        {
            psArr[i].Play(false);//必备的
            float deltaTime = psArr[i].main.useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;//类型 main中设置
            psTimesArr[i] -= (deltaTime * psArr[i].main.simulationSpeed) * speedScale;

            float curTime = startTime + psTimesArr[i];

            psArr[i].Simulate(curTime, false, false, true);

            if (curTime < 0.0f)
            {
                psArr[i].Play(false);
                psArr[i].Stop(false, ParticleSystemStopBehavior.StopEmittingAndClear);
            }
        }


	}
}
