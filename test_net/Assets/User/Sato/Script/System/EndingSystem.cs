using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.InputSystem;

public class EndingSystem : MonoBehaviourPunCallbacks
{
    private bool isAniEnd = false;  //�A�j���[�V�����I������

    private bool first = true;

    [SerializeField, Header("����SE")] AudioClip enterSE;

    AudioSource audioSource;

    private void Start()
    {
        //�o�����̂�ς���
        gameObject.transform.GetChild(1).gameObject.SetActive(false);
        gameObject.transform.GetChild(2).gameObject.SetActive(false);

        //�o�����̂�ς���
        if (PhotonNetwork.IsMasterClient)
        {
            gameObject.transform.GetChild(1).GetComponent<Text>().text = "�^�C�g����";
        }
        else
        {
            gameObject.transform.GetChild(1).GetComponent<Text>().text = "1P����҂�";
        }

        audioSource = GetComponent<AudioSource>();
    }

    //�R���g���[���[B����
    public void OnActionPressB(InputAction.CallbackContext context)
    {
        // �����ꂽ�u�Ԃ�Performed�ƂȂ�
        if (!context.performed) return;

        if (isAniEnd)
        {
            if (first)
            {
                //�V�[�����擾
                GlobalSceneName.SceneName = ManagerAccessor.Instance.sceneMoveManager.GetSceneName();
                ManagerAccessor.Instance.sceneMoveManager.SceneMoveName("NewTitle");

                audioSource.PlayOneShot(enterSE);

                first = false;
            }
        }
    }
    public void OnActionReleaseB(InputAction.CallbackContext context)
    {
        // �����ꂽ�u�Ԃ�Performed�ƂȂ�
        if (!context.performed) return;

    }

    public void OnAnimationEnd()
    {
        //�o�����̂�ς���
        if (PhotonNetwork.IsMasterClient)
        {
            gameObject.transform.GetChild(1).gameObject.SetActive(true);
            gameObject.transform.GetChild(2).gameObject.SetActive(true);
        }
        else
        {
            gameObject.transform.GetChild(1).gameObject.SetActive(true);
        }

        //�A�j���[�V�����I�����̏���
        isAniEnd = true;
    }
}
