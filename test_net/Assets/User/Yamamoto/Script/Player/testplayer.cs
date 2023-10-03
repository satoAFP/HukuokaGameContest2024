using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class testplayer : MonoBehaviourPunCallbacks
{

   [SerializeField,Header("テスト用プレイヤー速度")] private float speed;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.name = "Player" + photonView.OwnerActorNr;
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            Vector2 position = transform.position;

            if (Input.GetKey(KeyCode.A))
            {
                position.x -= speed;
            }
            if (Input.GetKey(KeyCode.D))
            {
                position.x += speed;
            }
            if (Input.GetKey(KeyCode.W))
            {
                position.y += speed;
            }
            if (Input.GetKey(KeyCode.S))
            {
                position.y -= speed;
            }


            transform.position = position;
        }
    }
}
