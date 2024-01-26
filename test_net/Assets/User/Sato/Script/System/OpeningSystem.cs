using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.InputSystem;

public class OpeningSystem : MonoBehaviourPunCallbacks
{
    [SerializeField, Header("入力用テキスト")] GameObject InputText;

    [SerializeField, Header("決定SE")] AudioClip enterSE;

    AudioSource audioSource;

    private bool first = true;

    private void Start()
    {
        //出すものを変える
        if (PhotonNetwork.IsMasterClient)
        {
            InputText.transform.GetChild(0).GetComponent<Text>().text = "次へ";
        }
        else
        {
            InputText.transform.GetChild(0).GetComponent<Text>().text = "ロード中...";
            InputText.transform.GetChild(1).gameObject.SetActive(false);
        }

        audioSource = GetComponent<AudioSource>();
    }

    [PunRPC]
    private void RpcSceneMove()
    {
        //オーナーの時
        if (PhotonNetwork.IsMasterClient)
        {
            audioSource.PlayOneShot(enterSE);

            ManagerAccessor.Instance.sceneMoveManager.SceneMoveName("StageSelect");
        }

        InputText.transform.GetChild(0).GetComponent<Text>().text = "ロード中..."; 
        InputText.transform.GetChild(1).gameObject.SetActive(false);
    }

    //コントローラーB入力
    public void OnActionPressB(InputAction.CallbackContext context)
    {
        // 離された瞬間でPerformedとなる
        if (!context.performed) return;

        if (first)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                photonView.RPC(nameof(RpcSceneMove), RpcTarget.All);
                first = false;
            }
        }
    }
    public void OnActionReleaseB(InputAction.CallbackContext context)
    {
        // 離された瞬間でPerformedとなる
        if (!context.performed) return;

    }
}
