using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TutorialState_T
{
    START,      //開始
    MOVE,       //移動
    CAMERA,     //カメラ移動
    ROBOT,      //ロボット出現
    BEACON,     //ビーコン誘導
    REBEACON,   //射影機前に誘導
    SHUTTER,    //撮影
    SNIPER,     //スナイパー
    SHOTEFFECT, //射撃フェーズへの移行演出
    XRAY,       //レントゲン確認
    SHOT,       //射撃
    END         //終了
}

public enum ResetConditions
{
    PLAYERAWAY,     //プレイヤーが外に出た時
    ENEMYAWAY,      //エネミーが外に出た時
    BUILCRUSH,      //幕かかったビル、射影機が壊された時
    NOWEAK,         //弱点を撮影できなかったとき
    TIMEOVER,       //時間切れでミサイルを撃たれた時
    MISSIONFAILD,   //弱点を外した時
    PLAYERDEAD      //プレイヤーと敵が当たったとき
}

public class TutorialManager_T : MonoBehaviour {

   

    private TutorialState_T m_State = TutorialState_T.START;
    private bool isClear = false;
    public int[] textCounts;
    private int nowStateTextNum = 0;
    private int nowTextIndex = 0;
    private bool isFadeNow = false;
    private bool isTextreaded = true;

    public Transform playerTrans;
    public Transform enemyTrans;
    public Transform inStageParent;

    public GameObject movePoint;
    public GameObject look1obj;
    public GameObject look2obj;
    private LookPoint look1;
    private LookPoint look2;
    public GameObject beacon1;
    public GameObject beacon2;

    public GameObject maku;

    public GameObject cutinCamera;
    public GameObject cutinCameraWaku;

    public GameObject attackPlayer;
    public GameObject switchCamera;

    public GameObject drone;

    public GameObject playerCamera;
    

    private Transform playerStartTrans;
    private Transform enemyStartTrans;

    public TutorialText_T m_TextManager;

    private bool isReseting = false;
    private ResetConditions nowCondition = ResetConditions.PLAYERAWAY;

    public ShootingCollider sc;

    public AudioSource[] bgms;

    public bool Shot { get; set; }
    

