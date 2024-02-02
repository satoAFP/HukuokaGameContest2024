using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RotateTransformView : MonoBehaviourPunCallbacks, IPunObservable
{
    private const float InterpolationPeriod = 0.1f; // 補間にかける時間

    private Vector3 p1;
    private Vector3 p2;
    private float elapsedTime;

    private void Start()
    {
        p1 = transform.eulerAngles;
        p2 = p1;
        elapsedTime = 0f;
    }

    private void Update()
    {
        if (!photonView.IsMine)
        {
            // 他プレイヤーのネットワークオブジェクトは、補間処理を行う
            elapsedTime += Time.deltaTime;
            transform.eulerAngles = Vector3.Lerp(p1, p2, elapsedTime / InterpolationPeriod);
        }
    }

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.eulerAngles);
        }
        else
        {
            // 受信時の座標を、補間の開始座標にする
            p1 = transform.eulerAngles;
            // 受信した座標を、補間の終了座標にする
            p2 = (Vector3)stream.ReceiveNext();
            // 経過時間をリセットする
            elapsedTime = 0f;
        }
    }
}
