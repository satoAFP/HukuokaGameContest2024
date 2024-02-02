using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimmickFallStone : MonoBehaviour
{
    [SerializeField, Header("落下速度")] private Vector3 speed;

    // Update is called once per frame
    void FixedUpdate()
    {
        //ポーズ中は止まる
        if (!ManagerAccessor.Instance.dataManager.isClear &&
            !ManagerAccessor.Instance.dataManager.isDeth &&
            !ManagerAccessor.Instance.dataManager.isPause)
        {
            transform.position -= speed;
        }
    }
}
