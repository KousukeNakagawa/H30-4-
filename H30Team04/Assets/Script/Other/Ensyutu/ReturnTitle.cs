using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnTitle : MonoBehaviour {

    //後で修正する　とりあえずexeの時にカーソル出ないように


    public static ReturnTitle Instance
    {
        get;
        private set;
    }

    void Awake()
    {
        // 重複防止措置。
        // 既にある場合は自身を削除する
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
