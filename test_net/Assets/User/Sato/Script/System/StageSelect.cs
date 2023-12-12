using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class StageSelect : MonoBehaviourPunCallbacks
{

    [SerializeField, Header("�t�F�[�h���x")] private int FeedSpeed;

    [SerializeField, Header("�ړ�����V�[����")] private string sceneName;


    private bool isOwnerEnter = false;  //P1����������
    private bool isClientEnter = false; //P2����������

    //�S�[���ɐG��Ă��邩�ǂ���
    private bool isOwnerFadeStart = false;
    private bool isClientFadeStart = false;

    private bool first = true;


    // Update is called once per frame
    void FixedUpdate()
    {
        //�X�e�[�W�ɓ���
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
            MoveStage(isOwnerEnter, ManagerAccessor.Instance.dataManager.isOwnerInputKey_C_D_UP);
        else
            MoveStage(isClientEnter, ManagerAccessor.Instance.dataManager.isClientInputKey_C_D_UP);

        //�S�[����̃t�F�[�h����
        if (isOwnerFadeStart)
        {
            //�����ׂ��{�^���̉摜��\��
            ManagerAccessor.Instance.dataManager.player1.transform.GetChild(0).gameObject.SetActive(false);
            Debug.Log("aaa");
            if (ManagerAccessor.Instance.dataManager.player1.transform.Find("PlayerImage").GetComponent<SpriteRenderer>().color.a > 0)
                ManagerAccessor.Instance.dataManager.player1.transform.Find("PlayerImage").GetComponent<SpriteRenderer>().color -= new Color32(0, 0, 0, (byte)FeedSpeed);
        }

        if (isClientFadeStart)
        {
            //�����ׂ��{�^���̉摜��\��
            ManagerAccessor.Instance.dataManager.player2.transform.GetChild(0).gameObject.SetActive(false);
            Debug.Log("aaa");
            if (ManagerAccessor.Instance.dataManager.player2.transform.Find("PlayerImage").GetComponent<SpriteRenderer>().color.a > 0)
                ManagerAccessor.Instance.dataManager.player2.transform.Find("PlayerImage").GetComponent<SpriteRenderer>().color -= new Color32(0, 0, 0, (byte)FeedSpeed);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player1")
        {
            if (PhotonNetwork.IsMasterClient)
            {
                //�����ׂ��{�^���̉摜�\��
                collision.transform.GetChild(0).gameObject.SetActive(true);
                collision.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = ManagerAccessor.Instance.spriteManager.CrossUp;

                photonView.RPC(nameof(RpcShareIsNotOpenBox), RpcTarget.All, true, true);
                photonView.RPC(nameof(RpcShareIsOwnerEnter), RpcTarget.All, true);
            }
        }

        if (collision.gameObject.name == "Player2")
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                //�����ׂ��{�^���̉摜�\��
                collision.transform.GetChild(0).gameObject.SetActive(true);
                collision.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = ManagerAccessor.Instance.spriteManager.CrossUp;

                photonView.RPC(nameof(RpcShareIsNotOpenBox), RpcTarget.All, false, true);
                photonView.RPC(nameof(RpcShareIsClientEnter), RpcTarget.All, true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player1")
        {
            //�����ׂ��{�^���̉摜��\��
            collision.transform.GetChild(0).gameObject.SetActive(false);

            photonView.RPC(nameof(RpcShareIsNotOpenBox), RpcTarget.All, true, false);
            photonView.RPC(nameof(RpcShareIsOwnerEnter), RpcTarget.All, false);

        }

        if (collision.gameObject.name == "Player2")
        {
            //�����ׂ��{�^���̉摜��\��
            collision.transform.GetChild(0).gameObject.SetActive(false);

            photonView.RPC(nameof(RpcShareIsNotOpenBox), RpcTarget.All, false, false);
            photonView.RPC(nameof(RpcShareIsClientEnter), RpcTarget.All, false);

        }
    }

    //�V�[���؂�ւ���񋤗L
    [PunRPC]
    private void RpcShareIsOwnerEnter(bool data)
    {
        isOwnerEnter = data;
    }

    //�V�[���؂�ւ���񋤗L
    [PunRPC]
    private void RpcShareIsClientEnter(bool data)
    {
        isClientEnter = data;
    }

    //�V�[���؂�ւ���񋤗L
    [PunRPC]
    private void RpcShareStart()
    {
        if (PhotonNetwork.IsMasterClient)
            ManagerAccessor.Instance.sceneMoveManager.SceneMoveName(sceneName);
    }

    //feed���L
    [PunRPC]
    private void RpcShareFadeStart(bool isOwner)
    {
        if (isOwner)
        {
            isOwnerFadeStart = true;
        }
        else
        {
            isClientFadeStart = true;
        }
    }


    [PunRPC]
    private void RpcShareIsNotOpenBox(bool isOwner, bool data)
    {
        if (isOwner)
            ManagerAccessor.Instance.dataManager.isOwnerNotOpenBox = data;
        else
            ManagerAccessor.Instance.dataManager.isClientNotOpenBox = data;
    }

    /// <summary>
    /// �X�e�[�W�Ɉړ�
    /// </summary>
    /// <param name="player">owner��client��</param>
    private void MoveStage(bool player,bool input)
    {
        //owner��client��
        if (player)
        {
            //���ꂼ����͂��Ă��邩
            if (input)
            {
                //��񂵂�����Ȃ�
                if (first)
                {
                    //�V�[���ړ��J�n
                    photonView.RPC(nameof(RpcShareStart), RpcTarget.All);

                    //�t�F�[�h�J�n
                    if (PhotonNetwork.IsMasterClient)
                        photonView.RPC(nameof(RpcShareFadeStart), RpcTarget.All, true);
                    else
                        photonView.RPC(nameof(RpcShareFadeStart), RpcTarget.All, false);

                    first = false;
                }
            }
        }
    }
}
