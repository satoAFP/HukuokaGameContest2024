using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimmickFallStone : MonoBehaviour
{
    [SerializeField, Header("�������x")] public Vector3 Speed;

    [SerializeField, Header("������܂ł̃t���[��")] private int DeleteTime;

    private int frameCount = 0;

    // Update is called once per frame
    void FixedUpdate()
    {
        //�|�[�Y���͎~�܂�
        if (!ManagerAccessor.Instance.dataManager.isPause)
        {
            transform.position -= Speed;

            //��莞�Ԃŏ�����
            frameCount++;
            if (frameCount == DeleteTime) 
            {
                Destroy(gameObject);
            }
        }

        //�N���A�������͎��S��������
        if (ManagerAccessor.Instance.dataManager.isClear &&
            ManagerAccessor.Instance.dataManager.isDeth)
        {
            Destroy(gameObject);
        }
    }
}
