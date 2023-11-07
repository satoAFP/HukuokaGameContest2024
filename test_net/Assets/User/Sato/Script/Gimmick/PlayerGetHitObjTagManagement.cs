using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGetHitObjTagManagement : MonoBehaviour
{
    [SerializeField, Header("�E�����蔻��")] private PlayerGetHitObjTag rightJudge;
    [SerializeField, Header("�������蔻��")] private PlayerGetHitObjTag leftJudge;
    [SerializeField, Header("�������蔻��")] private PlayerGetHitObjTag downJudge;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //�E�ɓ������Ă��锻��
        for (int i = 0; i < rightJudge.HitTags.Count; i++)
        {
            if (rightJudge.HitTags.Count != 0 && rightJudge.HitTags[i] == "Gimmick") 
            {
                ManagerAccessor.Instance.dataManager.isHitRight = true;
            }
        }

        //���ɓ������Ă��锻��
        for (int i = 0; i < leftJudge.HitTags.Count; i++)
        {
            if (leftJudge.HitTags.Count != 0 && leftJudge.HitTags[i] == "Gimmick")
            {
                ManagerAccessor.Instance.dataManager.isHitLeft = true;
            }
        }

        //���ɓ������Ă��锻��
        for (int i = 0; i < downJudge.HitTags.Count; i++)
        {
            if (downJudge.HitTags.Count != 0 && downJudge.HitTags[i] == "Gimmick"|| downJudge.HitTags[i] == "Floor")
            {
                ManagerAccessor.Instance.dataManager.isHitDown = true;
            }
        }

        //�������Ă��Ȃ��Ƃ�
        if (rightJudge.HitTags.Count == 0)
            ManagerAccessor.Instance.dataManager.isHitRight = false;
        if (leftJudge.HitTags.Count == 0)
            ManagerAccessor.Instance.dataManager.isHitLeft = false;
        if (downJudge.HitTags.Count == 0)
            ManagerAccessor.Instance.dataManager.isHitDown = false;

    }
}
