using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimmickFallStone : MonoBehaviour
{
    [SerializeField, Header("�������x")] private Vector3 speed;

    // Update is called once per frame
    void FixedUpdate()
    {
        //�|�[�Y���͎~�܂�
        if (!ManagerAccessor.Instance.dataManager.isClear &&
            !ManagerAccessor.Instance.dataManager.isDeth &&
            !ManagerAccessor.Instance.dataManager.isPause)
        {
            transform.position -= speed;
        }
    }
}
