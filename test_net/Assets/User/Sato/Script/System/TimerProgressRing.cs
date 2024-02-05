using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class TimerProgressRing : MonoBehaviourPunCallbacks
{
    private Image circle;



    private void Start()
    {
        //データ取得
        circle = gameObject.GetComponent<Image>();
        circle.fillAmount = 0;
    }

    private void FixedUpdate()
    {

        if (!ManagerAccessor.Instance.dataManager.isClear &&
            !ManagerAccessor.Instance.dataManager.isDeth &&
            !ManagerAccessor.Instance.dataManager.isPause)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                DataManager dataManager = ManagerAccessor.Instance.dataManager;
                circle.fillAmount = 0;

                //十字キー下を押しているときのみ表示
                if (dataManager.isOwnerInputKey_C_D_DOWN)
                {
                    if (dataManager.player1.GetComponent<PlayerController>().movelock)
                        circle.fillAmount = 1.0f / 30.0f * dataManager.player1.GetComponent<PlayerController>().holdtime;
                }
            }
        }
    }
}