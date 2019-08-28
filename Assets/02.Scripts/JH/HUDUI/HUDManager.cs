using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HUDManager : MonoBehaviour
{
    [Header("Fighter_body")]
    public Transform horizontalRotation;
    public Transform fighterBody;

    [Header("Reference_scripts")]
    public AviationManager aviationManager;
    public ThrustControl thrustControl;

    [Header("Horizontal_line")]
    public Transform GlobalHLine;
    public Transform LocalHLine;

    [Header("Texts")]
    public Text altitudeText;
    public Text speedText;
    public Text throttleText;

    [Header("navigation_arrow")]
    public Transform navArrow;

    [Header("target")]
    public Transform target;

    [Header("control_square")]
    public Transform controlSquare;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        altitudeText.text = "ALT " + fighterBody.position.y.ToString("N1");
        speedText.text = "SPD " + thrustControl.currForce.ToString("N1");
        throttleText.text = "THR " + aviationManager.throttle.ToString("N1");

        GlobalHLine.localRotation = Quaternion.Euler(0
                                               ,transform.localRotation.eulerAngles.y
                                               ,horizontalRotation.localRotation.eulerAngles.z);
        //fighterBody.rotation.eulerAngles.y
        LocalHLine.localRotation = Quaternion.Euler(0
                                                   ,transform.localRotation.eulerAngles.y
                                                   , fighterBody.localRotation.eulerAngles.z);

        navArrow.LookAt(target);
        controlSquare.localPosition = new Vector2(-((Mathf.Epsilon + aviationManager.roll) / 7 * 10 * 320)
                                                 , -((Mathf.Epsilon + aviationManager.pitch+0.4f) * 10 / 4 * 220) );
        //fighterBody.rotation.eulerAngles.y

    }
}
