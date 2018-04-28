using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Xray_SSS : MonoBehaviour
{
    [SerializeField] GameObject lineObject;
    LineRenderer line;

    //同時に捕捉できる射影機の数・捕捉線の始点位置・色・透明度
    [SerializeField, Range(1, 6)] int XrayCaptureNum = 2;
    [SerializeField, Range(0, 10)] int startPoint = 4;
    [SerializeField] Color startColor;
    [SerializeField] Color endColor;
    [SerializeField, Range(1, 2)] float colorDepth = 1;

    GameObject target;

    int XrayNum; //選択している射影機のナンバー（数字で管理）

    void Start()
    {
        line = lineObject.GetComponent<LineRenderer>();
        XrayNum = 0;
    }

    void Update()
    {
        SSS(); //射影機の「Search」「Select」「Shutter」
    }

    /// <summary>
    /// ＊射影機の「Search」「Select」「Shutter」
    /// </summary>
    void SSS()
    {
        //各々の射影機との距離を近い順に保管するリスト
        var sortXrayDistance = new Dictionary<float, GameObject>();

        //射影機の「探索」
        Search(sortXrayDistance);

        if (sortXrayDistance.Count == 0) Destroy(line);

        //var keyArray = sortXrayDistance.Keys.ToArray();
        var keyList = sortXrayDistance.Keys.ToList();
        //if (Input.GetButtonDown("Select")) keyList.Sort();
        //keyList.Reverse();
        
        for (int i = 0; i < XrayCaptureNum; i++)
        {
            if (XrayNum == i)
            {
                //射影機の「選択」
                Select(sortXrayDistance, i, keyList);
                //射影機の「起動」
                Shutter(sortXrayDistance, i, keyList);
            }
        }
    }

    /// <summary>
    /// ＊射影機の「探索」
    /// </summary>
    void Search(Dictionary<float, GameObject> sortXrayDistance)
    {
        //射影機のサーチ
        var XrayMachines = GameObject.FindGameObjectsWithTag("Xline");

        //数字で対象を管理
        if (Input.GetButtonDown("Select")) XrayNum++;
        if (XrayNum >= XrayCaptureNum) XrayNum = 0;

        foreach (var Xray in XrayMachines)
        {
            //本当の位置を取得・プレイヤーに対しての方向を取得
            Vector3 XrayPos = Xray.transform.Find("model").position;
            Vector3 direction = XrayPos - transform.position;

            //各々の射影機との距離を登録
            if (!sortXrayDistance.ContainsKey(direction.sqrMagnitude))
                sortXrayDistance.Add(direction.sqrMagnitude, Xray);
        }

        //最大捕捉数の調整
        if (XrayCaptureNum > XrayMachines.Length) XrayCaptureNum = XrayMachines.Length;
    }

    /// <summary>
    /// ＊射影機の「選択」
    /// </summary>
    void Select(Dictionary<float, GameObject> sortXrayDistance, int i, List<float> keyArray)
    {
        //Debug.Log(keyArray.IndexOf(keyArray.Min()));
        //方向・始点・終点
        Vector3 direction = sortXrayDistance[keyArray[i]].transform.Find("model").position - transform.position;
        Vector3 start = transform.position + direction * startPoint / 100 + Vector3.up * 3;
        Vector3 end = direction / 10;
        //距離に比例する透明度
        float depthCalculation = colorDepth - direction.sqrMagnitude / 10000;
        float depth = (depthCalculation <= 0.5f) ? 0.5f : depthCalculation;

        target = sortXrayDistance[keyArray[i]];

        //選択している射影機と自信を結ぶ線
        DrawLine(start, start + end, startColor, endColor * depth, 2);
    }

    /// <summary>
    /// ＊射影機の「撮影」
    /// </summary>
    void Shutter(Dictionary<float, GameObject> sortXrayDistance, int i, List<float> keyArray)
    {
        if (sortXrayDistance.Count <= 0) return;

        if (Input.GetButtonDown("Shutter")) //選択中の射影機の起動
        {
            sortXrayDistance[keyArray[i]].GetComponent<XrayMachine>().XrayPlay();
            XrayNum = 0;
        }
    }

    void DrawLine(Vector3 p1, Vector3 p2, Color c1, Color c2, float width, bool isSharp = true)
    {
        line.SetPosition(0, p1);
        line.SetPosition(1, p2);
        line.startColor = c1;
        line.endColor = c2;
        line.startWidth = width;
        line.endWidth = (isSharp) ? 0 : width;
    }

    public GameObject GetTargetXray()
    {
        return target;
    }
}
