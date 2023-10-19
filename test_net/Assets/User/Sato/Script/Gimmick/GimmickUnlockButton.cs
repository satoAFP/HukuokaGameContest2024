using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class GimmickUnlockButton : CGimmick
{
    private enum Key
    {
        A, B, X, Y
    }

    //‚»‚ê‚¼‚ê‚Ìƒ{ƒ^ƒ““ü—Íó‹µ
    [System.NonSerialized] public bool isButton = false;

    //“š‚¦
    [System.NonSerialized] public List<int> answer = new List<int>();

    public List<bool> ClearSituation;


    private void Update()
    {
        if (ManagerAccessor.Instance.dataManager.isUnlockButtonStart)
        {
            for (int i = 0; i < answer.Count; i++)
            {
                if (ClearSituation[i])
                {
                    continue;
                }
                else
                {
                    if (PhotonNetwork.LocalPlayer.IsMasterClient)
                    {
                        switch (answer[i])
                        {
                            case (int)Key.A:
                                if (ManagerAccessor.Instance.dataManager.isOwnerInputKey_CA)
                                    ClearSituation[i] = true;
                                break;
                            case (int)Key.B:
                                if (ManagerAccessor.Instance.dataManager.isOwnerInputKey_CB)
                                    ClearSituation[i] = true;
                                break;
                            case (int)Key.X:
                                if (ManagerAccessor.Instance.dataManager.isOwnerInputKey_CX)
                                    ClearSituation[i] = true;
                                break;
                            case (int)Key.Y:
                                if (ManagerAccessor.Instance.dataManager.isOwnerInputKey_CY)
                                    ClearSituation[i] = true;
                                break;
                        }
                    }
                }
            }
        }


    }


    private void OnCollisionStay2D(Collision2D collision)
    {
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            if (collision.gameObject.name == "Player1")
            {
                ManagerAccessor.Instance.dataManager.isUnlockButtonStart = true;
            }
        }
        else
        {
            if (collision.gameObject.name == "Player2")
            {
                ManagerAccessor.Instance.dataManager.isUnlockButtonStart = true;
            }
        }
    }
}
