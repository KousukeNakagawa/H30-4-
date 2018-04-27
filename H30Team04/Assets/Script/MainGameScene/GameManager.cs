using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    enum PhaseState
    {
        photoState,
        PhotoCheckState,
        attackState
    }

    const int WEEKBONUS = 10; //弱点部位へのボーナス 

    //それぞれの弱点の確立
    public struct WeekPointProbability
    {
        public int num; //弱点番号
        public int probability; //弱点の確立
    }

    public struct WeekPointData
    {
        public string name; //撮影に使った画像の名前
        public List<WeekPointProbability> datas; //弱点の情報
    }

    private List<WeekPointData> weekDatas;
    private int weeknumber; //敵の弱点の数字
    [SerializeField]
    private int weekcount = 0; //弱点の数
    [SerializeField]
    private int mapXsize = 0; //マップのx方向のマスの数


    private PhaseState phaseState;
    [SerializeField]
    bool test = false;  //必ず消し去るbool型
    public int testnum = 0;
    // Use this for initialization
    void Start () {
        phaseState = PhaseState.photoState;

        weeknumber = Random.Range(0, weekcount);
        weekDatas = new List<WeekPointData>();
    }
	
	// Update is called once per frame
	void Update () {
        
    }
    void FixedUpdate()
    {
        switch (phaseState)
        {
            case PhaseState.photoState: PhotoState(); break;
            case PhaseState.PhotoCheckState: PhotoCheckState(); break;
            case PhaseState.attackState: AttackState(); break;
        }
    }


        private void PhotoState()
    {
        GameClear();
        GameOver();
        PhaseTransition();
    }

    private void PhotoCheckState()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            phaseState = PhaseState.attackState;
        }
    }

    public void AttackState()
    {

    }

    void GameClear()
    {
        //if (m_Enemy == null)
        //{
        //    Debug.Log("ゲームクリア");
        //}
    }

    void GameOver()
    {
        //if (m_Player == null)
        //{
        //    Debug.Log("ゲームオーバー");
        //}
    }

    void PhaseTransition()
    {
        if (test||BigEnemyScripts.mTransform.position.x > (mapXsize - 1) * MainStageDate.TroutLengthX)
        {
            phaseState = PhaseState.PhotoCheckState;
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

    public List<WeekPointData> GetWeekPointData
    {
        get { return weekDatas; }
    }

    public bool PhotoCheckStateNow()
    {
        return phaseState == PhaseState.PhotoCheckState;
    }

    public bool AttackStateNow()
    {
        return phaseState == PhaseState.attackState;
    }

    public void PlDes()
    {

    }
}
