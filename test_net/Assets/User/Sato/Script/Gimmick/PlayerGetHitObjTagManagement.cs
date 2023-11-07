using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGetHitObjTagManagement : MonoBehaviour
{
    [SerializeField, Header("右当たり判定")] private PlayerGetHitObjTag rightJudge;
    [SerializeField, Header("左当たり判定")] private PlayerGetHitObjTag leftJudge;
    [SerializeField, Header("下当たり判定")] private PlayerGetHitObjTag downJudge;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //右に当たっている判定
        for (int i = 0; i < rightJudge.HitTags.Count; i++)
        {
            if (rightJudge.HitTags.Count != 0 && rightJudge.HitTags[i] == "Gimmick") 
            {
                ManagerAccessor.Instance.dataManager.isHitRight = true;
            }
        }

        //左に当たっている判定
        for (int i = 0; i < leftJudge.HitTags.Count; i++)
        {
            if (leftJudge.HitTags.Count != 0 && leftJudge.HitTags[i] == "Gimmick")
            {
                ManagerAccessor.Instance.dataManager.isHitLeft = true;
            }
        }

        //下に当たっている判定
        for (int i = 0; i < downJudge.HitTags.Count; i++)
        {
            if (downJudge.HitTags.Count != 0 && downJudge.HitTags[i] == "Gimmick"|| downJudge.HitTags[i] == "Floor")
            {
                ManagerAccessor.Instance.dataManager.isHitDown = true;
            }
        }

        //あたっていないとき
        if (rightJudge.HitTags.Count == 0)
            ManagerAccessor.Instance.dataManager.isHitRight = false;
        if (leftJudge.HitTags.Count == 0)
            ManagerAccessor.Instance.dataManager.isHitLeft = false;
        if (downJudge.HitTags.Count == 0)
            ManagerAccessor.Instance.dataManager.isHitDown = false;

    }
}
