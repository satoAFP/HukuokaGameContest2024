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

            // 他プレイヤーのネットワークオブジェクトは、補間処理を行う
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
            // 受信時の座標を、補間の開始座標にする
            p1 = transform.position;
            // 受信した座標を、補間の終了座標にする
            p2 = (Vector3)stream.ReceiveNext();
            // 経過時間をリセットする
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
