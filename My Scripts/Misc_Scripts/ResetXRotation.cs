//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class ResetXRotation : MonoBehaviour 
//{
//    void Start()
//    {
//        ResetTheXRotationToZero();
//    }

//    void ResetTheXRotationToZero()
//    {
//        Transform[] perimiter = GetComponentsInChildren<Transform>();

//        foreach(Transform t in perimiter)
//        {
//            t.localEulerAngles = new Vector3(Mathf.Abs(t.localEulerAngles.x)-t.localEulerAngles.x, t.localEulerAngles.y, t.localEulerAngles.z);
//        }
//    }

//}
