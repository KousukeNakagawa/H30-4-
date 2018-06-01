using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class FloorCreate : MonoBehaviour {

    [SerializeField] private TextAsset csvFile; // CSVファイル
    [SerializeField] private Material[] floorMats;

    [SerializeField] private GameObject floorPrefab;

    private int row = 0;

    [SerializeField] private Vector3 startPos = Vector3.zero;

    // Use this for initialization
    void Start () {
        StringReader reader = new StringReader(csvFile.text);


        while (reader.Peek() > -1)
        {
            //一行読み込む
            string line = reader.ReadLine();

            line = line.ToUpper();

            //カンマで区切る
            string[] data = line.Split(',');


            for (int i = 0; i < data.Length; i++)
            {
                //-で区切る
                string[] minidata = data[i].Split('-');

                if (minidata[0] == "") continue;
                
                 Arrangement(minidata, i);
                
            }

            row++;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void Arrangement(string[] minidata, int column)
    {
        //生成する
        GameObject createObject = Instantiate(floorPrefab);
        
        if (createObject == null) return;

        createObject.GetComponent<Renderer>().material = floorMats[int.Parse(minidata[0])];

        //ポジション
        createObject.transform.position = ObjectPosition(column) + startPos;

        //回転
        if(minidata.Length > 1)
        {
            Vector3 rotate = createObject.transform.eulerAngles;
            rotate.y = RotationSize(minidata[1]);
            createObject.transform.eulerAngles = rotate;
        }
        

        //自分の子にする
        createObject.transform.parent = transform;

    }

    private Vector3 ObjectPosition(int column)
    {
        Vector3 result = Vector3.zero;
        
        result.x = column * MainStageDate.TroutLengthX / 2 + MainStageDate.TroutLengthX / 4;
        result.z = -row * MainStageDate.TroutLengthZ / 2 - MainStageDate.TroutLengthZ / 4;
       
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
