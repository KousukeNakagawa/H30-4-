using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public enum PhaseState
    {
        photoState,
        endState,
        switchState,
        PhotoCheckState,
        attackState,
        waitingState
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
    public GameObject m_GameClear;
    public GameObject m_GameOver;
    public GameObject m_endCam;
    public GameObject m_switchCam;
    public GameObject m_attackP;
    public TextController m_textcontroller;
    public GameObject m_camera;
    GameObject m_player;
    GameObject m_enemy;
    CameraController m_CC;
    public GameObject minimap;
    private int weeknumber; //敵の弱点の数字
    [SerializeField]
    private int weekcount = 0; //弱点の数
    [SerializeField]
    private int mapXsize = 0; //マップのx方向のマスの数

    private bool gameend = false;
    private bool overflash = false, clearflash = false;


    private PhaseState phaseState;

    bool isxrayzero = false; //射影機をすべて使い終わったか
    public int testnum = 0;

    [SerializeField] private AudioSource[] bgms;
    private int nowBGM = 0;

    [SerializeField] private GameObject[] lifeIcons;
    private int oldLife;
    private int _frame = 0;
    private Soldier playerScript;

    [SerializeField] private float endXpos = 370;
    [SerializeField] Text m_PushA;

    Vector3 _overScale,_clearScale;
    float _overtime = 0,_cleartime=0;

    public GameObject limitTimerObj;
    private Text limitText;

    [SerializeField] private float limit = 30.0f;

    // Use this for initialization
    void Start () {
        m_attackP.SetActive(false);
        phaseState = PhaseState.photoState;
        //m_camera = Camera.main.gameObject;
        m_player = GameObject.FindGameObjectWithTag("Player");
        m_enemy = GameObject.FindGameObjectWithTag("BigEnemy").transform.root.gameObject;
        //m_CC = m_camera.transform.parent.parent.GetComponent<CameraController>();
        weeknumber = Random.Range(0, weekcount);
        //weeknumber = 0; //テスト用
        weekDatas = new List<WeekPointData>();
        m_GameClear.SetActive(false);
        m_GameOver.SetActive(false);
        playerScript = m_player.GetComponent<Soldier>();
        oldLife = playerScript.GetResidue();
        GameTextController.TextStart(0);
        UnlockManager.AllSet(true);
        _overScale = m_GameOver.transform.localScale;
        _clearScale = m_GameClear.transform.localScale;
        _overtime = Time.time;
        _cleartime = Time.time;
        m_PushA.gameObject.SetActive(false);
        limitText = limitTimerObj.transform.Find("Text").GetComponent<Text>();
        limitText.text = ((int)Mathf.Ceil(limit)).ToString();
        limitTimerObj.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
        switch (phaseState)
        {
            case PhaseState.photoState: PhotoState(); break;
            case PhaseState.endState: EndState(); break;
            case PhaseState.switchState: SwitchState(); break;
            case PhaseState.PhotoCheckState: PhotoCheckState(); break;
            case PhaseState.attackState: AttackState(); break;
            case PhaseState.waitingState: WaitState(); break;
        }

        if (overflash)
        {
            m_GameOver.SetActive(true);
            iTween.ScaleTo(m_GameOver, iTween.Hash("x", 1, "y", 1, "z", 1, "time", 6));
            _overScale = m_GameOver.transform.localScale;
            Invoke("OverFlash", 5f);
        }

        if (clearflash)
        {
            m_GameClear.SetActive(true);
            iTween.ScaleTo(m_GameClear, iTween.Hash("x", 1, "y", 1, "z", 1, "time", 6));
            _clearScale = m_GameClear.transform.localScale;
            Invoke("ClearFlash", 5f);
        }
    }


    private void PhotoState()
    {
        // GameClear();
        //GameOver();
        if (oldLife != playerScript.GetResidue())
        {
            oldLife = playerScript.GetResidue();
            lifeIcons[oldLife].SetActive(false);
        }
        if (playerScript.Annihilation())
        {
            Fade.FadeOut();
            ChengeWait();
            
        }
        PhaseTransition();
    }

    private void EndState()
    {
        BigEnemyScripts.shootingPhaseMove.moveSpeed = 2;
        if (BigEnemyScripts.mTransform.position.x > (mapXsize - 0) * MainStageDate.TroutLengthX)
        {
            GameTextController.TextStart(7);
            m_endCam.SetActive(false);
            m_switchCam.SetActive(true);
            phaseState = PhaseState.switchState;
        }
    }

    private void SwitchState()
    {
        BigEnemyScripts.shootingPhaseMove.moveSpeed = 0;
        //if (Fade.IsFadeOutOrIn() && Fade.IsFadeEnd())
        //{
        //    m_player.SetActive(false);
        //}
        if (!Fade.IsFadeOutOrIn() && !Fade.IsFadeEnd())
        {
            //m_attackP.SetActive(true);
            //m_attackP.transform.Find("Camera").GetComponent<AudioListener>().enabled = false;
            SetBGM(1);
            GameTextController.TextStart(8);
        }
        if (m_switchCam == null)
        {
            //m_attackP.transform.Find("Camera").GetComponent<AudioListener>().enabled = true;
            phaseState = PhaseState.PhotoCheckState;
        }
    }

    private void PhotoCheckState()
    {
        if(BigEnemyScripts.mTransform.position.x >= endXpos)
        {
            ChengeWait();
            BigEnemyScripts.shootingFailure.FailureAction();
        }
    }

    public void AttackState()
    {
        limitText.text = ((int)Mathf.Ceil(limit)).ToString();
        if (limit == 0)
        {
            ChengeWait();
            BigEnemyScripts.shootingFailure.FailureAction();
            GameTextController.TextStart(11);
            return;
        }
        limit -= Time.deltaTime;
        if (limit < 0) limit = 0;
    }

    public void WaitState()
    {
        if (gameend)
        {
            if (Input.anyKeyDown)
            {
                ScecnManager.SceneChange("GameStart1");
            }

        }
        else
        {
            if (BigEnemyScripts.breakEffectManager.isEnd) GameClear();
            if (Fade.IsFadeOutOrIn() && Fade.IsFadeEnd()) GameOver();
        }
        
    }


    public void GameClear()
    {
        clearflash = true;
        GameTextController.TextStart(10);
       // m_GameClear.SetActive(true);
        Time.timeScale = 1;
        m_enemy.SetActive(false);
        SetBGM(2);
        gameend = true;
    }

    public void GameOver()
    {
      //  m_GameOver.SetActive(true);
        overflash = true;
        SetBGM(3);
        gameend = true;
    }

    void PhaseTransition()
    {
        if (isxrayzero||BigEnemyScripts.mTransform.position.x > (mapXsize - 0) * MainStageDate.TroutLengthX)
        {
            GameTextController.TextStart(15);
            m_player.SetActive(false);
            //Camera.main.gameObject.SetActive(false);
            m_camera.SetActive(false);
            PlDes();

            Vector3 enemyPos = BigEnemyScripts.mTransform.position;
            enemyPos.x = (mapXsize * MainStageDate.TroutLengthX) - (0.5f * MainStageDate.TroutLengthX);
            BigEnemyScripts.mTransform.position = enemyPos;

            minimap.SetActive(false);
            lifeIcons[0].transform.parent.gameObject.SetActive(false);
            m_endCam.SetActive(true);
            UnlockManager.AllSet(false);
            phaseState = PhaseState.endState;
        }
    }
    public void ChengeWait()
    {
        BigEnemyScripts.shootingPhaseMove.moveSpeed = 0;
        phaseState = PhaseState.waitingState;
        limitTimerObj.SetActive(false);
    }

    public void ChengeShot()
    {
        BigEnemyScripts.shootingPhaseMove.enabled = false;
        phaseState = PhaseState.attackState;
        limitTimerObj.SetActive(true);
    }

    public void XrayZero()
    {
        isxrayzero = true;
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

    public bool PhotoStateNow()
    {
        return phaseState == PhaseState.photoState;
    }

    public bool PhotoCheckStateNow()
    {
        return phaseState == PhaseState.PhotoCheckState;
    }

    public bool AttackStateNow()
    {
        return phaseState == PhaseState.attackState;
    }

    public PhaseState NowState()
    {
        return phaseState;
    }

    public void PlDes()
    {
        bool test = false;
        if (!test)
        {
            BigEnemyScripts.shootingPhaseMove.ShootingPhaseSet();
            test = true;
        }
    }

    public void Damege(int i)
    {
        if (weeknumber == i)
        {
            BigEnemyScripts.breakEffectManager.ChangeType();
            //m_attackP.GetComponent<AttackPlayer>().ClearCamera();
        }
        else
        {
            BigEnemyScripts.shootingFailure.FailureAction();
            GameTextController.TextStart(11);
        }
    }

    private void SetBGM(int n)
    {
        if (nowBGM == n) return;
        foreach(var bgm in bgms)
        {
            bgm.enabled = false;
        }
        bgms[n].enabled = true;
        nowBGM = n;
    }

    void OverFlash()
    {
        m_PushA.gameObject.SetActive(true);
        if (Time.time > _overtime)
        {
            float alpha = m_PushA.GetComponent<CanvasRenderer>().GetAlpha();
            if (alpha == 1.0f)
            {
                m_PushA.GetComponent<CanvasRenderer>().SetAlpha(0.0f);
            }
            else
            {
                m_PushA.GetComponent<CanvasRenderer>().SetAlpha(1.0f);
            }
            _overtime += 1;
        }
    }

    void ClearFlash()
    {
        m_PushA.gameObject.SetActive(true);
        if (Time.time > _overtime)
        {
            float alpha = m_PushA.GetComponent<CanvasRenderer>().GetAlpha();
            if (alpha == 1.0f)
            {
                m_PushA.GetComponent<CanvasRenderer>().SetAlpha(0.0f);
            }
            else
            {
                m_PushA.GetComponent<CanvasRenderer>().SetAlpha(1.0f);
            }
            _cleartime += 1;
        }
    }
}
