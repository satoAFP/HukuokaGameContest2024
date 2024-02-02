using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GimmickRotateObject : MonoBehaviourPunCallbacks
{
    [SerializeField, Header("回転速度")] private float RotateSpeed;

    [SerializeField, Header("回転方向")] private bool RotateDir;

    private void FixedUpdate()
    {
        if (RotateDir)
            transform.eulerAngles += new Vector3(0, 0, RotateSpeed);
        else
            transform.eulerAngles -= new Vector3(0, 0, RotateSpeed);
    }

}
