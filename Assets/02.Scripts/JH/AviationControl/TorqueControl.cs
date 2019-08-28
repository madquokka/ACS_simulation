using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorqueControl : MonoBehaviour
{
	[Header("Aviation_status")]
	public AviationManager aviationManager;
	[HideInInspector] public AviationManager.AviationStatus status;

	[Header("Force_status")]
	public ThrustControl thrustControl;

	[Header("Fighter")]
	public Rigidbody fighterBody;

	[Header("Force values")]
	public float currForce		= 0f;				//현재 추력값
	public float TorqueResist	= 0f;				//회전항력

	[Header("Torque_values")]
	[HideInInspector] public Quaternion controlAngle;

	[Header("inputCheck")]
	public float pitchVal = 0;
	public float yawVal = 0;
	public float rollVal = 0;

	[Header("Speed_boundary")]
	[HideInInspector] public float _minSB;	//최소 속도경계
	[HideInInspector] public float _norSB;  //평균 속도경계
	[HideInInspector] public float _maxSB;  //최대 속도경계

	// Start is called before the first frame update
	void Start()
    {
		//속도경계 설정
		_minSB = aviationManager.minimumSpeedBoundary;
		_norSB = aviationManager.normalSpeedBoundary;
		_maxSB = aviationManager.maxSpeedBoundary;
	}

    // Update is called once per frame
    void Update()
    {
		controlAngle = aviationManager.controlAngle;
		currForce = thrustControl.currForce;

		switch(status)
		{
			case AviationManager.AviationStatus.BELOW_MINIMUM_SPEED:
				Torque(0);			//비행체 회전하기
				break;

			case AviationManager.AviationStatus.NORMAL_SPEED:
				Torque(1);
				break;

			case AviationManager.AviationStatus.AFTER_BURNER:
				Torque(2);
				break;
		}

		SetTorqueResist();			//회전저항값 구하기

		//print(controlAngle);
		//비행 상태값에 따른 회전각 계산
		//fighterBody.AddRelativeTorque(new Vector3(-(controlAngle.x + 0.4f) * 100000f
		//										, controlAngle.y * 10000f
		//										, -controlAngle.z * 10000)
		//										, ForceMode.Force);
		
    }

	private void SetTorqueResist()
	{
		TorqueResist = Mathf.Abs(controlAngle.x)
					 + Mathf.Abs(controlAngle.y)
					 + Mathf.Abs(controlAngle.z);
	}

	private void Torque(int stat)
	{
		
		switch(stat)
		{
			case 0:
                pitchVal = -controlAngle.x * 10000f
                            * (currForce / (_minSB / 5 * 3));
                yawVal = controlAngle.y * 2500f
                            * (currForce / (_minSB / 5 * 4));
                rollVal = -controlAngle.z * 2500f
                            * (currForce / (_minSB / 5 * 3));
                fighterBody.AddRelativeTorque(new Vector3(pitchVal
												, yawVal
                                                , rollVal)
												, ForceMode.Force);
				break;

			case 1:
                pitchVal    = -controlAngle.x * 30000f
                            * Mathf.Log((currForce / _minSB) + 1.3f + Mathf.Epsilon, 2);
				yawVal		= controlAngle.y * 3000f
							* Mathf.Log((currForce / _minSB) + 1.3f + Mathf.Epsilon, 2);
                rollVal		= -controlAngle.z * 5000f
							* Mathf.Log((currForce / _minSB) + 1.3f + Mathf.Epsilon, 2);
                fighterBody.AddRelativeTorque(new Vector3(pitchVal
												, yawVal
                                                , rollVal)
												, ForceMode.Force);
				break;

			case 2:
                float alpha = Mathf.Log(_norSB / _minSB, 2) + 1.3f;
                pitchVal = -controlAngle.x * 25000f
                            * alpha - Mathf.Log(alpha - currForce / _norSB);
				yawVal = controlAngle.y * 3000f
                            * alpha - Mathf.Log(alpha - currForce / _norSB);
                rollVal = -controlAngle.z * 5000f
                            * alpha - Mathf.Log(alpha - currForce / _norSB);
                fighterBody.AddRelativeTorque(new Vector3(pitchVal
												, yawVal
												, rollVal)
												, ForceMode.Force);
				break;

		}
	}

	public void GetStatus(AviationManager.AviationStatus stat)
	{
		status = stat;
	}
}
