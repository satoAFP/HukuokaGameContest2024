using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerGetHitObjTagManagement : MonoBehaviourPunCallbacks
{
    [SerializeField, Header("‰E“–‚½‚è”»’è")] private PlayerGetHitObjTag rightJudge;
    [SerializeField, Header("¶“–‚½‚è”»’è")] private PlayerGetHitObjTag leftJudge;
    [SerializeField, Header("‰º“–‚½‚è”»’è")] private PlayerGetHitObjTag downJudge;

    //“®‚©‚·‚©‚Ç‚¤‚©
    [System.NonSerialized] public bool isMotion = true;

    // Update is called once per frame
    void Update()
    {
        if (isMotion)
        {
            //‰E‚É“–‚½‚Á‚Ä‚¢‚é”»’è
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

            //¶‚É“–‚½‚Á‚Ä‚¢‚é”»’è
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

            //‰º‚É“–‚½‚Á‚Ä‚¢‚é”»’è
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

            //‚ ‚½‚Á‚Ä‚¢‚È‚¢‚Æ‚«
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
                    ManagerAccessor.Instance.dataManager.isClientHitDown = false;
            }
        }
    }
}
