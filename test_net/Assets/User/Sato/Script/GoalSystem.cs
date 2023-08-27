using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class GoalSystem : MonoBehaviourPunCallbacks
{
    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            //オーナーの時
            if (PhotonNetwork.IsMasterClient)
            {
                ManagerAccessor.Instance.dataManager.GetSetIsOwnerClear = true;
            }
            else
            {
                ManagerAccessor.Instance.dataManager.GetSetIsClientClear = true;
            }
        }
    }

}
