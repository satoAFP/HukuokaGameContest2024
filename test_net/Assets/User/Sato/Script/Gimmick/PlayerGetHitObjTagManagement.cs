using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerGetHitObjTagManagement : MonoBehaviourPunCallbacks
{
    [SerializeField, Header("右当たり判定")] private PlayerGetHitObjTag rightJudge;
    [SerializeField, Header("左当たり判定")] private PlayerGetHitObjTag leftJudge;
    [SerializeField, Header("下当たり判定")] private PlayerGetHitObjTag downJudge;

    //動かすかどうか
    [System.NonSerialized] public bool isMotion = true;

    // Update is called once per frame
    void Update()
    {
        if (isMotion)
        {
            //右に当たっている判定
            for (int i = 0; i < rightJudge.HitTags.Count; i++)
            {
                if (rightJudge.HitTags.Count != 0 && rightJudge.HitTags[i] == "Gimmick")
                {
                    if ((PhotonNetwork.IsMasterClient && photonView.IsMine) || (!PhotonNetwork.IsMasterClient && !photonView.IsMine))
                    {
                        ManagerAccessor.Instance.dataManager.isOwnerHitRight = true;
                    }
                    else
                    {
                        ManagerAccessor.Instance.dataManager.isClientHitRight = true;
                    }
                }
            }

            //左に当たっている判定
            for (int i = 0; i < leftJudge.HitTags.Count; i++)
            {
                if (leftJudge.HitTags.Count != 0 && leftJudge.HitTags[i] == "Gimmick")
                {
                    if ((PhotonNetwork.IsMasterClient && photonView.IsMine) || (!PhotonNetwork.IsMasterClient && !photonView.IsMine))
                    {
                        ManagerAccessor.Instance.dataManager.isOwnerHitLeft = true;
                    }
                    else
                    {
                        ManagerAccessor.Instance.dataManager.isClientHitLeft = true;
                    }
                }
            }

            //下に当たっている判定
            for (int i = 0; i < downJudge.HitTags.Count; i++)
            {
                if (downJudge.HitTags.Count != 0 && downJudge.HitTags[i] == "Floor" || downJudge.HitTags[i] == "Gimmick") 
                {
                    if ((PhotonNetwork.IsMasterClient && photonView.IsMine) || (!PhotonNetwork.IsMasterClient && !photonView.IsMine))
                    {
                        ManagerAccessor.Instance.dataManager.isOwnerHitDown = true;
                    }
                    else
                    {
                        ManagerAccessor.Instance.dataManager.isClientHitDown = true;
                    }
                }
            }

            //あたっていないとき
            if ((PhotonNetwork.IsMasterClient && photonView.IsMine) || (!PhotonNetwork.IsMasterClient && !photonView.IsMine))
            {
                if (rightJudge.HitTags.Count == 0)
                    ManagerAccessor.Instance.dataManager.isOwnerHitRight = false;
                if (leftJudge.HitTags.Count == 0)
                    ManagerAccessor.Instance.dataManager.isOwnerHitLeft = false;
                if (downJudge.HitTags.Count == 0)
                {   
                    ManagerAccessor.Instance.dataManager.isOwnerHitDown = false;
                }
            }
            else
            {
                if (rightJudge.HitTags.Count == 0)
                    ManagerAccessor.Instance.dataManager.isClientHitRight = false;
                if (leftJudge.HitTags.Count == 0)
                    ManagerAccessor.Instance.dataManager.isClientHitLeft = false;
                if (downJudge.HitTags.Count == 0)
                {
                    ManagerAccessor.Instance.dataManager.isClientHitDown = false;
                }
                    
            }
        }
    }
}
