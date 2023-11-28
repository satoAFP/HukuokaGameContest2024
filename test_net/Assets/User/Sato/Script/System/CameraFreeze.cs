using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFreeze : MonoBehaviour
{
    public Vector3 fixedRotation;

    void LateUpdate()
    {
        // ƒJƒƒ‰‚Ì‰ñ“]‚ğŒÅ’è
        transform.rotation = Quaternion.Euler(fixedRotation);
    }
}
