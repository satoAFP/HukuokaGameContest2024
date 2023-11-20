using Photon.Pun;
using UnityEngine;

public class AvatarTransformView : MonoBehaviourPunCallbacks, IPunObservable
{
    //�v���C���[�������Ă��邩���
    [System.NonSerialized] public bool isPlayerMove = false;

    private const float InterpolationPeriod = 0.1f; // ��Ԃɂ����鎞��

    private Vector3 p1;         //���g�̍��W�L���p
    private Vector3 p2;         //��M�������W�L���p
    private float elapsedTime;

    private bool onKey = false; //�ړ����Ă��邩�ǂ���
    private bool first = true;  //�A���ŏ������ʂ�Ȃ�����
    private bool first1 = true;

    private Vector3 memPos = Vector3.zero;

    private Rigidbody2D rb;     //���W�b�g�{�f�B

    //���������グ�Ĉړ�����Ƃ��A�ŏ��Ƀv���C���[���m�̍������߂�
    private bool distanceFirst = true;
    private Vector3 dis = Vector3.zero;

    private void Start()
    {
        //������
        p1 = transform.position;
        p2 = p1;
        elapsedTime = 0f;

        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        DataManager dataManager = ManagerAccessor.Instance.dataManager;

        if (gameObject.name != "CopyKey")
        {
            if (GetComponent<PlayerController>().islift)
            {
                if (first1)
                {
                    memPos = transform.position;
                    first1 = false;
                }

                if (PhotonNetwork.IsMasterClient)
                {
                    //���W����C��
                    if (dataManager.isOwnerInputKey_C_L_RIGHT || dataManager.isOwnerInputKey_C_L_LEFT)
                    {
                        memPos = transform.position;
                    }
                    else
                    {
                        transform.position = memPos;
                    }

                    //���������グ�Ĉړ�����Ƃ��A�ŏ��Ƀv���C���[���m�̍������߂�
                    if (distanceFirst)
                    {
                        //1P��2P�̍��W�̍����L��
                        if (!ManagerAccessor.Instance.dataManager.isAppearCopyKey)
                            dis = dataManager.player1.transform.position - dataManager.player2.transform.position;
                        else
                            dis = dataManager.copyKey.transform.position - dataManager.player2.transform.position;
                        distanceFirst = false;
                    }

                    //2P��1P�ɒǏ]����悤�ɂ���
                    if (!ManagerAccessor.Instance.dataManager.isAppearCopyKey)
                        ManagerAccessor.Instance.dataManager.GetPlyerObj("Player2").transform.position = dataManager.player1.transform.position - dis;
                    else
                        ManagerAccessor.Instance.dataManager.GetPlyerObj("Player2").transform.position = dataManager.copyKey.transform.position - dis;
                }
                else
                {
                    //���W����C��
                    if (dataManager.isOwnerInputKey_C_L_RIGHT || dataManager.isOwnerInputKey_C_L_LEFT)
                    {
                        memPos = transform.position;
                    }
                    else
                    {
                        transform.position = memPos;
                    }

                    //���������グ�Ĉړ�����Ƃ��A�ŏ��Ƀv���C���[���m�̍������߂�
                    if (distanceFirst)
                    {
                        //1P��2P�̍��W�̍����L��
                        if (!ManagerAccessor.Instance.dataManager.isAppearCopyKey)
                            dis = dataManager.player1.transform.position - dataManager.player2.transform.position;
                        else
                            dis = dataManager.copyKey.transform.position - dataManager.player2.transform.position;
                        distanceFirst = false;
                    }

                    //2P��1P�ɒǏ]����悤�ɂ���
                    if (!ManagerAccessor.Instance.dataManager.isAppearCopyKey)
                        ManagerAccessor.Instance.dataManager.GetPlyerObj("Player2").transform.position = dataManager.player1.transform.position - dis;
                    else
                        ManagerAccessor.Instance.dataManager.GetPlyerObj("Player2").transform.position = dataManager.copyKey.transform.position - dis;
                }
            }
            else
            {
                first1 = true;
                distanceFirst = true;
            }
        }
        else
        {
            if (GetComponent<CopyKey>().islift)
            {
                if (first1)
                {
                    memPos = transform.position;
                    first1 = false;
                }

                if (dataManager.isOwnerInputKey_C_L_RIGHT || dataManager.isOwnerInputKey_C_L_LEFT)
                {
                    memPos = transform.position;
                }
                else
                {
                    transform.position = memPos;
                }

                //���������グ�Ĉړ�����Ƃ��A�ŏ��Ƀv���C���[���m�̍������߂�
                if (distanceFirst)
                {
                    //1P��2P�̍��W�̍����L��
                    if (!ManagerAccessor.Instance.dataManager.isAppearCopyKey)
                        dis = dataManager.player1.transform.position - dataManager.player2.transform.position;
                    else
                        dis = dataManager.copyKey.transform.position - dataManager.player2.transform.position;
                    distanceFirst = false;
                }

                //2P��1P�ɒǏ]����悤�ɂ���
                if (!ManagerAccessor.Instance.dataManager.isAppearCopyKey)
                    ManagerAccessor.Instance.dataManager.GetPlyerObj("Player2").transform.position = dataManager.player1.transform.position - dis;
                else
                    ManagerAccessor.Instance.dataManager.GetPlyerObj("Player2").transform.position = dataManager.copyKey.transform.position - dis;
            }
            else
            {
                first1 = true; 
                distanceFirst = true;
            }
        }

        //�f�[�^���M�T�C�h
        if(photonView.IsMine)
        {
            //�ړ���
            if (rb.velocity.magnitude > 0.1f) 
            {
                if (first)
                {
                    //�ړ��J�n����1�t���[���ڂ����f�[�^���M
                    photonView.RPC(nameof(RpcIsMove), RpcTarget.Others, true);
                    first = false;

                    //�v���C���[�������Ă�����
                    isPlayerMove = true;
                }
            }
            //��~��
            else
            {
                if (!first)
                {
                    //�ړ��I������1�t���[���ڂ����f�[�^���M
                    photonView.RPC(nameof(RpcIsMove), RpcTarget.Others, false);
                    first = true;

                    //�v���C���[���~�܂��Ă�����
                    isPlayerMove = false;
                }
            }
        }
        //�f�[�^��M�T�C�h
        else
        {
            //���ԉ��Z
            elapsedTime += Time.deltaTime;

            //�^��ł���Ƃ��̍��W������Ȃ������߂ɁA1P��ʂ̎���2P�͉^��ł���Ƃ��ʐM���Ȃ��悤�ɂ���
            if ((!ManagerAccessor.Instance.dataManager.GetPlyerObj("Player1").GetComponent<PlayerController>().islift && PhotonNetwork.IsMasterClient) ||
                !PhotonNetwork.IsMasterClient) 
            {
                // ���v���C���[�̃l�b�g���[�N�I�u�W�F�N�g�́A��ԏ������s��
                if (onKey)
                {
                    //�ړ����͕�ԏ���
                    transform.position = Vector3.LerpUnclamped(p1, p2, elapsedTime / InterpolationPeriod);
                }
                else
                {
                    //��~���͎�M�������W����
                    transform.position = p2;
                }
            }
        }
    }

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            //���g�̍��W���M
            stream.SendNext(transform.position);

            if (gameObject.name == "LiftBlock")
                ManagerAccessor.Instance.dataManager.chat.text = "���M��";
        }
        else
        {
            // ��M���̍��W���A��Ԃ̊J�n���W�ɂ���
            p1 = transform.position;
            // ��M�������W���A��Ԃ̏I�����W�ɂ���
            p2 = (Vector3)stream.ReceiveNext();
            // �o�ߎ��Ԃ����Z�b�g����
            elapsedTime = 0f;

            if (gameObject.name == "LiftBlock")
                ManagerAccessor.Instance.dataManager.chat.text = "��M��";
        }
    }

    //�ړ����Ă��邩�ǂ����̏�񑗐M
    [PunRPC]
    private void RpcIsMove(bool onkey)
    {
        onKey = onkey;
    }
}
