using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ButtonControlSystem : MonoBehaviourPunCallbacks
{
    [SerializeField, Header("�{�^�����W�I�u�W�F�N�g")] private GameObject[] Buttons;

    [SerializeField, Header("�A���_�[�o�[")] private GameObject underber;

    [SerializeField, Header("�c��true ����false")] private bool inputDirection;

    private DataManager dataManager;        //�f�[�^�}�l�[�W���[�擾�p

    private bool buttonNum = true;          //�{�^���̐�

    private bool isSelect = false;          //�{�^�����I�����ꂽ�Ƃ�

    //�A���œ���Ȃ��悤
    private bool first = true;
    private bool firstSceneMove = true;


    // Update is called once per frame
    void Update()
    {
        if (!isSelect)
        {
            dataManager = ManagerAccessor.Instance.dataManager;

            if (PhotonNetwork.IsMasterClient)
            {
                if (inputDirection)
                {
                    //�㉺����͂���ƃJ�[�\�����㉺����
                    if (dataManager.isOwnerInputKey_C_L_UP || dataManager.isOwnerInputKey_C_L_UP)
                    {
                        if (first)
                        {
                            buttonNum = !buttonNum;
                            first = false;
                        }
                    }
                    else
                        first = true;
                }
                else
                {
                    //���E����͂���ƃJ�[�\�������E����
                    if (dataManager.isOwnerInputKey_C_L_RIGHT || dataManager.isOwnerInputKey_C_L_LEFT)
                    {
                        if (first)
                        {
                            buttonNum = !buttonNum;
                            first = false;
                        }
                    }
                    else
                        first = true;
                }

                //�{�^���ɂ��������W�ƃT�C�Y�ɕύX
                if (buttonNum)
                {
                    RectTransform buttonTra = Buttons[0].GetComponent<RectTransform>();
                    Debug.Log(buttonTra.sizeDelta.y / 2);
                    underber.GetComponent<RectTransform>().position = new Vector2(buttonTra.position.x, buttonTra.position.y - buttonTra.sizeDelta.y / 2);
                    underber.GetComponent<RectTransform>().sizeDelta = new Vector2(buttonTra.sizeDelta.x, underber.GetComponent<RectTransform>().sizeDelta.y);
                }
                else
                {
                    RectTransform buttonTra = Buttons[1].GetComponent<RectTransform>();
                    underber.GetComponent<RectTransform>().position = new Vector2(buttonTra.position.x, buttonTra.position.y - buttonTra.sizeDelta.y / 2);
                    underber.GetComponent<RectTransform>().sizeDelta = new Vector2(buttonTra.sizeDelta.x, underber.GetComponent<RectTransform>().sizeDelta.y);
                }



                //�V�[���ړ�����
                if (dataManager.isOwnerInputKey_CB)
                {
                    if (firstSceneMove)
                    {
                        if (buttonNum)
                        {
                            ManagerAccessor.Instance.sceneMoveManager.SceneMoveRetry();
                            isSelect = true;
                            firstSceneMove = false;
                        }
                        else
                        {
                            ManagerAccessor.Instance.sceneMoveManager.SceneMoveName("StageSelect");
                            isSelect = true;
                            firstSceneMove = false;
                        }
                    }
                }
            }
        }
    }
}
