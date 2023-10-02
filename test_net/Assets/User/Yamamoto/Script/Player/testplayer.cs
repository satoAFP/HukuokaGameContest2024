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
        
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            Vector2 position = transform.position;

            if (Input.GetKey("left"))
            {
                position.x -= speed;
            }
            else if (Input.GetKey("right"))
            {
                position.x += speed;
            }
            else if (Input.GetKey("up"))
            {
                position.y += speed;
            }
            else if (Input.GetKey("down"))
            {
                position.y -= speed;
            }

            transform.position = position;
        }
    }
}
