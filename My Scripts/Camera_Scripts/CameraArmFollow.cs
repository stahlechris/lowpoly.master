using UnityEngine;

namespace LowPoly.CameraUI
{
    public class CameraArmFollow : MonoBehaviour
    {
        Transform myTransform;
        #region CONST STRING FINALS
        const string PLAYER_TAG = "Player";
        const string MouseY = "Mouse Y";
        const string MouseX = "Mouse X";
        const string XboxHorizontal = "XboxHorizontal";
        const string XboxVertical = "XboxVertical";
#endregion
        public Transform player;

        float X;
        float Y;

        float X2;
        float Y2;

        void Awake()
        {
            //cache transform to eliminate extra step
            myTransform = transform;
        }
        void Start()
        {
            //player = GameObject.FindGameObjectWithTag(PLAYER_TAG);
            //cameraChanger = GetComponent<CameraChanger>();
            //Debug.Log("Camera Follow script found: " + player.name); this was my first debug message!!!!they grow up so fast
        }

        void LateUpdate()
        {
            //lock cam to player every frame-end
            myTransform.position = player.position;
            RotateCameraIfControlAndClickDown();
            RotateCameraIfRightThumbstick();
        }


        void RotateCameraIfControlAndClickDown()
        {
            if (!Global.HAVING_A_CONVERSATION)
            {
                if (Input.GetMouseButton(0) && Input.GetKey(KeyCode.LeftControl)) //if contrl + click
                {                                                       //sensitivity             //sensitivity
                    myTransform.Rotate(new Vector3(-Input.GetAxis(MouseY) * 3.5f, Input.GetAxis(MouseX) * 3.5f, 0));
                    X = myTransform.rotation.eulerAngles.x;
                    Y = myTransform.rotation.eulerAngles.y;
                    myTransform.rotation = Quaternion.Euler(X, Y, 0);
                }
            }
        }


        void RotateCameraIfRightThumbstick()
        {
            if (Global.XBOX_CONTROLLER_PLUGGED_IN)
            {
                myTransform.Rotate(new Vector3(-Input.GetAxis(XboxHorizontal) * 3.5f, Input.GetAxis(XboxVertical) * 3.5f, 0));
                X2 = myTransform.rotation.eulerAngles.x;
                Y2 = myTransform.rotation.eulerAngles.y;
                myTransform.rotation = Quaternion.Euler(X2, Y2, 0);
            }
        }
    }
}
