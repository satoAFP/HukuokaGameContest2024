using Photon.Pun;
using UnityEngine;

public class AvatarTransformView : MonoBehaviourPunCallbacks, IPunObservable
{
    private const float InterpolationPeriod = 0.1f; // 補間にかける時間

    private Vector3 p1;         //自身の座標記憶用
    private Vector3 p2;         //受信した座標記憶用
    private float elapsedTime;

    private bool onKey = false; //移動しているかどうか
    private bool first = true;  //連続で処理が通らないため


    private Rigidbody2D rb;     //リジットボディ

    private void Start()
    {
        //初期化
        p1 = transform.position;
        p2 = p1;
        elapsedTime = 0f;

        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        //データ送信サイド
        if(photonView.IsMine)
        {
            //移動時
            if (rb.velocity.magnitude > 0.1f) 
            {
                if (first)
                {
                    //移動開始時の1フレーム目だけデータ送信
                    photonView.RPC(nameof(RpcIsMove), RpcTarget.Others, true);
                    first = false;
                }
            }
            //停止時
            else
            {
                if (!first)
                {
                    //移動終了時の1フレーム目だけデータ送信
                    photonView.RPC(nameof(RpcIsMove), RpcTarget.Others, false);
                    first = true;
                }
            }
        }
        //データ受信サイド
        else
        {
            //時間加算
            elapsedTime += Time.deltaTime;

            // 他プレイヤーのネットワークオブジェクトは、補間処理を行う
            if (onKey)
            {
                //移動時は補間処理
                transform.position = Vector3.LerpUnclamped(p1, p2, elapsedTime / InterpolationPeriod);
            }
            else
            {
                //停止時は受信した座標を代入
                transform.position = p2;
            }
        }
    }

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            //自身の座標送信
            stream.SendNext(transform.position);

            if (gameObject.name == "LiftBlock")
                ManagerAccessor.Instance.dataManager.chat.text = "送信中";
        }
        else
        {
            // 受信時の座標を、補間の開始座標にする
            p1 = transform.position;
            // 受信した座標を、補間の終了座標にする
            p2 = (Vector3)stream.ReceiveNext();
            // 経過時間をリセットする
            elapsedTime = 0f;

            if (gameObject.name == "LiftBlock")
                ManagerAccessor.Instance.dataManager.chat.text = "受信中";
        }
    }

    //移動しているかどうかの情報送信
    [PunRPC]
    private void RpcIsMove(bool onkey)
    {
        onKey = onkey;
    }
}