	// Use this for initialization
	void Start () {
        Shot = false;
        playerStartTrans = playerTrans;
        enemyStartTrans = enemyTrans;

        enemyTrans.gameObject.SetActive(false);

        look1 = look1obj.GetComponent<LookPoint>();
        look2 = look2obj.GetComponent<LookPoint>();
        movePoint.SetActive(false);
        look1obj.SetActive(false);
        look2obj.SetActive(false);
        beacon1.SetActive(false);
        beacon2.SetActive(false);
        cutinCamera.SetActive(false);
        cutinCameraWaku.SetActive(false);
        attackPlayer.SetActive(false);
        switchCamera.SetActive(false);
        drone.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {

        if (isReseting && !isFadeNow)
        {
            if (!m_TextManager.IsCompleteDisplayText)
            {
                if (Input.GetButtonDown("Select"))
                {
                    m_TextManager.SkipScenario();
                }
            }
            else 
            {
                if (Input.GetButtonDown("Select"))
                {
                    Fade.FadeOut(1.0f);
                    isFadeNow = true;
                }
            }
            return;
        }
        

        if (isFadeNow) //フェードアウト中
        {
            if (Fade.IsFadeEnd())//フェードアウトが完了したら
            {
                //暗転した瞬間に呼ばれる
                if (isReseting) ResetTutorial();
                else StateStartBack();
                Fade.FadeIn(1.0f); //フェードイン開始
                isFadeNow = false; 
            }
        }
        else if (Fade.IsFadeEnd()) //フェードインが完了したら
        {
            if (isTextreaded)
            {
                StateUpdate();
            }
            else
            {
                NextTextUpdate();
            }

        }
    }
    /// <summary>各アップデート</summary>
    private void StateUpdate()
    {
        switch (m_State)
        {
            case TutorialState_T.START: StartUpdate(); break;
            case TutorialState_T.MOVE:MoveUpdate();  break;
            case TutorialState_T.CAMERA: CameraUpdate(); break;
            case TutorialState_T.ROBOT: RobotUpdate(); break;
            case TutorialState_T.BEACON:BeaconUpdate();  break;
            case TutorialState_T.REBEACON: ReBeaconUpdate(); break;
            case TutorialState_T.SHUTTER:ShutterUpdate(); break;
            case TutorialState_T.SNIPER: SniperUpdate(); break;
            case TutorialState_T.SHOTEFFECT: ShotPhaseUpdate(); break;
            case TutorialState_T.XRAY: XrayUpdate(); break;
            case TutorialState_T.SHOT: ShotUpdate(); break;
            case TutorialState_T.END: EndUpdate(); break;
        }
    }

    /// <summary>裏方作業</summary>
    private void StateStartBack()
    {
        switch (m_State)//フェード中にやりたいこと
        {
            case TutorialState_T.CAMERA:CameraStart(); break;
            case TutorialState_T.ROBOT:RobotStart(); break;
            case TutorialState_T.REBEACON: ReBeaconStart(); break;
            case TutorialState_T.SNIPER: SniperStart(); break;
            case TutorialState_T.SHOTEFFECT: ShotPhaseStart(); break;
        }
    }

    /// <summary>次の項目へ</summary>
    private void NextState()
    {
        m_State++;
        isTextreaded = false;
        switch (m_State) //フェードせずにやりたいこと
        {
            case TutorialState_T.MOVE: MoveStart(); break;
            case TutorialState_T.BEACON: BeaconStart(); break;
            case TutorialState_T.SHUTTER:ShutterStart(); break;
            case TutorialState_T.XRAY: XrayStart(); break;
            case TutorialState_T.SHOT: ShotStart(); break;
            case TutorialState_T.END: EndStart(); break;
        }
        if (m_State == TutorialState_T.MOVE || m_State == TutorialState_T.SHUTTER || m_State == TutorialState_T.BEACON
            || m_State == TutorialState_T.XRAY || m_State == TutorialState_T.SHOT || m_State == TutorialState_T.END) return;
        else Fade.FadeOut(1.0f);
        isFadeNow = true;
    }

    /// <summary>リセット宣言</summary>
    public void ResetState(ResetConditions condition)
    {
        isReseting = true;
        //nowCondition = condition;
        switch (condition)
        {
            case ResetConditions.PLAYERAWAY: m_TextManager.TextStart(18); break;
            case ResetConditions.PLAYERDEAD: m_TextManager.TextStart(15); break;
            case ResetConditions.ENEMYAWAY: m_TextManager.TextStart(16); break;
            case ResetConditions.BUILCRUSH: m_TextManager.TextStart(17); break;
            case ResetConditions.NOWEAK: m_TextManager.TextStart(20); break;
            case ResetConditions.TIMEOVER: m_TextManager.TextStart(18); break;
            case ResetConditions.MISSIONFAILD: m_TextManager.TextStart(19); break;
        }

    }

    /// <summary>リセット処理</summary>
    private void ResetTutorial()
    {

        switch (m_State)
        {
            case TutorialState_T.MOVE: PlayerReset(); break;
            case TutorialState_T.BEACON: PlayerReset();EnemyReset(); BuildingReset(); break;
            case TutorialState_T.REBEACON: PlayerReset(); EnemyReset(); BuildingReset(); break;
            case TutorialState_T.SHUTTER: ShutterReset(); break;
            case TutorialState_T.SNIPER: PlayerReset(); break;
            case TutorialState_T.XRAY: break;
            case TutorialState_T.SHOT: ShotReset(); break;
        }
        m_TextManager.HidePanel();
        isReseting = false;
    }

    /// <summary>次のテキストへ</summary>
    private void NextText()
    {
        nowTextIndex++;
        m_TextManager.TextStart(nowTextIndex);
    }

    /// <summary>文字送り、ページ送り、枠けし</summary>
    private void NextTextUpdate()
    {
        if(textCounts[(int)m_State] == nowStateTextNum && m_TextManager.IsCompleteDisplayText)
        {
            if (Input.GetButtonDown("Select"))
            {
                nowStateTextNum = 0;
                isTextreaded = true;
                m_TextManager.HidePanel();
                if(m_State == TutorialState_T.SHUTTER)
                {
                    cutinCamera.SetActive(false);
                    cutinCameraWaku.SetActive(false);
                }
            }
            
            return;
        }

        if (!m_TextManager.IsCompleteDisplayText)
        {
            if (Input.GetButtonDown("Select"))
            {
                m_TextManager.SkipScenario();
            }
        }
        else if(nowStateTextNum == 0 && textCounts[(int)m_State] != 0)
        {
            NextText();
            nowStateTextNum++;
        }
        else
        {
            if (Input.GetButtonDown("Select"))
            {
                NextText();
                nowStateTextNum++;
            }
        }
    }

    //アップデート達
    private void StartUpdate()
    {
        if (!m_TextManager.IsFase() && m_TextManager.IsCompleteDisplayText)
        {
            m_TextManager.OpenPhDface(2.0f);
            if (Input.GetButtonDown("Select"))
            {
                m_TextManager.SkipOpenFace(2.0f);
            }
        }
        else if (!m_TextManager.IsCompleteDisplayText)
        {
            if (Input.GetButtonDown("Select"))
            {
                m_TextManager.SkipScenario();
            }
        }
        else
        {
            if (Input.GetButtonDown("Select"))
            {
                NextState();
                m_TextManager.ClosePhDface(3.0f);
            }
        }
    }

    private void MoveUpdate()
    {
        if((playerTrans.position - movePoint.transform.position).ToTopView().sqrMagnitude < 5*5)
        {
            NextState();
        }
    }

    private void CameraUpdate()
    {
        if(look1.IsLook && look2.IsLook)
        {
            NextState();
        }
    }

    private void RobotUpdate()
    {
        NextState();
    }

    private void BeaconUpdate()
    {
        if ((enemyTrans.position.ToTopView() - beacon1.transform.position.ToTopView()).sqrMagnitude < 7*7 )
        {
            NextState();
        }
    }

    private void ReBeaconUpdate()
    {
        if ((enemyTrans.position.ToTopView() - beacon2.transform.position.ToTopView()).sqrMagnitude < 6*6)
        {
            NextState();
        }
    }

    private void ShutterUpdate()
    {

        if(maku.activeSelf)
        {
            NextState();
        }
    }

    private void SniperUpdate()
    {
        if(drone == null)
        {
            NextState();
        }
    }

    private void ShotPhaseUpdate()
    {
        if (switchCamera == null)
        {
            NextState();
        }
    }

    private void XrayUpdate()
    {
        if (Input.GetButtonDown("WeaponChange"))
        {
            NextState();
        }
    }

    private void ShotUpdate()
    {
        if (TutorialEnemyScripts.breakEffectManager.isEnd) NextState();
    }

    private void EndUpdate()
    {
        ScecnManager.SceneChange("GameStart1");
    }

    //開始時の処理達
    private void StartStart()
    {
    }

    private void MoveStart()
    {
        movePoint.SetActive(true);

    }

    private void CameraStart()
    {
        movePoint.SetActive(false);
        PlayerReset();
        look1obj.SetActive(true);
        look2obj.SetActive(true);
    }

    private void RobotStart()
    {
        look1obj.SetActive(false);
        look2obj.SetActive(false);

        PlayerReset();

        enemyTrans.gameObject.SetActive(true);
    }

    private void BeaconStart()
    {
        beacon1.SetActive(true);
    }

    private void ReBeaconStart()
    {
        beacon1.SetActive(false);

        PlayerReset();
        EnemyReset();

        beacon2.SetActive(true);
    }

    private void ShutterStart()
    {
        cutinCamera.SetActive(true);
        cutinCameraWaku.SetActive(true);
    }

    /// <summary>射影機失敗時の処理</summary>
    private void ShutterReset()
    {
        PlayerReset(); EnemyReset(); BuildingReset();

        m_State = TutorialState_T.REBEACON;
        nowTextIndex--;
    }

    private void SniperStart()
    {
        beacon2.SetActive(false);

        PlayerReset();
        EnemyReset();
        BuildingReset();

        enemyTrans.gameObject.SetActive(false);
        drone.SetActive(true);
    }

    private void ShotPhaseStart()
    {
        playerTrans.gameObject.SetActive(false);
        playerCamera.SetActive(false);
        enemyTrans.gameObject.SetActive(true);
        switchCamera.gameObject.SetActive(true);
        attackPlayer.gameObject.SetActive(true);

        bgms[0].enabled = false;
        bgms[1].enabled = true;

        TutorialEnemyScripts.shootingPhaseMove.ShootingPhaseSet();
    }


    private void XrayStart()
    {
        sc.ColliderOn();
    }

    private void ShotStart()
    {

    }

    private void ShotReset()
    {
        // PlayerReset(); EnemyReset(); BuildingReset();

        m_State = TutorialState_T.XRAY;
        nowTextIndex--;

        Shot = false;
    }

    private void EndStart()
    {
        Time.timeScale = 1;
        bgms[1].enabled = false;
        bgms[2].enabled = true;
        //ScecnManager.SceneChange("GameStart1");
    }
    /// <summary>プレイヤーリセット </summary>
    private void PlayerReset()
    {
        playerTrans.gameObject.GetComponent<TutorialPlayer>().Respawn();
        //playerTrans.position = playerStartTrans.position;
        //playerTrans.rotation = playerStartTrans.rotation;
    }
    /// <summary>エネミーリセット</summary>
    private void EnemyReset()
    {
        enemyTrans.gameObject.GetComponent<TutorialEnemyScripts>().Restart();
        //enemyTrans.position = enemyStartTrans.position;
        //enemyTrans.rotation = enemyStartTrans.rotation;
    }

    //private void FadeOut()
    //{
    //    Fade.FadeOut();
    //}
    /// <summary>建物リセット</summary>
    private void BuildingReset()
    {
        foreach(Transform child in inStageParent)
        {
            if (child.gameObject.activeSelf && child.tag == "Building" || m_State > TutorialState_T.SHUTTER && child.tag == "XlineEnd") continue;

            child.gameObject.SetActive(true);
            if (child.tag == "XlineEnd" || child.tag == "Xline") child.gameObject.GetComponent<TutorialXrayMachine>().XrayReset();
            else if(child.tag == "Building") child.gameObject.GetComponent<TutorialBuil>().BuilReset();
        }
    }
    /// <summary>説明を読み終わったかどうか</summary>
    public bool IsReaded()
    {
        return (isTextreaded &&!isReseting);
    }
    /// <summary>現在の状態を取得</summary>
    public TutorialState_T GetState()
    {
        return m_State;
    }

    public void Damege(int i)
    {
        if (0 == i)
        {
            TutorialEnemyScripts.breakEffectManager.ChangeType();
            //m_attackP.GetComponent<AttackPlayer>().ClearCamera();
        }
        else
        {
            ResetState(ResetConditions.MISSIONFAILD);
        }
    }
}
