using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class UIUnderbar : MonoBehaviourPunCallbacks
{
    [SerializeField, Header("箱プレイヤーアイコン")]
    private GameObject P1Icon;

    [SerializeField, Header("鍵プレイヤーアイコン")]
    private GameObject P2Icon;

    private Vector2 UnderbarPos;//自身が操作しているキャラアイコンの下に表示するバーの座標

    // Start is called before the first frame update
    void Start()
    {
        UnderbarPos.y = transform.position.y;

        //操作しているキャラによってキャラアイコンの下に表示するバーの位置を変える
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            //Debug.Log("ほそ");
            UnderbarPos.x = P1Icon.transform.position.x;
        }
        else
        {
            ///Debug.Log("ほそ1");
            UnderbarPos.x = P2Icon.transform.position.x;
        }

        transform.position = UnderbarPos;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
