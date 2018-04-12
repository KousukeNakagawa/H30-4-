using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MapCreate : MonoBehaviour {

    //private TextAsset csvFile; // CSVファイル
    public TextAsset csvFile; // CSVファイル


    // Use this for initialization
    void Start()
    {

        //csvFile = Resources.Load("yonde") as TextAsset;
        StringReader reader = new StringReader(csvFile.text);

        int num = 0;

        while (reader.Peek() > -1)
        {
            num++;
            //一行読み込む
            string line = reader.ReadLine();
            //カンマで区切る
            string[] nums = line.Split(',');
            for (int i = 0; i < nums.Length; i++)
            {
                Debug.Log("列 " + num + ":" + nums[i]);
            }
        }


    }

    // Update is called once per frame
    void Update()
    {

    }
}
