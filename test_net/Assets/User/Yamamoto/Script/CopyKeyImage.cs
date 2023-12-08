using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyKeyImage : MonoBehaviour
{
    [SerializeField, Header("通常時のコピーキー")]
    private Sprite CKeyImage;

    [SerializeField, Header("運搬時のコピーキー")]
    private Sprite CKeyLiftImage;

   [SerializeField, Header("死亡時のコピーキー")]
    private Sprite CKeyDeathImage;

    CopyKey copykey;

    private bool firstLR = true;//左右移動一度だけ処理を行う



    // Start is called before the first frame update
    void Start()
    {
        copykey = transform.parent.GetComponent<CopyKey>();//CopyKeyスクリプトを取得

        GetComponent<SpriteRenderer>().sprite = CKeyImage;//画像の初期化
    }

    // Update is called once per frame
    void Update()
    {
        if(copykey.copykey_death)
        {
            GetComponent<SpriteRenderer>().sprite = CKeyDeathImage;//コピーキーの死亡時画像
        }
    }
}
