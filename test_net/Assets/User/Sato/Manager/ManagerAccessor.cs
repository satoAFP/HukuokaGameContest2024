using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerAccessor
{
    //シングルトンパターン
    private static ManagerAccessor instance = null;
    public static ManagerAccessor Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new ManagerAccessor();
            }
            return instance;
        }
    }

    public SceneMoveManager sceneMoveManager;

}