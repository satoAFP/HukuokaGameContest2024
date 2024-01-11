using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    private Vector3 dir;

    // Start is called before the first frame update
    void Start()
    {
        dir.x = Random.Range(-0.1f, 0.1f);
        dir.y = Random.Range(-0.1f, 0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += dir;
    }
}
