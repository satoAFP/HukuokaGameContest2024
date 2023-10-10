using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class GimmickBlock : CGimmick
{
    //ÇªÇÍÇºÇÍÇÃÉ{É^Éìì¸óÕèÛãµ
    [System.NonSerialized] public bool liftMode = false;

    private bool hitOwner = false;
    private bool hitClient = false;

    private bool first = true;

    private void FixedUpdate()
    {
        if (hitOwner && hitClient && ManagerAccessor.Instance.dataManager.isOwnerInputKey_LM && ManagerAccessor.Instance.dataManager.isClientInputKey_LM) 
        {
            if(first)
            {
                Vector3 input = gameObject.transform.localPosition;
                input.y += 1;
                gameObject.transform.localPosition = input;
            }


        }
        else
        {
            
        }


    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player1")
        {
            hitOwner = true;
        }

        if (collision.gameObject.name == "Player2")
        {
            hitClient = true;
        }
    }


    private void OnCollisionExit2D(Collision2D collision)
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
