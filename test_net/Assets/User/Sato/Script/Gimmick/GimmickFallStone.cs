using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimmickFallStone : MonoBehaviour
{
    [SerializeField, Header("落下速度")] public Vector3 Speed;

    [SerializeField, Header("消えるまでのフレーム")] private int DeleteTime;

    private int frameCount = 0;

    // Update is called once per frame
    void FixedUpdate()
    {
        //ポーズ中は止まる
        if (!ManagerAccessor.Instance.dataManager.isPause)
        {
            transform.position -= Speed;

            //一定時間で消える
            frameCount++;
            if (frameCount == DeleteTime) 
            {
                Destroy(gameObject);
            }
        }

        //クリアもしくは死亡時消える
        if (ManagerAccessor.Instance.dataManager.isClear &&
            ManagerAccessor.Instance.dataManager.isDeth)
        {
            Destroy(gameObject);
        }
    }
}
