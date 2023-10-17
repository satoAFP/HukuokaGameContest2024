using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GimmickUnlockButtonManagement : MonoBehaviour
{
    [SerializeField, Header("ギミック用ボタン")]
    private List<GameObject> gimmickButton;

    [SerializeField, Header("扉")]
    private GameObject door;

    [SerializeField, Header("入力する数")]
    private int inputKey;

    public List<int> answer = new List<int>();

    private enum Key
    {
        A,B,X,Y
    }

    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            for (int i = 0; i < inputKey; i++) 
            {
                answer.Add(Random.Range(0, 4));
            }

            for(int i=0;i<gimmickButton.Count;i++)
            {
                gimmickButton[i].GetComponent<GimmickUnlockButton>().answer = answer;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        ////ボタンが押されているオブジェクトの数カウント用
        //int count = 0;

        ////ボタンの数だけ回す
        //for (int i = 0; i < gimmickButton.Count; i++)
        //{
        //    if (gimmickButton[i].GetComponent<GimmickUnlockButton>().isButton == true)
        //    {
        //        count++;
        //    }
        //}

        ////同時押しが成功すると、扉が開く
        //if (gimmickButton.Count == count)
        //{
        //    door.SetActive(false);
        //}


    }
}
