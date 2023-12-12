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

    



    // Start is called before the first frame update
    void Start()
    {
        copykey = transform.parent.GetComponent<CopyKey>();//CopyKeyスクリプトを取得

        GetComponent<SpriteRenderer>().sprite = CKeyImage;//画像の初期化
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(copykey.copykey_death)
        {
            GetComponent<SpriteRenderer>().sprite = CKeyDeathImage;//コピーキーの死亡時画像
        }
        else
        {
            //コピーキーの移動した方向に応じてプレイヤーの向きを変える
            if (ManagerAccessor.Instance.dataManager.copyKey.GetComponent<CopyKey>().imageleft)
            {
                transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
            }
            else
            {
                transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            }

            if (ManagerAccessor.Instance.dataManager.copyKey.GetComponent<CopyKey>().changeliftimage)
            {
                GetComponent<SpriteRenderer>().sprite = CKeyLiftImage;//持ち上げ時の画像に変える
            }
            else if(ManagerAccessor.Instance.dataManager.copyKey.GetComponent<CopyKey>().standardCopyKeyImage)
            {
                GetComponent<SpriteRenderer>().sprite = CKeyImage;//画像を元に戻す
            }



        }
    }
}
