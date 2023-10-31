using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteManager : MonoBehaviour
{
    [Header("コントローラー入力画像")]
    //ボタン
    public Sprite ArrowRight;
    public Sprite ArrowLeft;
    public Sprite ArrowUp;
    public Sprite ArrowDown;
    //十字キー
    public Sprite CrossRight;
    public Sprite CrossLeft;
    public Sprite CrossUp;
    public Sprite CrossDown;
    //上ボタン
    public Sprite R1;
    public Sprite R2;
    public Sprite L1;
    public Sprite L2;
    //スティック
    public Sprite StickR;



    // Start is called before the first frame update
    void Start()
    {
        //マネージャーアクセッサに登録
        ManagerAccessor.Instance.spriteManager = this;
    }
}
