using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrustControl : MonoBehaviour
{
	#region public_values
	[Header("Aviation_status")]
	public AviationManager aviationManager;
	[HideInInspector] public AviationManager.AviationStatus status;

	[Header("Torque_status")]
	public TorqueControl torqueControl;

	[Header ("Fighter")]
	public Rigidbody fighterBody;

	[Header ("Force values")]
	public float currForce		= 0f;					//현재 추력값
	public float ADCeleration	= 0f;					//감가속치
	public float TorqueResist	= 0f;					//회전항력
    public float BrakeResist    = 0f;                   //브레이크 저항력

	[Header("Torque_values")]
	public Quaternion controlAngle;

	[Header("inputCheck")]
	[HideInInspector] public int boosterValue;			//감가속치 계산에 쓸 booster bool값 변환 변수
	[HideInInspector] public float throttle;            //감가속치 계산에 쓸 throttle변수
    public int brakeButton;

	[Header("Speed_boundary")]
	[HideInInspector] public float _minimumSpeedBoundary; //최소 속도경계
	[HideInInspector] public float _normalSpeedBoundary;  //평균 속도경계
	[HideInInspector] public float _maxSpeedBoundary;     //최대 속도경계
	#endregion

	#region private_value

	private float throttleVal = 0f;     //감가속치 계산에 쓸 쓰로틀 계산변수
	private float boosterVal = 0f;      //감가속치 계산에 쓸 부스터 계산변수

	#endregion

	#region Start
	// Start is called before the first frame update
	void Start()
    {
		//초기 트랙패드 값 0.5가 들어가므로 0으로 초기화 시켜준다.
		throttle = 0f;

		//속도경계 설정
		_minimumSpeedBoundary = aviationManager.minimumSpeedBoundary;
		_normalSpeedBoundary = aviationManager.normalSpeedBoundary;
		_maxSpeedBoundary = aviationManager.maxSpeedBoundary;
    }
	#endregion

	#region Update
	// Update is called once per frame
	void Update()
    {
		//정기 상태 갱신
		boosterValue = aviationManager.booster;      //부스터 갱신
		throttle = aviationManager.throttle;    //쓰로틀 갱신
        brakeButton = aviationManager.BrkBtn;   //브레이크 갱신

		controlAngle = aviationManager.controlAngle;    //회전각 갱신
		TorqueResist = torqueControl.TorqueResist;		//회전저항값 갱신

		//비행 상태값에 따른 감가속 계산
		switch(status)
		{
			case AviationManager.AviationStatus.BELOW_MINIMUM_SPEED:
				SetADCeleration(0);                                     //감가속 구하기
				break;

			case AviationManager.AviationStatus.NORMAL_SPEED:
				SetADCeleration(1);
				break;

			case AviationManager.AviationStatus.AFTER_BURNER:
				SetADCeleration(2);
				break;
		}

		SetCurrForce();		//현재 추력값 세팅
		Thrust();			//추력값으로 비행체 전진

		SetStatus();		//비행 상태 갱신

		

	}
	#endregion

	#region Thrust

	//감가속 변수 구함
	private void SetADCeleration(int stat)
	{
        
		switch(stat)
		{
			//이착륙 상태
			case 0:
                //- 이착륙 상태 구함
                //쓰로틀 출력구함
                throttleVal = Mathf.Pow((throttle + 0.45f), 2);
				//부스터 출력구함
				boosterVal = boosterValue * Mathf.Pow(throttle, 2);
				//감가속치 구함(가중치 제외)
				ADCeleration = ((throttleVal + boosterVal));

                BrakeResist = (float)brakeButton * 10f;
                //5배 더올림
                break;

			//일반 비행 상태
			case 1:
                
                //- 일반 비행상태 구함
                //쓰로틀 출력구함
                throttleVal = throttle;
				//부스터 출력구함
				boosterVal = boosterValue * throttle;
                //감가속치 구함(가중치 제외)
                ADCeleration = (((throttleVal + boosterVal) / 2)
                               * Mathf.Log(Mathf.Epsilon + (_normalSpeedBoundary / _minimumSpeedBoundary) - (currForce / _minimumSpeedBoundary) + 1, 3))
                               + 1f;
                BrakeResist = (float)brakeButton * 10f;
                break;

			//애프터버너 상태
			case 2:
                //- 애프터 버너 비행상태 구함
                //쓰로틀 출력구함
                throttleVal = throttle;
                //부스터 출력구함
                boosterVal = boosterValue * throttle;
                //감가속치 구함(가중치 제외)
                ADCeleration = (((throttleVal + boosterVal) / 2)
                               * Mathf.Log(Mathf.Epsilon + (_normalSpeedBoundary / _minimumSpeedBoundary) - (currForce / _minimumSpeedBoundary) + 1, 3))
                               + 1f;
                BrakeResist = (float)brakeButton * 10f;
                //가속치 올리려고 1.2배 더 올림
                //2배 더 올림
                break;
		}
	}

	//현재 추력 계산
	private void SetCurrForce()
	{
		currForce = currForce + ADCeleration - TorqueResist - BrakeResist;    //현재속도 구하기
		//현재 속도 최대속도면 최대속도에 고정
		if (currForce > Mathf.Epsilon + _maxSpeedBoundary) currForce = _maxSpeedBoundary;
		//현재 속도 후진속도 최대면 후진 최저속도에 고정
		if (currForce < Mathf.Epsilon - 5f) currForce = -5f;
		
	}

	//현재 추력만큼 비행기 전진
	private void Thrust()
	{
		//가중치 더해서 전투기 발진
		fighterBody.AddRelativeForce(Vector3.back * currForce * 100f, ForceMode.Force);
	}

	#endregion

	#region status_channel
	//비정기 비행 상태 갱신
	public void GetStatus(AviationManager.AviationStatus stat)
	{
		status = stat;
	}

	//추력값에 따른 비행 상태 변환(AviationManager로 메서드 호출)
	private void SetStatus()
	{
		if(currForce <= Mathf.Epsilon + _minimumSpeedBoundary)
		{
			aviationManager.SetStatus(AviationManager.AviationStatus.BELOW_MINIMUM_SPEED);
		}
		else if(currForce > Mathf.Epsilon + _minimumSpeedBoundary 
			&& currForce <= Mathf.Epsilon + _normalSpeedBoundary)
		{
			aviationManager.SetStatus(AviationManager.AviationStatus.NORMAL_SPEED);
		}
		else if(currForce <= Mathf.Epsilon + _maxSpeedBoundary)
		{
			aviationManager.SetStatus(AviationManager.AviationStatus.AFTER_BURNER);
		}
	}
	#endregion
}
