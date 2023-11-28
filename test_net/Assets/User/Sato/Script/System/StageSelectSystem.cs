using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelectSystem : MonoBehaviour
{
    [SerializeField, Header("ŠK’i")] private GameObject[] stairs;

    // Start is called before the first frame update
    void Start()
    {
        ManagerAccessor.Instance.saveDataManager.ClearDataLoad();

        for (int i = 0; i < ManagerAccessor.Instance.saveDataManager.clearData.Length; i++) 
        {
            if (ManagerAccessor.Instance.saveDataManager.clearData[i] == 1) 
            {
                stairs[0].SetActive(true);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
