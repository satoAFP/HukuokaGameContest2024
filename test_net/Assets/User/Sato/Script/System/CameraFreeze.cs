using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFreeze : MonoBehaviour
{
    public Vector3 fixedRotation;

    void LateUpdate()
    {
        // �J�����̉�]���Œ�
        transform.rotation = Quaternion.Euler(fixedRotation);
    }
}
