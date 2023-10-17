using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject p = GameObject.Find("Player1");

        Debug.Log(p.name);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
