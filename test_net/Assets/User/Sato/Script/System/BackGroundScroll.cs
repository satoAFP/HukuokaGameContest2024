using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BackGroundScroll : MonoBehaviourPunCallbacks
{
    [SerializeField, Header("背景オブジェクト")] private GameObject BackGround;

    [SerializeField, Header("画像サイズ")] private Vector2 Size;

    private GameObject[] imgObj = new GameObject[9];
    private GameObject player = null;

    

    private Vector3 memPos = Vector3.zero;
    [SerializeField] private Vector2 mapPos = Vector2.zero;

    private bool first = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            player = ManagerAccessor.Instance.dataManager.player1;
        }
        else
        {
            player = ManagerAccessor.Instance.dataManager.player2;
        }

        if (first)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                memPos = player.transform.position;
                first = false;
            }
            else
            {
                memPos = player.transform.position;
                first = false;
            }

            for (int i = 0; i < 9; i++)
                imgObj[i] = Instantiate(BackGround);

            for (int i = 0; i < 9; i += 3) 
            {
                imgObj[i].transform.position = new Vector3(-9, Size.y / 2 - (Size.y * i / 3), 0);
                imgObj[i + 1].transform.position = new Vector3(9, Size.y / 2 - (Size.y * i / 3), 0);
                imgObj[i + 2].transform.position = new Vector3(27, Size.y / 2 - (Size.y * i / 3), 0);
            }

        }

        if (player.transform.position.x >= 0) 
        {
            mapPos.x = Mathf.Floor(player.transform.position.x / Size.x);
        }
        else
        {
            if (mapPos.x == 0)
                mapPos.x = -1;

            mapPos.x = Mathf.Ceil(player.transform.position.x / Size.x) - 1;
        }

        if (player.transform.position.y >= 0)
        {
            mapPos.y = Mathf.Floor(player.transform.position.y / Size.y);
        }
        else
        {
            if (mapPos.y == 0)
                mapPos.y = -1;

            mapPos.y = Mathf.Ceil(player.transform.position.y / Size.y) - 1;
        }

        for (int i = 0; i < 9; i += 3)
        {
            imgObj[i].transform.position = new Vector3(-9 + (mapPos.x * Size.x), Size.y - (Size.y * i / 3) + (mapPos.y * Size.y), 0);
            imgObj[i + 1].transform.position = new Vector3(9 + (mapPos.x * Size.x), Size.y - (Size.y * i / 3) + (mapPos.y * Size.y), 0);
            imgObj[i + 2].transform.position = new Vector3(27 + (mapPos.x * Size.x), Size.y - (Size.y * i / 3) + (mapPos.y * Size.y), 0);
        }

    }
}
