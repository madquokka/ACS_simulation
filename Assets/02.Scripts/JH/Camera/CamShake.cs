using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class CamShake : MonoBehaviour
{
    public Camera cam;      //카메라 위치

    public float ShakeAmount;   //카메라 흔들기
    float ShakeTime;            //흔드는 시간
    Vector3 initialPosition;

    public SteamVR_Camera steamCam;


    public void VibrateForTime(float time)
    {
        ShakeTime = time;
    }

    // Start is called before the first frame update
    void Start()
    {
        ShakeTime = 5f;
        initialPosition = new Vector3(0, 0, -10f);
        UnityEngine.XR.InputTracking.disablePositionalTracking = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(ShakeTime > 0)
        {
            transform.position = Random.insideUnitSphere * ShakeAmount + initialPosition;
            ShakeTime -= Time.deltaTime;
        }
        else
        {
            ShakeTime = 0.0f;
            transform.position = initialPosition;
        }
    }
}
