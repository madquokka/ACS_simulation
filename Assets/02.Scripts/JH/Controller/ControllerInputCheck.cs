using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class ControllerInputCheck : MonoBehaviour
{
    [Header ("Input_sources")]
    public SteamVR_Input_Sources leftController;
	public SteamVR_Input_Sources rightController;

    [Header("LeftHand_controller")]
    public SteamVR_Action_Boolean LMenuClicked;         //메뉴 버튼 클릭 확인
    public SteamVR_Action_Boolean LTriggerClicked;      //트리거 클릭 확인
    public SteamVR_Action_Vector2 LTrackpadDragged;     //트랙패드 드래깅 확인
    public SteamVR_Action_Pose LPoseTracker;            //위치값 받음
    public SteamVR_Action_Vibration LHaptic;            //진동값 받음


    [Header("RightHand_controller")]
    public SteamVR_Action_Boolean RMenuClicked;         //메뉴 버튼 클릭 확인
	public SteamVR_Action_Boolean RTriggerClicked;      //트리거 클릭 확인
	public SteamVR_Action_Vector2 RTrackpadDragged;     //트랙패드 드래깅 확인
	public SteamVR_Action_Pose RPoseTracker;            //위치값 받음
    public SteamVR_Action_Vibration RHaptic;            //진동값 받음

    [Header ("Recalculated_values for aviationManager")]
	[HideInInspector] public bool booster = false;
	[HideInInspector] public float throttle = 0;
	[HideInInspector] public Quaternion RcontrollerAngle;
    [HideInInspector] public bool menu = false;

    [Header("Recalculated_values for SetCamPosition")]
    public bool lMenu = false;

    [Header("진동 메서드 발생 주기 컨트롤")]
    private float hapticDuration = 0.5f;

    // Update is called once per frame
    void Update()
    {
        //------------------------------------오른쪽 컨트롤러 갱신 영역 시작
		//컨트롤러 각도 갱신
		RcontrollerAngle = RPoseTracker.localRotation;
		
		//오른쪽 트리거 눌렀을 때(한번)
		if(RTriggerClicked.stateDown)
		{
			booster = true;

            //진동 발생주기 변수 리셋
            hapticDuration = 0.3f;
        }
        else if(RTriggerClicked.state)
        {
            float rand = Random.Range(0.5f, 1);     //랜덤 진동 크기 생성
            HapticOn(0.3f, rand);                   //햅틱컨트롤의 메서드 실행
        }
		//트리거 뗐을 때(한번)
		else if(RTriggerClicked.stateUp)
		{
			booster = false;
		}

        //오른쪽 메뉴 눌렀을 때
        if(RMenuClicked.stateDown)
        {
            menu = true;
        }
        else if(RMenuClicked.state)
        {
            float rand = Random.Range(0.5f, 1);
            HapticOn(0.3f, rand);
        }
        //오른쪽 메뉴 뗐을 때
        else if(RMenuClicked.stateUp)
        {
            menu = false;
        }

		//트랙패드 값 변경시
		if(RTrackpadDragged.changed)
		{
			throttle = Mathf.Lerp(throttle
								  , ((RTrackpadDragged.axis.y + 1f) / 2)
								  , 0.1f);
		}
        //------------------------------------오른쪽 컨트롤러 갱신 영역 끝

        //------------------------------------왼쪽 컨트롤러 갱신 영역 시작

        //왼쪽 컨트롤러 메뉴 버튼은 카메라 위치 변경에 사용됨

        //------------------------------------왼쪽 컨트롤러 갱신 영역 끝
    }

    private void HapticOn(float duration, float rand)
    {
        hapticDuration += Time.deltaTime;       //진동실행주기 변수 갱신
        if (hapticDuration > duration - 0.1f)               //0.5초 지나면 1회 실행
        {
            hapticDuration = 0;
            //오른쪽 버튼 누르는 동안 진동 발생
            LHaptic.Execute(0, duration, 50 * rand, 35 * rand, leftController);
            RHaptic.Execute(0, duration, 50 * rand, 35 * rand, rightController);
        }
    }
}
