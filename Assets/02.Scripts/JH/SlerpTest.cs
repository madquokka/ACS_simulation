using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlerpTest : MonoBehaviour
{
    public Transform t1;
    public Transform t2;

    float time = 0;

    bool check = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if(!check)
        //{
        //    transform.localRotation = Quaternion.Slerp(t1.rotation, t2.rotation, time);
        //    if (transform.localRotation == t2.localRotation)
        //    {
        //        check = true;
        //        time = 0;
        //    }
        //}
        //time = time + Time.deltaTime;

        //if(check)
        //{
        //    transform.localRotation = Quaternion.Slerp(t2.rotation, t1.rotation, time);
        //}


        transform.position = Vector3.Lerp(t1.position, t2.position, time);
        time += Time.deltaTime;
    }
}
