using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPCamera : MonoBehaviour
{
    public Vector2 sense;
    public float distance, xOffSet, yOffSet;
    Transform cam;
    Vector2 turn;
    float CamPosy = 0;
    // Start is called before the first frame update
    void Start()
    {
        //TIRA ISSO DEPOIS CARA
        Cursor.lockState = CursorLockMode.Locked;
        cam = transform.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        cam.localPosition = new Vector3(xOffSet, yOffSet, -distance);
        turn.x += Input.GetAxis("Mouse X");
        turn.y = Input.GetAxis("Mouse Y");
        CamPosy += -turn.y * sense.y;
        if (CamPosy < -90){
            CamPosy = -90;
        }
        if (CamPosy > 90){
            CamPosy = 90;
        }
        transform.localRotation = Quaternion.Euler(CamPosy, turn.x * sense.x, 0);
    }
}
