using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ButtonControlSystem : MonoBehaviourPunCallbacks
{
    [SerializeField, Header("�{�^�����W�I�u�W�F�N�g")] private GameObject[] Buttons;

    [SerializeField, Header("�A���_�[�o�[")] private GameObject underber;

    [SerializeField, Header("�c��true ����false")] private bool inputDirection;

    private bool buttonNum = true;

    private bool first = true;
    private bool firstSceneMove = true;


    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (inputDirection)
            {
                //�㉺����͂���ƃJ�[�\�����㉺����
                if (ManagerAccessor.Instance.dataManager.isOwnerInputKey_C_L_UP || ManagerAccessor.Instance.dataManager.isOwnerInputKey_C_L_UP)
                {
                    if (first)
                    {
                        buttonNum = !buttonNum;
                        first = false;
                    }
                    else
                        first = true;
                }
            }
            else
            {
                //���E����͂���ƃJ�[�\�������E����
                if (ManagerAccessor.Instance.dataManager.isOwnerInputKey_CA /*|| ManagerAccessor.Instance.dataManager.isOwnerInputKey_C_L_LEFT*/)
                {
                    if (first)
                    {
                        Debug.Log("aaa");
                        buttonNum = !buttonNum;
                        first = false;
                    }
                    else
                    {
                        Debug.Log("bbb");
                        first = true;
                    }
                }
            }

            
            if (buttonNum)
            {
                underber.GetComponent<RectTransform>().position = Buttons[0].GetComponent<RectTransform>().position;
                underber.GetComponent<RectTransform>().sizeDelta = new Vector2(Buttons[0].GetComponent<RectTransform>().sizeDelta.x, underber.GetComponent<RectTransform>().sizeDelta.y);
            }
            else
            {
                underber.GetComponent<RectTransform>().position = Buttons[0].GetComponent<RectTransform>().position;
                underber.GetComponent<RectTransform>().sizeDelta = new Vector2(Buttons[0].GetComponent<RectTransform>().sizeDelta.x, underber.GetComponent<RectTransform>().sizeDelta.y);
            }



            //�V�[���ړ�����
            if (ManagerAccessor.Instance.dataManager.isOwnerInputKey_B)
            {
                if (firstSceneMove)
                {
                    if (buttonNum)
                    {
                        ManagerAccessor.Instance.sceneMoveManager.SceneMoveRetry();
                        firstSceneMove = false;
                    }
                    else
                    {
                        ManagerAccessor.Instance.sceneMoveManager.SceneMoveName("StageSelect");
                        firstSceneMove = false;
                    }
                }
            }
        }
    }
}
