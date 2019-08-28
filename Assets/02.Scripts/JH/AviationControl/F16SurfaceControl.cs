using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class F16SurfaceControl : MonoBehaviour
{
    #region Values
    [Header("Aviation status")]
    public AviationManager aviationManager;
    private float pitch;
    private float yaw;
    private float roll;

    [Header("turning_object")]
    public Transform VoletL;
    public Transform VoletR;
    public Transform AileronL;
    public Transform AileronR;
    public Transform ElevatorL;
    public Transform ElevatorR;
    public Transform Rudder;
    public Transform BrakeLT;
    public Transform BrakeLB;
    public Transform BrakeRT;
    public Transform BrakeRB;

    [Header("rolling_range")]
    //회전각도 범위
    private float VoletRange;
    private float AileronRange;
    private float ElevatorRange;
    private float RudderRange;
    private float BrakeRange;

    //사용 수학함수
    public float Epsilon;
    //public float PitchRollCheck;

    [Header("brake_roll_check")]
    //브레이크 회전 시간을 잡는 코드
    [HideInInspector] public float UpTime;
    [HideInInspector] public float DownTime;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        Epsilon = Mathf.Epsilon + 1 - 1;        //근사값 0으로 제대로 맞춰주기 위해 1 더하고 1 뺌
        //PitchRollCheck = 0;                   //pitch값과 roll값을 확인해서 보간줄 조건확인위한 변수

        VoletRange = Epsilon + 10;
        AileronRange = Epsilon + 25;
        ElevatorRange = Epsilon + 18;
        RudderRange = Epsilon + 25;
        BrakeRange = Epsilon + 35;

        //브레이크 올리고 내리는 타이머
        UpTime = 0;
        DownTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        pitch = -aviationManager.pitch;      //pitch값 가져옴    -1 ~ 1
        yaw = aviationManager.yaw;          //yaw값 가져옴      -1 ~ 1
        roll = aviationManager.roll;        //roll값 가져옴     -1 ~ 1
        
        VoletL.transform.localRotation = Quaternion.Euler(new Vector3(pitch * VoletRange, -34.747f, -1.253f));
        VoletR.transform.localRotation = Quaternion.Euler(new Vector3(pitch * VoletRange, 34.747f, 1.253f));

        AileronL.transform.localRotation = Quaternion.Euler(new Vector3((pitch * 2 + roll * 1.2f)/2 * AileronRange, -8.91f, -1.395f));
        AileronR.transform.localRotation = Quaternion.Euler(new Vector3((pitch * 2 + -roll * 1.2f)/2 * AileronRange, 8.91f, 1.395f));

        ElevatorL.transform.localRotation = Quaternion.Euler(new Vector3(-(pitch * 2 + roll * 1.2f)/2 * ElevatorRange, 0, 0));
        ElevatorR.transform.localRotation = Quaternion.Euler(new Vector3(-(pitch * 2 + -roll * 1.2f)/2 * ElevatorRange, 0, 0));

        Rudder.transform.localRotation = Quaternion.Euler(new Vector3(0, yaw * RudderRange, 0));

        BrakeCheck();
    }

    private void BrakeCheck()
    {
        if(aviationManager.BrkBtn == 1)
        {
            DownTime = 0;

            BrakeLT.transform.localRotation = Quaternion.Lerp(BrakeLT.transform.localRotation
                                                              , Quaternion.Euler(new Vector3(-BrakeRange, 0, -12.045f))
                                                              , UpTime);
            BrakeLB.transform.localRotation = Quaternion.Lerp(BrakeLB.transform.localRotation
                                                              , Quaternion.Euler(new Vector3(BrakeRange, 0, -12.045f))
                                                              , UpTime);
            BrakeRT.transform.localRotation = Quaternion.Lerp(BrakeRT.transform.localRotation
                                                              , Quaternion.Euler(new Vector3(-BrakeRange, 0, 12.045f))
                                                              , UpTime);
            BrakeRB.transform.localRotation = Quaternion.Lerp(BrakeRB.transform.localRotation
                                                              , Quaternion.Euler(new Vector3(BrakeRange, 0, 12.045f))
                                                              , UpTime);

            UpTime += Time.deltaTime;
        }
        else
        {
            UpTime = 0;

            BrakeLT.transform.localRotation = Quaternion.Lerp(BrakeLT.transform.localRotation
                                                              , Quaternion.Euler(new Vector3(0, 0, -12.045f))
                                                              , DownTime);
            BrakeLB.transform.localRotation = Quaternion.Lerp(BrakeLB.transform.localRotation
                                                              , Quaternion.Euler(new Vector3(0, 0, -12.045f))
                                                              , DownTime);
            BrakeRT.transform.localRotation = Quaternion.Lerp(BrakeRT.transform.localRotation
                                                              , Quaternion.Euler(new Vector3(0, 0, 12.045f))
                                                              , DownTime);
            BrakeRB.transform.localRotation = Quaternion.Lerp(BrakeRB.transform.localRotation
                                                              , Quaternion.Euler(new Vector3(0, 0, 12.045f))
                                                              , DownTime);

            DownTime += Time.deltaTime;
        }
    }
}
