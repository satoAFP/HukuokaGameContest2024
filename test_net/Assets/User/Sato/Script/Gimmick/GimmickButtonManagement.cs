using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimmickButtonManagement : CGimmick
{
    [SerializeField, Header("ギミック用ボタン")]
    private List<GameObject> gimmickButton;

    [SerializeField, Header("扉")]
    private GameObject door;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //ボタンが押されているオブジェクトの数カウント用
        int count = 0;

        //ボタンの数だけ回す
        for (int i = 0; i < gimmickButton.Count; i++)
        {
            if (gimmickButton[i].GetComponent<GimmickButton>().isButton == true)
            {
                count++;
            }
        }

        Debug.Log(count+":"+ gimmickButton[0].GetComponent<GimmickButton>().isButton + ":" + gimmickButton[1].GetComponent<GimmickButton>().isButton);

        //同時押しが成功すると、扉が開く
        if (gimmickButton.Count == count) 
        {
            door.SetActive(false);
        }

    }
}
