using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ebac.Core.Sigleton;
using Cinemachine;

public class ShakeCamera : Singleton <ShakeCamera>
{
    public CinemachineVirtualCamera virtualCamera;

    public float shakeTime;

    private CinemachineBasicMultiChannelPerlin _c;

    [Header("Shake Values")]
    public float amplitude = 3f;
    public float frequency = 3f;
    public float time = .3f;


 

    [NaughtyAttributes.Button]
    public void Shake()
    {
        Shake(amplitude, frequency, time);
    }

    public void Shake(float amplitude, float frequency, float time)
    {
        _c.m_AmplitudeGain = amplitude;
        _c.m_FrequencyGain = frequency;

        shakeTime = time;
    }

    public void StopShake()
    {
        _c.m_AmplitudeGain = 0f;
        _c.m_FrequencyGain = 0f;
    }

    private void Update()
    {
        if(shakeTime > 0)
        {
            shakeTime -= Time.deltaTime;
        }
        else
        {
            if(_c == null) _c = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            StopShake();
        }
    }
}
