using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MapCreate : MonoBehaviour {

    //private TextAsset csvFile; // CSVファイル
    public TextAsset csvFile; // CSVファイル

    public GameObject builAPrefab;
    public GameObject builBPrefab;
    public GameObject builCPrefab;
    public GameObject XrayMachinePrefab;

    private int row = 0;


    // Use this for initialization
    void Start()
    {

        //csvFile = Resources.Load("yonde") as TextAsset;
        StringReader reader = new StringReader(csvFile.text);


        while (reader.Peek() > -1)
        {
            //一行読み込む
            string line = reader.ReadLine();
            //カンマで区切る
            string[] data = line.Split(',');


            for (int i = 0; i < data.Length; i++)
            {
                //-で区切る
                string[] minidata = data[i].Split('-');

                if (minidata.Length < 2) continue;

                if(minidata[0] == "X") //射影機
                {
                    GameObject createObject = Instantiate(XrayMachinePrefab);
                    //createObject.transform.position = ObjectPosition(minidata[0], minidata[2], i);
                    createObject.transform.position = new Vector3(i * MainStageDate.TroutLengthX + MainStageDate.TroutLengthX / 2, 0, -row * MainStageDate.TroutLengthZ - MainStageDate.TroutLengthZ / 2);
                    createObject.transform.rotation = Quaternion.AngleAxis(RotationSize(minidata[2]), new Vector3(0, 1, 0));
                    createObject.GetComponent<XrayMachine>().SetCSVData(minidata[1]);
                    createObject.transform.parent = transform;
                }
                else { 
                    Arrangement(minidata[0], minidata[1], minidata[2], i);
                }
            }

            row++;
        }


    }

    // Update is called once per frame
    void Update()
    {

    }

    private void Arrangement(string name,string centercount, string direction, int column)
    {
        //生成するビル
        GameObject createObject = null;

        switch (name)
        {
            case "A": createObject = Instantiate(builAPrefab); break;
            case "B": createObject = Instantiate(builBPrefab); break;
            case "C": createObject = Instantiate(builCPrefab); break;
        }

        if (createObject == null) return;

        //ビルの中央の数
        createObject.GetComponent<BuilCreate>().SetCenterCount(centercount);

        //ビルのポジション
        createObject.transform.position = ObjectPosition(name, direction, column);

        //ビルの回転
        createObject.transform.rotation = Quaternion.AngleAxis(RotationSize(direction), new Vector3(0, 1, 0));

        //自分の子にする
        createObject.transform.parent = transform;

    }

    private Vector3 ObjectPosition(string name, string direction, int column)
    {
        Vector3 result = Vector3.zero;

        if(direction == "E") //東向き
        {
            switch (name)
            {
                case "A":
                case "B":
                case "X":
                    result.x = (column + 1) * MainStageDate.TroutLengthX;
                    result.z = -row * MainStageDate.TroutLengthZ;
                    break;
                case "C":
                    result.x = (column + 2) * MainStageDate.TroutLengthX;
                    result.z = -row * MainStageDate.TroutLengthZ;
                    break;
            }
        }
        else if (direction == "S") //南向き
        {
            switch (name)
            {
                case "A":
                case "X":
                    result.x = (column + 1) * MainStageDate.TroutLengthX;
                    result.z = -(row + 1) * MainStageDate.TroutLengthZ;
                    break;
                case "B":
                    result.x = (column + 2) * MainStageDate.TroutLengthX;
                    result.z = -(row + 1) * MainStageDate.TroutLengthZ;
                    break;
                case "C":
                    result.x = (column + 2) * MainStageDate.TroutLengthX;
                    result.z = -(row + 2) * MainStageDate.TroutLengthZ;
                    break;
            }
        }
        else if(direction == "W") //西向き
        {
            switch (name)
            {
                case "A":
                case "X":
                    result.x = column * MainStageDate.TroutLengthX;
                    result.z = -(row + 1) * MainStageDate.TroutLengthZ;
                    break;
                case "B":
                case "C":
                    result.x = column * MainStageDate.TroutLengthX;
                    result.z = -(row + 2) * MainStageDate.TroutLengthZ;
                    break;
            }
        }
        else //北向き
        {
            result.x = column * MainStageDate.TroutLengthX;
            result.z = -row * MainStageDate.TroutLengthZ;
        }

        return result;
    }

    private float RotationSize(string direction)
    {
        //建物の回転
        float rotation = 0;

        if (direction == "E") rotation = 90;
        else if (direction == "S") rotation = 180;
        else if (direction == "W") rotation = 270;

        return rotation;
    }
}
