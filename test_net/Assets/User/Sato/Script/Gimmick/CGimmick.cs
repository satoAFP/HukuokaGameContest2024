using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class CGimmick : MonoBehaviourPunCallbacks

{
    protected const int PLAYER1 = 1;
    protected const int PLAYER2 = 2;

    protected bool p1_Button = false;
    protected bool p2_Button = false;

    //キーナンバー
    protected enum KEY_NUMBER
    {
        A, D, W, S, B,
        LM,
        C_A, C_B, C_X, C_Y,
        C_L_RIGHT, C_L_LEFT, C_L_UP, C_L_DOWN,
        C_R_RIGHT, C_R_LEFT, C_R_UP, C_R_DOWN,
        C_D_RIGHT, C_D_LEFT, C_D_UP, C_D_DOWN,
        C_R1, C_R2, C_L1, C_L2
    }

    

}
