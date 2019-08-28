using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FrontGlassHUD : MonoBehaviour
{
    public Transform cam;

    public RawImage horizontalLine;
    public Transform VerticalLine;

    public float pitch;
    public float yaw;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //수직 각도 변경값 받기
        if(cam.localRotation.x > -0.7f && cam.localRotation.x < 0.7f)
        {
            pitch = cam.localRotation.x / 7 * 10;
        }
        //print(pitch * 800);

        //수직 각도 변경
        VerticalLine.localPosition = new Vector3(0, pitch * 800, 0);

        //print(cam.localRotation.eulerAngles.y);

        //수평 각도 변경값 받기
        yaw = cam.localRotation.eulerAngles.y / 360;

        //수평 각도 변경
        horizontalLine.uvRect = new Rect(yaw, 0, 1, 1);
        //print(yaw);
    }
}
