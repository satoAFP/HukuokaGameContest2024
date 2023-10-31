using Photon.Pun;
using UnityEngine;

public class AvatarOnlyTransformView : MonoBehaviourPunCallbacks, IPunObservable
{
    //�v���C���[�������Ă��邩���
    [System.NonSerialized] public bool isPlayerMove = false;

    private const float InterpolationPeriod = 0.1f; // ��Ԃɂ����鎞��

    private Vector3 p1;         //���g�̍��W�L���p
    private Vector3 p2;         //��M�������W�L���p
    private float elapsedTime;

    private Vector3 lastPosition; // ���O�̈ʒu

    private bool onKey = false; //�ړ����Ă��邩�ǂ���
    private bool first = true;  //�A���ŏ������ʂ�Ȃ�����


    private Rigidbody2D rb;     //���W�b�g�{�f�B

    private void Start()
    {
        //������
        p1 = transform.position;
        p2 = p1;
        elapsedTime = 0f;
        lastPosition = transform.position; // �ŏ��͌��݂̈ʒu���L�^

        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Vector3 currentPosition = transform.position;

        //�f�[�^���M�T�C�h
        if (photonView.IsMine)
        {
            //�ړ���
            if (isPlayerMove)
            {
                if (first)
                {
                    //�ړ��J�n����1�t���[���ڂ����f�[�^���M
                    photonView.RPC(nameof(RpcIsMove), RpcTarget.Others, true);
                    first = false;
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
                }
            }
        }
        //�f�[�^��M�T�C�h
        else
        {
            //���ԉ��Z
            elapsedTime += Time.deltaTime;

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

        // ���݂̈ʒu�𒼑O�̈ʒu�Ƃ��ĕۑ�
        lastPosition = currentPosition;
    }

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            //���g�̍��W���M
            stream.SendNext(transform.position);

            
        }
        else
        {
            // ��M���̍��W���A��Ԃ̊J�n���W�ɂ���
            p1 = transform.position;
            // ��M�������W���A��Ԃ̏I�����W�ɂ���
            p2 = (Vector3)stream.ReceiveNext();
            // �o�ߎ��Ԃ����Z�b�g����
            elapsedTime = 0f;

            
            //ManagerAccessor.Instance.dataManager.chat.text = "��M��";
        }
    }

    //�ړ����Ă��邩�ǂ����̏�񑗐M
    [PunRPC]
    private void RpcIsMove(bool onkey)
    {
        onKey = onkey;
    }
}