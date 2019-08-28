using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class SetCamposition : MonoBehaviour
{
    [Header("Aviation manager")]
    public AviationManager aviationManager;     //비행 관리자
    public ControllerInputCheck inputCheck;     //입력확인

    [Header ("View position")]
	public Transform FirstPersonView;           //1인칭 시점 위치
    public Transform ThirdPersonView;           //3인칭 시점 위치

    [Header ("Cam set object")]
    public Transform camPosition;               //카메라 세팅 위치 받아올 변수
    public Transform cam;                       //카메라

    [Header("Switch values")]
    public bool switchClicked;                  //컨트롤러값 받아오는 변수
    public int camSwitch;                       //캠위치 변경하는 변수 0은 1인칭, 1은 3인칭

    [Header("현재 카메라의 회전값을 받아오는 변수들")]
    public float camPitch;                      //카메라 pitch값
    public float camYaw;                        //카메라 yaw값

	void Start()
	{
        camSwitch = 0;      //캠위치 1인칭

		camPosition.position = FirstPersonView.position;
		cam.position = camPosition.position;
		UnityEngine.XR.InputTracking.disablePositionalTracking = true;
	}

	// Update is called once per frame
	void Update()
    {
        switchClicked = inputCheck.LMenuClicked.stateDown;
        if(switchClicked == true)
        {
            switchClicked = false;
            camSwitch = (camSwitch + 1) % 2;
        }
        
        if(transform.localRotation.x > -0.7f && transform.localRotation.x < 0.8f)
        {
            camPitch = transform.localRotation.x / 10;
        }
        if (transform.localRotation.y > -0.8f && transform.localRotation.y < 0.8f)
        {
            camYaw = transform.localRotation.y / 10;
        }

        //------------------------------------------

        //캠 위치 변경할 변수 위치 변경
        switch(camSwitch)
        {
            case 0:     //1인칭
		        camPosition.position = FirstPersonView.position;
                break;

            case 1:     //3인칭
                camPosition.position = ThirdPersonView.position;
                break;
        }

        //캠 위치 변경
		cam.position = camPosition.position + new Vector3(camPitch, camYaw, 0);
	}
    
}
