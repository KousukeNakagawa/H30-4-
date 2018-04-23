using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    enum PhaseState
    {
        photoState,
        attackState
    }

    const int WEEKBONUS = 10; //弱点部位へのボーナス 

    //それぞれの弱点の確立
    public struct WeekPointProbability
    {
        public int num; //弱点番号
        public float probability; //弱点の確立
    }

    public struct WeekPointData
    {
        public string name; //撮影に使った画像の名前
        public List<WeekPointProbability> datas; //弱点の情報
    }

    private List<WeekPointData> weekDatas;
    private int weeknumber; //敵の弱点の数字
    [SerializeField]
    private int weekcount; //弱点の数


    private PhaseState phaseState;
    [SerializeField]
    GameObject m_Player;
    [SerializeField]
    GameObject m_Enemy;
    [SerializeField]
    GameObject m_MiniMap;
    [SerializeField]
    float cameraH = 20;
    [HideInInspector]
    public float redLine;
    [SerializeField]
    bool test = false;  //必ず消し去るbool型

    GameObject Flayers;
    GameObject Arrows;
    public int m_FlyerCount;
    bool m_IsChange;
    private int currentSelectStageIndex;

    // Use this for initialization
    void Start () {
        phaseState = PhaseState.photoState;
        currentSelectStageIndex = 0;
        //Flayers = transform.Find("Screen").Find("Flyers").gameObject;
        //Arrows = Flayers.transform.Find("Arrows").gameObject;

        weeknumber = Random.Range(0, weekcount);
        weekDatas = new List<WeekPointData>();
    }
	
	// Update is called once per frame
	void Update () {
        //m_MiniMap.transform.position = new Vector3(m_Player.transform.position.x, m_Player.transform.position.y + cameraH, m_Player.transform.position.z);
        //UpdateSelect();
    }
    void FixedUpdate()
    {
        switch (phaseState)
        {
            case PhaseState.photoState: PhotoState(); break;
            case PhaseState.attackState: AttackState(); break;
        }
    }
    void UpdateSelect()
    {
        const float Margin = 0.5f;
        float inputHorizontal = (Input.GetAxisRaw("XboxLeftHorizontal") != 0) ? Input.GetAxisRaw("XboxLeftHorizontal") : Input.GetAxisRaw("Horizontal");
        if (Mathf.Abs(inputHorizontal) > Margin && !m_IsChange)
        {
            if (inputHorizontal > 0.0f)
            {
                currentSelectStageIndex += 1;
            }
            else
            {
                currentSelectStageIndex += (m_FlyerCount - 1);
            }
            currentSelectStageIndex = currentSelectStageIndex % m_FlyerCount;
            float l_positionX = currentSelectStageIndex * 1280;
            //Arrows.GetComponent<Arrows>().SetTargetLocalPositionX(l_positionX);
            //Flayers.GetComponent<Flyers>().MoveTargetPositionX(-l_positionX);
            m_IsChange = true;
        }
        else if (Mathf.Abs(inputHorizontal) <= Margin)
        {
            m_IsChange = false;
        }
    }

        private void PhotoState()
    {
        GameClear();
        GameOver();
        PhaseTransition();
    }

    private void AttackState()
    {

    }

    void GameClear()
    {
        if (m_Enemy == null)
        {
            Debug.Log("ゲームクリア");
        }
    }

    void GameOver()
    {
        if (m_Player == null)
        {
            Debug.Log("ゲームオーバー");
        }
    }

    void PhaseTransition()
    {
        if (test)
        {
            phaseState = PhaseState.attackState;
        }
    }

    /// <summary>弱点データの保存</summary>
    public void SetWeekPhoto(string texname,List<int> weeknums)
    {
        WeekPointData result;
        result.name = texname;
        List<WeekPointProbability> list = new List<WeekPointProbability>();
        List<int> probabilitys = XrayProbability(weeknums);
        for (int i = 0; i < weeknums.Count; i++)
        {
            WeekPointProbability a;
            a.num = weeknums[i];
            a.probability = probabilitys[i];
            Debug.Log("弱点「" + a.num + "」の確立は" + a.probability + "％");
            list.Add(a);
        }
        result.datas = list;

        weekDatas.Add(result);
    }

    //弱点の確立の割り振り
    private List<int> XrayProbability(List<int> weeknums)
    {
        List<int> result = new List<int>();
        int count = 100;  //合計で100%になるように
        for (int i = 0; i < weeknums.Count; i++)
        {
            int probability = 0;
            if (weeknums[i] == weeknumber)
            {
                probability = WEEKBONUS;  //弱点部位にボーナス分％をプラス
                count -= WEEKBONUS; //そのぶん回す数減らす
            }
            result.Add(probability);
        }

        while(count > 0) //合計100%になるまで
        {
            count--;
            int plus = Random.Range(0, weeknums.Count);
            result[plus]++;
            //Debug.Log("今日は" + weeknums[plus]);
        }

        return result;
    }
}
