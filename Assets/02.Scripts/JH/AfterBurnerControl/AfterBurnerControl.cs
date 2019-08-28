using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterBurnerControl : MonoBehaviour
{
    [Header("Aviation manager")]
    public AviationManager aviationManager;

    [Header("particle Systems")]
    public ParticleSystem throttleParticle;     //쓰로틀 파티클 시스템 받아옴
    public ParticleSystem afterBurnerParticle;  //애프터 버너 파티클 시스템 받아옴

    [Header("ParticleSystem_mainModule")]
    public ParticleSystem.MainModule throttleModule;        //쓰로틀 메인 모듈 받아옴
    public ParticleSystem.MainModule afterburnerModule;     //애프터 버너 메인 모듈 받아옴

    [Header("ParticleSystem_emmision_module")]
    public ParticleSystem.EmissionModule afterburnerEmissionModule;

    [Header("booster value")]
    public int _booster;

    // Start is called before the first frame update
    void Start()
    {
        //각 파티클의 메인 모듈을 받아옴
        throttleModule = throttleParticle.main;
        afterburnerModule = afterBurnerParticle.main;

        throttleModule.startSpeed = 2.5f;
        afterburnerModule.startSpeed = throttleModule.startSpeed.constant * 1.5f;

        throttleModule.startSize = new ParticleSystem.MinMaxCurve(1f, 1.5f);
        afterburnerModule.startSize = throttleModule.startSize;

        afterburnerEmissionModule = afterBurnerParticle.emission;
    }

    // Update is called once per frame
    void Update()
    {
        _booster = aviationManager.booster;

        //애프터버너 부스터 킬때와 끌때 파티클 개수 조정
        if(_booster == 1)
        {
            //print("60");
            afterburnerEmissionModule.rateOverTime = 60f;
        }
        else if(_booster == 0)
        {
            //print("0");
            afterburnerEmissionModule.rateOverTime = 0f;
        }

        //파티클 시작속도 변환
        throttleModule.startSpeed = (aviationManager.throttle * 7.5f) + 2.5f;
        afterburnerModule.startSpeed = throttleModule.startSpeed.constant * 1.5f;

        //파티클 시작크기 변환
        throttleModule.startSize = new ParticleSystem.MinMaxCurve( (aviationManager.throttle) + 1f
                                                             , (aviationManager.throttle) * 1.5f + 1.5f);
        afterburnerModule.startSize = throttleModule.startSize;
    }
}
