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

    private Vector3 memPos = new Vector3(0, 0, 0);

    private void Start()
    {
        memPos = transform.position;
        p1 = transform.position;
        p2 = p1;
        elapsedTime = 0f;
    }

    private void Update()
    {
        if (!photonView.IsMine)
        {
            if (!(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S)))
            {
                if (first)
                {
                    memPos = transform.position;
                    first = false;
                }
            }
            else
            {
                first = true;
            }

            // ���v���C���[�̃l�b�g���[�N�I�u�W�F�N�g�́A��ԏ������s��
            elapsedTime += Time.deltaTime;
            transform.position = Vector3.LerpUnclamped(p1, p2, elapsedTime / InterpolationPeriod);
            Debug.Log("bbb");
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
            Debug.Log("aaa");
        }
    }

    [PunRPC]
    private void RpcMoving(bool onkey)
    {
        onKey = onkey;
    }
}
