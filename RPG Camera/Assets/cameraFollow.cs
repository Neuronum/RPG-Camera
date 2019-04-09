using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraFollow : MonoBehaviour
{
    public Transform player;
    public Transform camera_dest;

    private Vector3 _cameraOffset;
    private Vector3 rotateVec;
    private Vector3 newOffset; //暂时存储新计算出的cameraOffset，用来插值，已达到平滑效果
    private Vector3 destVec; // camera_dest.position - player.position
    private float countTime;


    [Range(0.01f, 1.0f)]
    public float SmoothFactor = 0.5f;

    [Range(0.01f, 1.0f)]
    public float cameraFollowSpeed = 0.01f;


    // Start is called before the first frame update
    void Start()
    {
        transform.position = camera_dest.position;
        transform.LookAt(player);
        _cameraOffset = transform.position - player.position;

        countTime = 0f;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //鼠标X轴控制方向上进行平滑
        newOffset = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * 10.0f, Vector3.up) * _cameraOffset;
        _cameraOffset = Vector3.Slerp(_cameraOffset, newOffset, SmoothFactor);

        //rotateVec 是世界坐标系中鼠标Y方向的旋转中心轴
        rotateVec = _cameraOffset;
        rotateVec.y = 0;
        rotateVec = Quaternion.AngleAxis(-90, Vector3.up) * rotateVec;

        //function Vector3.Slerp: Spherically interpolates between two vectors.
        //鼠标y轴上进行平滑
        newOffset = Quaternion.AngleAxis(-Input.GetAxis("Mouse Y") * 10.0f, rotateVec) * _cameraOffset;
        _cameraOffset = Vector3.Slerp(_cameraOffset, newOffset, SmoothFactor);

        

        if ( Input.GetAxis("Horizontal")!=0 || Input.GetAxis("Vertical") != 0)
        {
            countTime += Time.deltaTime;
            Debug.Log("countTIme: " + countTime);
            if(countTime > 1f)
            {
                destVec = camera_dest.position - player.position;
                _cameraOffset = Vector3.Slerp(_cameraOffset, destVec, cameraFollowSpeed);
                if (Vector3.Distance(_cameraOffset, destVec) < 0.25f)
                    countTime = 0;
            }
            
        }

        if (Input.GetAxis("Vertical") == 0 && Input.GetAxis("Horizontal") == 0)
            countTime = 0;

        transform.position = player.position + _cameraOffset;

        transform.LookAt(player);
    }
}
