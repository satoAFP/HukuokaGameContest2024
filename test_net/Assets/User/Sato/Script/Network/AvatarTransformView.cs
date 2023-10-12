using Photon.Pun;
using UnityEngine;

public class AvatarTransformView : MonoBehaviourPunCallbacks, IPunObservable
{
    private const float InterpolationPeriod = 0.1f; // ��Ԃɂ����鎞��

    private Vector3 p1;
    private Vector3 p2;
    private float elapsedTime;

    private bool onKey = false;
    private bool first = true;


    private Rigidbody2D rb;

    private void Start()
    {
        p1 = transform.position;
        p2 = p1;
        elapsedTime = 0f;

        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if(photonView.IsMine)
        {
            if (rb.velocity.magnitude > 0.1f) 
            {
                if (first)
                {
                    photonView.RPC(nameof(RpcIsMove), RpcTarget.Others, true);
                    first = false;
                }
            }
            else
            {
                if (!first)
                {
                    photonView.RPC(nameof(RpcIsMove), RpcTarget.Others, false);
                    first = true;
                }
            }
        }
        else
        {
            elapsedTime += Time.deltaTime;
            // ���v���C���[�̃l�b�g���[�N�I�u�W�F�N�g�́A��ԏ������s��
            if (onKey)
            {
                transform.position = Vector3.LerpUnclamped(p1, p2, elapsedTime / InterpolationPeriod);
            }
            else
            {
                transform.position = p2;
            }
        }
    }

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
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
        }
    }

    [PunRPC]
    private void RpcIsMove(bool onkey)
    {
        onKey = onkey;
    }
}
