using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AviationManager : MonoBehaviour
{
	#region aviation_status_code
	public enum AviationStatus
	{
		BELOW_MINIMUM_SPEED,
		NORMAL_SPEED,
		AFTER_BURNER
	}
	#endregion

	#region public_values
	[Header ("inputCheck")]
	public ControllerInputCheck inputCheck;
	[HideInInspector] public int booster;			//부스터 발사 확인(트리거)
	[HideInInspector] public float throttle;		//쓰로틀 발생 확인(트랙패드)
	[HideInInspector] public Quaternion _controllerAngle;   //회전각도 처리용(컨트롤러 각도)

	[HideInInspector] public Quaternion controlAngle;
	[HideInInspector] public float pitch = 0;
	[HideInInspector] public float yaw = 0;
	[HideInInspector] public float roll = 0;
	[HideInInspector] public float axis = 0;
    [HideInInspector] public int BrkBtn = 0;

	[Header ("ThrustControl")]
	public ThrustControl thrustControl;

	[Header ("TorqueControl")]
	public TorqueControl torqueControl;

	[Header ("Aviation_status")]
	[HideInInspector] public AviationStatus status = AviationStatus.BELOW_MINIMUM_SPEED;

	[Header ("Speed_boundary")]
	[HideInInspector] public float minimumSpeedBoundary;
	[HideInInspector] public float normalSpeedBoundary;
	[HideInInspector] public float maxSpeedBoundary;

	#endregion

	#region Awake
	private void Awake()
	{
		minimumSpeedBoundary = 1200;
		normalSpeedBoundary = 6000;
		maxSpeedBoundary = 8500;
	}
	#endregion

	// Start is called before the first frame update
	void Start()
    {
		
    }

    // Update is called once per frame
    void Update()
    {
        
		//정기 상태 갱신코드
		_controllerAngle = inputCheck.RcontrollerAngle;	// 오른쪽 컨트롤러 각도 갱신
		
		//회전각도 정리
        if(_controllerAngle.x >= Mathf.Epsilon - 0.8 && _controllerAngle.x <= Mathf.Epsilon)
        {
            //-0.8에서 0 사이의 값을 받아서 -1에서 1 사이의 값으로 변환
		    pitch	= ((Mathf.Clamp(_controllerAngle.x, Mathf.Epsilon - 0.8f, Mathf.Epsilon) + 0.28f) / 4 * 10);
        }
        if(_controllerAngle.y >= Mathf.Epsilon - 0.7f && _controllerAngle.y <= Mathf.Epsilon + 0.7f)
        {
            //-0.7에서 0.7 사이의 값을 받아서 -1에서 1 사이의 값으로 변환
		    yaw		= Mathf.Clamp(_controllerAngle.y, Mathf.Epsilon - 0.7f, Mathf.Epsilon + 0.7f);
        }
        if(_controllerAngle.z >= Mathf.Epsilon - 0.7f && _controllerAngle.z <= Mathf.Epsilon + 0.7f)
        {
            //-0.7에서 0.7 사이의 값을 받아서 -1에서 1 사이의 값으로 변환
		    roll	= Mathf.Clamp(_controllerAngle.z, Mathf.Epsilon - 0.7f, Mathf.Epsilon + 0.7f) / 7 * 10;
        }
        //print(yaw);
		axis	= _controllerAngle.w;
		controlAngle = new Quaternion(pitch, yaw, roll, axis);

		booster = inputCheck.booster ? 1 : 0;  		//트리거 눌렀으면 1, 안 눌렀으면 0
		throttle = inputCheck.throttle;             //트랙패드 입력갱신
        BrkBtn = inputCheck.menu ? 1 : 0;           //메뉴버튼 눌렀으면 1, 안 눌렀으면 0
        

		thrustControl.GetStatus(status);    //비행 상태 갱신(추력제어 쪽의 상태 갱신)
		torqueControl.GetStatus(status);    //비행 상태 갱신(회전제어 쪽의 상태 갱신)


	}

	//비행상태 변경 요청받는 메서드
	public void SetStatus(AviationStatus stat)
	{
		status = stat;
	}
}
