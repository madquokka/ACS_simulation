using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterSurfacesControl : MonoBehaviour
{
	#region public_values
	[Header("Aviation_status")]
	public AviationManager aviationManager;

	[Header("using_pointer_texture")]
    public Texture2D tex;

	[Header("turning_object")]
	public Transform rightElevator;
	public Transform leftElevator;
    public Transform rightFlap, leftFlap;
    public Transform rightRudder, leftRudder;

	[Header("detail_values")]
	public float MRx;
	public float MRy, MRz;
    public float maxAngle, flapMaxAngle, rudderMaxAngle;
	public float rightElevatorSetAngle, leftElevatorSetAngle;
	#endregion

	//public bool turnFlap;
	//public float turnSpeed;
	//private float angleX;

	#region Start
	// Use this for initialization
	void Start () {
		
	}
	#endregion

	#region Update
	// Update is called once per frame
	void Update () {
		//컨트롤러 회전값 받기
        MRx = Mathf.Clamp(aviationManager._controllerAngle.x, Mathf.Epsilon - 0.8f, Mathf.Epsilon);
		MRy = Mathf.Clamp(aviationManager._controllerAngle.y, Mathf.Epsilon - 0.7f, Mathf.Epsilon + 0.7f);
		MRz = Mathf.Clamp(aviationManager._controllerAngle.z, Mathf.Epsilon - 0.7f, Mathf.Epsilon + 0.7f);

        rightElevator.transform.localRotation = Quaternion.Euler(new Vector3((MRz + MRx+0.2f) * maxAngle, rightElevatorSetAngle, 0));
        leftElevator.transform.localRotation = Quaternion.Euler(new Vector3((-MRz + MRx+0.2f) * maxAngle, leftElevatorSetAngle, 0));

        rightFlap.transform.localRotation = Quaternion.Euler(new Vector3( -(MRz + MRx+0.4f) * flapMaxAngle, 6, 5));
        leftFlap.transform.localRotation = Quaternion.Euler(new Vector3( -(-MRz + MRx+0.4f) * flapMaxAngle, -6, -5));

        TransAng(rightRudder, 16, MRy * rudderMaxAngle, 107);
        TransAng(leftRudder, -16, MRy * rudderMaxAngle, 73);
		
		
		////x축 변환가능
		//if (MRx < 0 && MRx > -0.8f)
  //      {
  //          if(MRy < 0.75f && MRy > -0.65f)
  //          {
  //              if(MRz < 0.49f && MRz > -0.49f)
  //              {
  //              }
  //          }
  //      }

		//혹시몰라서 값 남겨둠
        //   if (turnFlap)
        //{
        //       angleX = Mathf.Lerp(angleX, 1, turnSpeed * Time.deltaTime);
        //       TransAng(rightFlap, 6, angleX * flapMaxAngle, 5);
        //       TransAng(leftFlap, -6, angleX * flapMaxAngle, -5);
        //}
        //else
        //{
        //       angleX = Mathf.Lerp(angleX, 0, turnSpeed * Time.deltaTime);
        //       TransAng(rightFlap, 6, angleX * flapMaxAngle, 5);
        //       TransAng(leftFlap, -6, angleX * flapMaxAngle, -5);
        //}
        
	}
	#endregion

	#region TransAng
	float c1, c2, c3, s1, s2, s3;
    float w, x, y, z;

    void TransAng(Transform tr, float heading, float attitude, float bank)
    {
        c1 = Mathf.Cos(heading * Mathf.Deg2Rad / 2);
        s1 = Mathf.Sin(heading * Mathf.Deg2Rad / 2);
        c2 = Mathf.Cos(attitude * Mathf.Deg2Rad / 2);
        s2 = Mathf.Sin(attitude * Mathf.Deg2Rad / 2);
        c3 = Mathf.Cos(bank * Mathf.Deg2Rad / 2);
        s3 = Mathf.Sin(bank * Mathf.Deg2Rad / 2);

        w = c1 * c2 * c3 - s1 * s2 * s3;
        x = s2 * c1 * c3 + s1 * c2 * s3;
        y = s1 * c2 * c3 + s2 * c1 * s3;
        z = s3 * c1 * c2 - s1 * s2 * c3;

        tr.transform.localRotation = new Quaternion(x, y, z, w);
    }
	#endregion

	#region OnGuI
	//텍스쳐 그리기
	void OnGUI()
    {
        GUI.DrawTexture(new Rect(Screen.width * 0.5f * (-MRz + 1f) - 8, Screen.height * 0.5f * (-MRx + 1f) - 8, 16f, 16f), tex);
    }
	#endregion
}
