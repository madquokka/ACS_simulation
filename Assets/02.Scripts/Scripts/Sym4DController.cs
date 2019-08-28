using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sym4D;

public class Sym4DController : MonoBehaviour
{
    public InputField portNo;
    public InputField roll;
    public InputField pitch;
    public InputField speed;

    private int xPort;  //시트 포트번호
    private int wPort;  //펜 포트번호
    private bool isWind = false;
    private readonly WaitForSeconds ws = new WaitForSeconds(0.1f);

    IEnumerator Start()
    {
        roll.text = "0";
        pitch.text = "0";
        speed.text = "0";

        //시트 포트번호 추출
        xPort = Sym4DEmulator.Sym4D_X_Find();
        portNo.text = xPort.ToString();
        yield return ws;

        //펜 포트번호 추출
        wPort = Sym4DEmulator.Sym4D_W_Find();
        yield return ws;

        //시트의 Roll, Pitch의 최대 회전각도(10도, 10도)
        Sym4DEmulator.Sym4D_X_SetConfig(100, 100);
        yield return ws;

        //펜의 최대회전수 (Max 100)
        Sym4DEmulator.Sym4D_W_SetConfig(100);
        yield return ws;
    }

    //시트 초기화(동작 정지)
    public void OnBodyInit()
    {
        Sym4DEmulator.Sym4D_X_EndContents();
    }

    //펜 초기화(동작 정지)
    public void OnPenInit()
    {
        Sym4DEmulator.Sym4D_W_EndContents();
    }

    //동작
    public void OnSetMotion()
    {
        StartCoroutine(SetMotion());
    }

    //동작2
    public void OnSetMotion2()
    {
        StartCoroutine(SetMotion2());
    }

    IEnumerator SetMotion()
    {
        int mRoll = int.Parse(roll.text);
        int mPitch = int.Parse(pitch.text);
        
        //동작시작
        Sym4DEmulator.Sym4D_X_StartContents(xPort);
        yield return ws;

        //동작명령
        Sym4DEmulator.Sym4D_X_SendMosionData(mRoll, mPitch);
        yield return ws;
    }

    IEnumerator SetMotion2()
    {
        int mRoll = int.Parse(roll.text);
        int mPitch = int.Parse(pitch.text);
        int mSpeed = int.Parse(speed.text);

        Sym4DEmulator.Sym4D_X_StartContents(xPort);
        yield return ws;

        Sym4DEmulator.Sym4D_X_SendMosionData2(mRoll, mPitch, mSpeed);
        yield return ws;
    }

    public void OnSetWind()
    {
        StartCoroutine(SetWind());
    }

    IEnumerator SetWind()
    {
        Sym4DEmulator.Sym4D_W_StartContents(wPort);
        yield return new WaitForSeconds(0.5f);

        isWind = !isWind;
        if (isWind)
        {
            Sym4DEmulator.Sym4D_W_SendMosionData(100);
        }
        else
        {
            Sym4DEmulator.Sym4D_W_SendMosionData(0);
            Sym4DEmulator.Sym4D_W_EndContents();
        }
    }
}
