using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

// MonoBehaviourPunCallbacks���p�����āAphotonView�v���p�e�B���g����悤�ɂ���
public class AvatarController : MonoBehaviourPunCallbacks
{
    

    private void Start()
    {
        ManagerAccessor.Instance.dataManager.text.text = photonView.OwnerActorNr + ":" + PhotonNetwork.NickName + ":" + PhotonNetwork.IsMasterClient;
    }

    private void Update()
    {
        // ���g�����������I�u�W�F�N�g�����Ɉړ��������s��
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
