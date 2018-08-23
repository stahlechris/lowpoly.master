using UnityEngine;

public class EyeGuard_UI : MonoBehaviour 
{
    public Transform stoneHengeCam;
    public void PrepareText()
    {
        TurnOffPlayerTracking();
        MakeTextFaceCamera();
    }

    void TurnOffPlayerTracking()
    {
        FaceWhoIsTargettingMe face = GetComponentInParent<FaceWhoIsTargettingMe>();
        face.enabled = false;
    }
    void MakeTextFaceCamera()
    {
        transform.LookAt(stoneHengeCam);
    }
}
