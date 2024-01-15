using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class ResultSystem : MonoBehaviourPunCallbacks
{
    [SerializeField, Header("クリア時に出すオブジェクト")] private GameObject[] clearObjs;

    [SerializeField, Header("オーナー失敗回数用テキスト")] private Text OwnerMissText;
    [SerializeField, Header("クライアント失敗回数用テキスト")] private Text ClientMissText;

    [SerializeField, Header("オーナーの評価画像")] private GameObject[] OwnerEvaluationObjs;
    [SerializeField, Header("クライアントの評価画像")] private GameObject[] ClientEvaluationObjs;

    [SerializeField, Header("評価描画用テキスト")] private Text EvaluationText;

    [SerializeField, Header("クリア時のボタン")] private GameObject ClearButton;
    [SerializeField, Header("ゲームオーバー時のボタン")] private GameObject GameOverButton;

    [SerializeField, Header("P1の死亡画像")] private GameObject P1DethImg;
    [SerializeField, Header("P2の死亡画像")] private GameObject P2DethImg;

    [SerializeField, Header("最終ステージの場合オンにしてください")] private bool isLast;

    [SerializeField, Header("出す間隔")] private int intervalFrame;

    [SerializeField, Header("noTapArea")] private GameObject noTapArea;

    [SerializeField, Header("クリア時のBGM")] private AudioClip ClearBGM;
    [SerializeField, Header("ゲームオーバー時のBGM")] private AudioClip LoseBGM;

    private int count = 0;      //フレームを数える
    private int objCount = 0;   //オブジェクトを数える

    private bool isRetry = false;       //リトライ選択したとき
    private bool isStageSelect = false; //ステージセレクト選択したとき

    private int ownerMemCount = 0;
    private int clientMemCount = 0;

    private bool first = true;

    private FadeAnimation fadeanimation;//フェードアニメーションスクリプトを代入する変数
   
    //クリア処理に一回しか入らない処理
    private bool clearFirst;

    void Start()
    {
        fadeanimation = GameObject.Find("Fade").GetComponent<FadeAnimation>();//フェードアニメーションスクリプト取得

        //出すものを変える
        if (PhotonNetwork.IsMasterClient)
        {
            ClearButton.transform.GetChild(0).gameObject.SetActive(true);
            GameOverButton.transform.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            ClearButton.transform.GetChild(1).gameObject.SetActive(true);
            GameOverButton.transform.GetChild(1).gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //2Pにミスデータ共有
        if (PhotonNetwork.IsMasterClient)
        {
            if (ownerMemCount != ManagerAccessor.Instance.dataManager.ownerMissCount)
            {
                photonView.RPC(nameof(RpcShareOwnerMissCount), RpcTarget.All, ManagerAccessor.Instance.dataManager.ownerMissCount);
                ownerMemCount = ManagerAccessor.Instance.dataManager.ownerMissCount;
            }
            if (clientMemCount != ManagerAccessor.Instance.dataManager.clientMissCount)
            {
                photonView.RPC(nameof(RpcShareClientMissCount), RpcTarget.All, ManagerAccessor.Instance.dataManager.clientMissCount);
                clientMemCount = ManagerAccessor.Instance.dataManager.clientMissCount;
            }
        }

        //失敗回数描画
        OwnerMissText.text = "操作ミス：" + ManagerAccessor.Instance.dataManager.ownerMissCount.ToString() + "回";
        ClientMissText.text = "操作ミス：" + ManagerAccessor.Instance.dataManager.clientMissCount.ToString() + "回";

        //評価画像表示
        //それぞれの評価取得
        int owner = ResultScoreChange(ManagerAccessor.Instance.dataManager.ownerMissCount);
        int client = ResultScoreChange(ManagerAccessor.Instance.dataManager.clientMissCount);


        if (ManagerAccessor.Instance.dataManager.isClear)
        {
            for (int i = 0; i < owner; i++)
                OwnerEvaluationObjs[i].SetActive(true);

            for (int i = 0; i < client; i++)
                ClientEvaluationObjs[i].SetActive(true);
        }

        //評価描画
        EvaluationText.text = "二人は「" + ResultScoreCheck() + "」";


        //クリア画面
        if (ManagerAccessor.Instance.dataManager.isClear)
        {
            if (first)
            {
                //クリアパネル表示
                gameObject.transform.GetChild(0).gameObject.SetActive(true);
                //クリア情報セーブ
                ManagerAccessor.Instance.saveDataManager.ClearDataSave(ManagerAccessor.Instance.sceneMoveManager.GetSceneName());

                //BGM変更
                ManagerAccessor.Instance.dataManager.BGM.GetComponent<AudioSource>().clip = ClearBGM;

                //再生
                ManagerAccessor.Instance.dataManager.BGM.GetComponent<AudioSource>().Play();

                first = false;
            }

            //フレームカウント
            count++;
        }

        //ゲームオーバー画面
        if (!ManagerAccessor.Instance.dataManager.isClear
            &&ManagerAccessor.Instance.dataManager.isDeth)
        {
            if(fadeanimation.fadeoutfinish)
            {
                if (first)
                {
                    gameObject.transform.GetChild(1).gameObject.SetActive(true);

                    //BGM変更
                    ManagerAccessor.Instance.dataManager.BGM.GetComponent<AudioSource>().clip = LoseBGM;

                    //再生
                    ManagerAccessor.Instance.dataManager.BGM.GetComponent<AudioSource>().Play();

                    //死亡時の画像変更
                    if (ManagerAccessor.Instance.dataManager.DeathPlayerName == "Player1")
                        P1DethImg.SetActive(true);
                    if (ManagerAccessor.Instance.dataManager.DeathPlayerName == "Player2")
                        P2DethImg.SetActive(true);

                    first = false;
                }
            }

        }

        //一定間隔で画像を出す
        if (count == intervalFrame && objCount < clearObjs.Length) 
        {
            clearObjs[objCount].SetActive(true);
            objCount++;

            count = 0;
        }

    }

    public void Retry()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            noTapArea.SetActive(true);
            ManagerAccessor.Instance.sceneMoveManager.SceneMoveRetry();
        }
    }

    public void StageSelect()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            noTapArea.SetActive(true);
                Debug.Log(isLast);

            if (isLast)
            {
                ManagerAccessor.Instance.sceneMoveManager.SceneMoveName("Ending");
            }
            else
                ManagerAccessor.Instance.sceneMoveManager.SceneMoveName("Ending");
        }
    }

    //リザルトの評価描画用
    private string ResultScoreCheck()
    {
        //それぞれの評価取得
        int owner = ResultScoreChange(ManagerAccessor.Instance.dataManager.ownerMissCount);
        int client = ResultScoreChange(ManagerAccessor.Instance.dataManager.clientMissCount);

        if (owner == 3 && client == 3)
            return "心の友";
        if ((owner == 3 && client == 2) || (owner == 2 && client == 3)) 
            return "親友";
        if ((owner == 3 && client == 1) || (owner == 1 && client == 3))
            return "喧嘩中";
        if ((owner == 3 && client == 0) || (owner == 0 && client == 3))
            return "片思い";
        if (owner == 2 && client == 2)
            return "友達";
        if ((owner == 2 && client == 1) || (owner == 1 && client == 2))
            return "友達の友達";
        if ((owner == 2 && client == 0) || (owner == 0 && client == 2))
            return "偽りの仲";
        if (owner == 1 && client == 1)
            return "知り合い";
        if ((owner == 1 && client == 0) || (owner == 0 && client == 1))
            return "初対面";
        if (owner == 0 && client == 0)
            return "他人";

        return "";
    }

    //ミスの回数をリザルト表示用のスコアに変更
    private int ResultScoreChange(int miss)
    {
        if (0 <= miss && miss < 3)
            return 3;
        else if (3 <= miss && miss < 6)
            return 2;
        else if (6 <= miss && miss < 9)
            return 1;
        else if (9 <= miss)
            return 0;

        return 0;
    }

    [PunRPC]
    private void RcpShareIsRetry()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            noTapArea.SetActive(true);
            ManagerAccessor.Instance.sceneMoveManager.SceneMoveRetry();
        }
    }

    [PunRPC]
    private void RcpShareIsStageSelect()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            noTapArea.SetActive(true);
            ManagerAccessor.Instance.sceneMoveManager.SceneMoveName("StageSelect");
        }
    }

    [PunRPC]
    private void RpcShareOwnerMissCount(int miss)
    {
        ManagerAccessor.Instance.dataManager.ownerMissCount = miss;
    }

    [PunRPC]
    private void RpcShareClientMissCount(int miss)
    {
        ManagerAccessor.Instance.dataManager.clientMissCount = miss;
    }
}
