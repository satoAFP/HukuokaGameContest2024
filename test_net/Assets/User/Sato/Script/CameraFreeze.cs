using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFreeze : MonoBehaviour
{
    public Vector3 fixedRotation;

    void LateUpdate()
    {
        // カメラの回転を固定
        transform.rotation = Quaternion.Euler(fixedRotation);
    }
}
