using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.InputSystem;

public class OpeningSystem : MonoBehaviourPunCallbacks
{
    [SerializeField, Header("���͗p�e�L�X�g")] GameObject InputText;

    [SerializeField, Header("����SE")] AudioClip enterSE;

    AudioSource audioSource;

    private bool first = true;

    private void Start()
    {
        //�o�����̂�ς���
        if (PhotonNetwork.IsMasterClient)
        {
            InputText.transform.GetChild(0).GetComponent<Text>().text = "����";
        }
        else
        {
            InputText.transform.GetChild(0).GetComponent<Text>().text = "���[�h��...";
            InputText.transform.GetChild(1).gameObject.SetActive(false);
        }

        audioSource = GetComponent<AudioSource>();
    }

    [PunRPC]
    private void RpcSceneMove()
    {
        //�I�[�i�[�̎�
        if (PhotonNetwork.IsMasterClient)
        {
            audioSource.PlayOneShot(enterSE);

            ManagerAccessor.Instance.sceneMoveManager.SceneMoveName("StageSelect");
        }

        InputText.transform.GetChild(0).GetComponent<Text>().text = "���[�h��..."; 
        InputText.transform.GetChild(1).gameObject.SetActive(false);
    }

    //�R���g���[���[B����
    public void OnActionPressB(InputAction.CallbackContext context)
    {
        // �����ꂽ�u�Ԃ�Performed�ƂȂ�
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
        // �����ꂽ�u�Ԃ�Performed�ƂȂ�
        if (!context.performed) return;

    }
}
