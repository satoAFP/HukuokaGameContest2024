using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGetHitObjTagManagement : MonoBehaviour
{
    [SerializeField, Header("‰E“–‚½‚è”»’è")] private PlayerGetHitObjTag rightJudge;
    [SerializeField, Header("¶“–‚½‚è”»’è")] private PlayerGetHitObjTag leftJudge;
    [SerializeField, Header("‰º“–‚½‚è”»’è")] private PlayerGetHitObjTag downJudge;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //‰E‚É“–‚½‚Á‚Ä‚¢‚é”»’è
        for (int i = 0; i < rightJudge.HitTags.Count; i++)
        {
            if (rightJudge.HitTags.Count != 0 && rightJudge.HitTags[i] == "Gimmick") 
            {
                ManagerAccessor.Instance.dataManager.isHitRight = true;
            }
        }

        //¶‚É“–‚½‚Á‚Ä‚¢‚é”»’è
        for (int i = 0; i < leftJudge.HitTags.Count; i++)
        {
            if (leftJudge.HitTags.Count != 0 && leftJudge.HitTags[i] == "Gimmick")
            {
                ManagerAccessor.Instance.dataManager.isHitLeft = true;
            }
        }

        //‰º‚É“–‚½‚Á‚Ä‚¢‚é”»’è
        for (int i = 0; i < downJudge.HitTags.Count; i++)
        {
            if (downJudge.HitTags.Count != 0 && downJudge.HitTags[i] == "Gimmick"|| downJudge.HitTags[i] == "Floor")
            {
                ManagerAccessor.Instance.dataManager.isHitDown = true;
            }
        }

        //‚ ‚½‚Á‚Ä‚¢‚È‚¢‚Æ‚«
        if (rightJudge.HitTags.Count == 0)
            ManagerAccessor.Instance.dataManager.isHitRight = false;
        if (leftJudge.HitTags.Count == 0)
            ManagerAccessor.Instance.dataManager.isHitLeft = false;
        if (downJudge.HitTags.Count == 0)
            ManagerAccessor.Instance.dataManager.isHitDown = false;

    }
}
