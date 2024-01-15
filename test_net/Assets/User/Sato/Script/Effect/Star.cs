using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    [SerializeField,Header("à⁄ìÆó ")]private float[] movePower;

    [SerializeField, Header("è¨Ç≥Ç≠Ç»ÇÈë¨ìx")] private Vector3 smallPower;

    private Vector3 dir;

    // Start is called before the first frame update
    void Start()
    {
        if (Random.Range(0, 2) == 0) 
            dir.x = Random.Range(movePower[0], movePower[1]);
        else
            dir.x = -Random.Range(movePower[0], movePower[1]);

        if (Random.Range(0, 2) == 0)
            dir.y = Random.Range(movePower[0], movePower[1]);
        else
            dir.y = -Random.Range(movePower[0], movePower[1]);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += dir;
        transform.localScale -= smallPower;
    }
}
