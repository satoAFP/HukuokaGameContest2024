using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGetHitObjTagManagement : MonoBehaviour
{
    [SerializeField, Header("右当たり判定")] private PlayerGetHitObjTag rightJudge;
    [SerializeField, Header("左当たり判定")] private PlayerGetHitObjTag leftJudge;
    [SerializeField, Header("下当たり判定")] private PlayerGetHitObjTag downJudge;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < rightJudge.HitTags.Count; i++) 
        {

        }


    }
}
