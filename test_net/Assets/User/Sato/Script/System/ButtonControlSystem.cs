using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ButtonControlSystem : MonoBehaviourPunCallbacks
{
    [SerializeField, Header("ボタン座標オブジェクト")] private GameObject[] Buttons;

    [SerializeField, Header("アンダーバー")] private GameObject underber;

    [SerializeField, Header("縦＝true 横＝false")] private bool inputDirection;

    private DataManager dataManager;        //データマネージャー取得用

    private bool buttonNum = true;          //ボタンの数

    private bool isSelect = false;          //ボタンが選択されたとき

    //連続で入らないよう
    private bool first = true;
    private bool firstSceneMove = true;


    // Update is called once per frame
    void Update()
    {
        if (!isSelect)
        {
            dataManager = ManagerAccessor.Instance.dataManager;

            if (PhotonNetwork.IsMasterClient)
            {
                if (inputDirection)
                {
                    //上下を入力するとカーソルが上下する
                    if (dataManager.isOwnerInputKey_C_L_UP || dataManager.isOwnerInputKey_C_L_DOWN)
                    {
                        if (first)
                        {
                            buttonNum = !buttonNum;
                            first = false;
                        }
                    }
                    else
                        first = true;
                }
                else
                {
                    //左右を入力するとカーソルが左右する
                    if (dataManager.isOwnerInputKey_C_L_RIGHT || dataManager.isOwnerInputKey_C_L_LEFT)
                    {
                        if (first)
                        {
                            buttonNum = !buttonNum;
                            first = false;
                        }
                    }
                    else
                        first = true;
                }

                //ボタンにあった座標とサイズに変更
                if (buttonNum)
                {
                    //アンダーバー座標とサイズ変更
                    RectTransform buttonTra = Buttons[0].GetComponent<RectTransform>();
                    underber.GetComponent<RectTransform>().position = new Vector2(buttonTra.position.x, buttonTra.position.y - buttonTra.sizeDelta.y / 2);
                    underber.GetComponent<RectTransform>().sizeDelta = new Vector2(buttonTra.sizeDelta.x + 100, underber.GetComponent<RectTransform>().sizeDelta.y);

                    underber.transform.GetChild(0).GetComponent<RectTransform>().position = new Vector2(buttonTra.position.x - buttonTra.sizeDelta.x / 2, underber.GetComponent<RectTransform>().position.y + 40);
                    underber.transform.GetChild(1).GetComponent<RectTransform>().position = new Vector2(buttonTra.position.x + buttonTra.sizeDelta.x / 2, underber.GetComponent<RectTransform>().position.y + 40);
                }
                else
                {
                    //アンダーバー座標とサイズ変更
                    RectTransform buttonTra = Buttons[1].GetComponent<RectTransform>();
                    underber.GetComponent<RectTransform>().position = new Vector2(buttonTra.position.x, buttonTra.position.y - buttonTra.sizeDelta.y / 2);
                    underber.GetComponent<RectTransform>().sizeDelta = new Vector2(buttonTra.sizeDelta.x + 100, underber.GetComponent<RectTransform>().sizeDelta.y);

                    underber.transform.GetChild(0).GetComponent<RectTransform>().position = new Vector2(buttonTra.position.x - buttonTra.sizeDelta.x / 2, underber.GetComponent<RectTransform>().position.y + 40);
                    underber.transform.GetChild(1).GetComponent<RectTransform>().position = new Vector2(buttonTra.position.x + buttonTra.sizeDelta.x / 2, underber.GetComponent<RectTransform>().position.y + 40);
                }



                //シーン移動処理
                if (dataManager.isOwnerInputKey_CB)
                {
                    if (firstSceneMove)
                    {
                        if (buttonNum)
                        {
                            ManagerAccessor.Instance.sceneMoveManager.SceneMoveRetry();
                            isSelect = true;
                            firstSceneMove = false;
                        }
                        else
                        {
                            ManagerAccessor.Instance.sceneMoveManager.SceneMoveName("StageSelect");
                            isSelect = true;
                            firstSceneMove = false;
                        }
                    }
                }
            }
        }
    }
}
