using UnityEngine;

namespace RPG.CameraUI
{
    public class CameraArmFollow : MonoBehaviour
    {
        const string PLAYER_TAG = "Player";
        const string MouseY = "Mouse Y";
        const string MouseX = "Mouse X";
        const string XboxHorizontal = "XboxHorizontal";
        const string XboxVertical = "XboxVertical";
        //CameraChanger cameraChanger;
        GameObject player;
        private float X;
        private float Y;

        private float X2;
        private float Y2;

        void Start()
        {
            player = GameObject.FindGameObjectWithTag(PLAYER_TAG);
            //cameraChanger = GetComponent<CameraChanger>();
            //Debug.Log("Camera Follow script found: " + player.name); this was my first debug message!!!!they grow up so fast

        }

        void LateUpdate()
        {
            this.transform.position = player.transform.position; //lock cam to player
            //this.transform.rotation = player.transform.rotation; 
            RotateCameraIfControlAndClickDown();
            RotateCameraIfRightThumbstick();    //these two let the player rotate the camera
        }


        void RotateCameraIfControlAndClickDown()
        {
            if (Input.GetMouseButton(0) && Input.GetKey(KeyCode.LeftControl)) //if contrl + click
            {
                transform.Rotate(new Vector3(-Input.GetAxis(MouseY) * 3.5f, Input.GetAxis(MouseX) * 3.5f, 0));
                X = transform.rotation.eulerAngles.x;
                Y = transform.rotation.eulerAngles.y;
                transform.rotation = Quaternion.Euler(X, Y, 0);
            }
        }

        void RotateCameraIfRightThumbstick()
        {
            transform.Rotate(new Vector3(-Input.GetAxis(XboxHorizontal) * 3.5f, Input.GetAxis(XboxVertical) * 3.5f, 0));
            X2 = transform.rotation.eulerAngles.x;
            Y2 = transform.rotation.eulerAngles.y;
            transform.rotation = Quaternion.Euler(X2, Y2, 0);
        }
    }
}
