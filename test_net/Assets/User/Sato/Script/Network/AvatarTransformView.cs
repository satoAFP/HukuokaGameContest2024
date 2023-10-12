using Photon.Pun;
using UnityEngine;

public class AvatarTransformView : MonoBehaviourPunCallbacks, IPunObservable
{
    private const float InterpolationPeriod = 0.1f; // 補間にかける時間

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
            // 他プレイヤーのネットワークオブジェクトは、補間処理を行う
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
            // 受信時の座標を、補間の開始座標にする
            p1 = transform.position;
            // 受信した座標を、補間の終了座標にする
            p2 = (Vector3)stream.ReceiveNext();
            // 経過時間をリセットする
            elapsedTime = 0f;
        }
    }

    [PunRPC]
    private void RpcIsMove(bool onkey)
    {
        onKey = onkey;
    }
}
