using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GimmickRotateObject : MonoBehaviourPunCallbacks
{
    [SerializeField, Header("‰ñ“]‘¬“x")] private float RotateSpeed;

    [SerializeField, Header("‰ñ“]•ûŒü")] private bool RotateDir;

    private void FixedUpdate()
    {
        if (RotateDir)
            transform.eulerAngles += new Vector3(0, 0, RotateSpeed);
        else
            transform.eulerAngles -= new Vector3(0, 0, RotateSpeed);
    }

}
