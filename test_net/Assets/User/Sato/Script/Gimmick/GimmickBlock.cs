using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class GimmickBlock : CGimmick
{
    //ÇªÇÍÇºÇÍÇÃÉ{É^Éìì¸óÕèÛãµ
    [System.NonSerialized] public bool liftMode = false;

    private GameObject Player;

    private bool hitOwner = false;
    private bool hitClient = false;

    private Vector3 dis = Vector3.zero;

    private bool first = true;

    private void FixedUpdate()
    {
        if (hitOwner /*&& hitClient*/ && ManagerAccessor.Instance.dataManager.isOwnerInputKey_LM /*&& ManagerAccessor.Instance.dataManager.isClientInputKey_LM*/) 
        {
            if(first)
            {
                Vector3 input = gameObject.transform.position;
                input.y += 1;
                gameObject.transform.localPosition = input;

                dis = transform.position - Player.transform.position;
                
                first = false;
            }

            //Debug.Log("aaa");
            gameObject.transform.position = dis + Player.transform.position;


            liftMode = true;
        }
        else
        {
            if (!first)
            {
                Vector3 input = gameObject.transform.position;
                input.y -= 1;
                gameObject.transform.localPosition = input;

                dis = new Vector3(gameObject.transform.position.x - Player.transform.position.x, gameObject.transform.position.y - Player.transform.position.y, 0);

                first = true;
            }

            liftMode = false;
        }


    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player1")
        {
            hitOwner = true;

            if(PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                Player = collision.gameObject;
            }
        }

        if (collision.gameObject.name == "Player2")
        {
            hitClient = true;

            if (PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                Player = collision.gameObject;
            }
        }
    }


    private void OnCollisionExit2D(Collision2D collision)
    {
        //éùÇøè„Ç∞ÇƒÇ¢ÇÈÇ∆Ç´
        if (!liftMode)
        {
            if (collision.gameObject.name == "Player1")
            {
                hitOwner = false;
            }

            if (collision.gameObject.name == "Player2")
            {
                hitClient = false;
            }
        }
    }


}
