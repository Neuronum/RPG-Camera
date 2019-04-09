using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    public Transform main_camera;

    [Range(0.1f, 2.0f)]
    public float speed = 0.1f; //movement speed

    [Range(0.1f, 1.0f)]
    public float rotateSpeed = 0.1f;

    private Vector3 horiVec;  //相对摄像机左右走的方向
    private Vector3 vertVec;  //相对摄像机朝前走的方向
    private Vector3 cubeForward; //the Direction of cube's front
    private Vector3 cubeOrientation;
    private float vInput;
    private float hInput;

    // Start is called before the first frame update
    void Start()
    {
        cubeForward = transform.position - main_camera.position;
        cubeForward.y = 0;
        cubeOrientation = cubeForward = Vector3.Normalize(cubeForward);


        Quaternion q1 = new Quaternion(Mathf.Sin(Mathf.PI / 12.0f),0f,0f,Mathf.Cos(Mathf.PI / 12.0f));
        Quaternion q2 = new Quaternion(0, Mathf.Sin(Mathf.PI / 6.0f) * Mathf.Cos(Mathf.PI / 6.0f),  Mathf.Sin(Mathf.PI / 6.0f) * Mathf.Sin(Mathf.PI / 6.0f), Mathf.Cos(Mathf.PI / 6.0f));
        Debug.Log("Cube Rotation: " + transform.rotation);
        Debug.Log("Cube Rotation: " + transform.eulerAngles);
        //Debug.Log("Cube Rotation Euler Angles: " + Quaternion.EulerAngles(transform.rotation));
        Debug.Log("Its Quaternion should be" + q2*q1);
        Debug.Log("y axis is" + q1*Vector3.up);
        Debug.Log("y axis should be" + Quaternion.AngleAxis(30, new Vector3(1f, 0, 0)) * Vector3.up);
        Debug.Log("q2 should be " + Quaternion.AngleAxis(60, new Vector3(0, 0.85f, 0.5f)));
        Debug.Log("q2 actually is " + q2);
        Debug.Log("Vector forward" + Vector3.forward);
        Debug.Log("Vector up" + Vector3.up);
        Debug.Log(Quaternion.AngleAxis(60, Quaternion.AngleAxis(30, new Vector3(1f, 0, 0)) * Vector3.up) * Quaternion.AngleAxis(30, new Vector3(1f, 0, 0)));
        Debug.Log("Rotate Order: Y-> X -> Z" + Quaternion.AngleAxis(30, Quaternion.AngleAxis(60, Vector3.up) * new Vector3(1f,0,0)) * Quaternion.AngleAxis(60, Vector3.up));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //计算相对摄像机朝前走的方向, 单位向量
        vertVec = transform.position - main_camera.position;
        vertVec.y = 0;
        vertVec = Vector3.Normalize(vertVec);

        //计算相对摄像机左右走的方向, 旋转vertVec 90度就可以了
        horiVec = Quaternion.AngleAxis(90, Vector3.up) * vertVec;

        cubeOrientation = transform.rotation * cubeForward; // 计算方块朝向
        //keycode down w or s
        if ( (vInput = Input.GetAxis("Vertical") ) != 0) {
            
            transform.position += vertVec * speed * vInput;
            cubeOrientation = Vector3.Slerp(cubeOrientation, Mathf.Sign(vInput) * vertVec, rotateSpeed);
            Debug.Log("Cube Orientation: " + cubeOrientation);
            Debug.Log("Verticle Vector: " + vertVec);
            transform.rotation = Quaternion.FromToRotation(cubeForward, cubeOrientation);

        }

        //keycode down a or d
        if ((hInput = Input.GetAxis("Horizontal")) != 0) {

            transform.position += horiVec * speed * hInput;
            cubeOrientation = Vector3.Slerp(cubeOrientation, Mathf.Sign(hInput) * horiVec, rotateSpeed);
            transform.rotation = Quaternion.FromToRotation(cubeForward, cubeOrientation);

        }
    }

}
