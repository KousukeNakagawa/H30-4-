using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XrayMachine : MonoBehaviour {

    private bool xrayOK = true;
    private GameObject m_XrayCameraObj;
    private Camera m_XrayCamera;

    public string texnumber = "01";
    private int weekLayerMask;
    private int builLayerMask;

    private GameManager gameManager;

    // Use this for initialization
    void Start () {
        m_XrayCameraObj = transform.Find("XrayCamera").gameObject;
        m_XrayCamera = m_XrayCameraObj.GetComponent<Camera>();
        m_XrayCamera.targetTexture = Resources.Load("Texture/RenderTextures/XrayCamera" + texnumber) as RenderTexture;
        weekLayerMask = LayerMask.GetMask(new string[] { LayerMask.LayerToName(8) });
        builLayerMask = LayerMask.GetMask(new string[] { LayerMask.LayerToName(9) });

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.U))
        {
            Shooting();
        }
	}

    //撮影
    private void Shooting()
    {
        Debug.Log("食らいやがれー");

        //ビルがあるかどうか
        RaycastHit builhit;
        bool hit = Physics.Raycast(
            m_XrayCameraObj.transform.position, m_XrayCameraObj.transform.forward,
            out builhit, m_XrayCamera.farClipPlane, builLayerMask);

        //なかったら終了
        if (!hit) return;

        //弱点が写っているかどうか
        RaycastHit[] weekpoints 
            = Physics.BoxCastAll(m_XrayCameraObj.transform.position,
            new Vector3(MainStageDate.TroutLengthX / 2, m_XrayCameraObj.transform.position.y, MainStageDate.TroutLengthZ / 2),
            m_XrayCameraObj.transform.forward, Quaternion.identity,m_XrayCamera.farClipPlane, weekLayerMask);

        //写っていなかったら終了
        if (weekpoints.Length < 1) return;
        //ビルの奥にあっても終了
        if (builhit.distance < weekpoints[0].distance) return;

        List<int> weeknums = new List<int>();
        for(int i = 0;i < weekpoints.Length; i++)
        {
            RaycastHit weekhit;
            //向いている方向によってレイのスタート位置を決める
            Vector3 raystartpos = (Mathf.Abs(m_XrayCameraObj.transform.forward.x) > Mathf.Abs(m_XrayCameraObj.transform.forward.z)) ?
                new Vector3(m_XrayCameraObj.transform.position.x, weekpoints[i].transform.position.y, weekpoints[i].transform.position.z) :
                new Vector3(weekpoints[i].transform.position.x, weekpoints[i].transform.position.y, m_XrayCameraObj.transform.position.z);
            Ray ray = new Ray(raystartpos,weekpoints[i].transform.position - raystartpos);
            //スタート位置からレイを飛ばして弱点に当たるかどうか
            if(Physics.Raycast(ray, out weekhit, m_XrayCamera.farClipPlane, weekLayerMask))
            {
                if(weekhit.transform.position == weekpoints[i].transform.position) //対象の弱点に当たった場合
                {
                    weeknums.Add(weekpoints[i].transform.GetComponent<WeekPoint>().GetWeekNumber());
                    //Debug.Log("いい子だ");
                }
                else  //対象の弱点に当たらない場合
                {
                    weekpoints[i].transform.GetComponent<WeekPoint>().HideObject();
                }
            }
            Debug.DrawRay(ray.origin, ray.direction, Color.red, 3.0f);
            Debug.Log(weekpoints[i].transform.name);
        }

        weeknums.Sort();
        if (gameManager != null) gameManager.SetWeekPhoto(texnumber, weeknums);
    }


    /// <summary>撮影（プレイヤーが呼ぶ関数）</summary>
    public void XrayPlay()
    {
        if (xrayOK)
        {
            Shooting(); 
            m_XrayCameraObj.SetActive(false); //使ったカメラは非表示に
            xrayOK = false;
            transform.tag = "XlineEnd";
        }
    }

    /// <summary>csvからもらうデータ</summary>
    public void SetCSVData(string num)
    {
        texnumber = num;
    }
}
