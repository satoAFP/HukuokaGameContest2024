using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

// MonoBehaviourPunCallbacksを継承して、photonViewプロパティを使えるようにする
public class AvatarController : MonoBehaviourPunCallbacks
{
    

    private void Start()
    {
        ManagerAccessor.Instance.dataManager.text.text = photonView.OwnerActorNr + ":" + PhotonNetwork.NickName + ":" + PhotonNetwork.IsMasterClient;
    }

    private void Update()
    {
        // 自身が生成したオブジェクトだけに移動処理を行う
        if (photonView.IsMine)
        {
            //var input = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0f);
            //transform.Translate(6f * Time.deltaTime * input.normalized);
        }
        
        
        //var players = PhotonNetwork.PlayerList;
        //var test = GameObject.FindWithTag("Finish").GetComponent<AvatarContainer>().GetEnumerator();

        //int a = 0;

        //while (test.MoveNext())
        //{
        //    a++;
        //}


        //if (PhotonNetwork.LocalPlayer.IsLocal)
        //{
        //    text.text = "Local:" + PhotonNetwork.LocalPlayer.ActorNumber + ":" + PhotonNetwork.LocalPlayer.NickName + ":" + a;
        //}

        //if (PhotonNetwork.LocalPlayer.IsMasterClient)
        //{
        //    text.text = "Master:" + PhotonNetwork.LocalPlayer.ActorNumber + ":" + PhotonNetwork.LocalPlayer.NickName + ":" + a;
        

    }
}
