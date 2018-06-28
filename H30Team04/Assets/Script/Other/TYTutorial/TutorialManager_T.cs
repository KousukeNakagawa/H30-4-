using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TutorialState_T
{
    START,      //開始
    UI,         //ui説明
    MOVE,       //移動
    CAMERA,     //カメラ移動
    ROBOT,      //ロボット出現
    BEACON,     //ビーコン誘導
    CURSOR,     //カーソル説明
    SHUTTER,    //撮影
    CURSORCHANGE, //カーソル切り替え
    CURSORCHANGEEND, //カーソル切り替え終わり
    SNIPER,     //スナイパー
    SHOTEFFECT, //射撃フェーズへの移行演出
    XRAYEFFECT,  //レントゲン演出
    XRAYCHECK,  //レントゲン確認
    SHOT,       //射撃
    END,         //終了
    TRUEEND     //ほんとに終了
}

public enum ResetConditions
{
    PLAYERAWAY,     //プレイヤーが外に出た時
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

    public GameObject cursorUI;

    public GameObject xray3;

    public GameObject padUIs;
    

    private Transform playerStartTrans;
    private Transform enemyStartTrans;

    public TutorialText_T m_TextManager;

    private bool isReseting = false;
    private ResetConditions nowCondition = ResetConditions.PLAYERAWAY;

    public ShootingCollider sc;

    public AudioSource[] bgms;

    public GameObject lifeUI;

    private bool okhakase = false;
    public AudioClip okse;
    private AudioSource m_Audio;

    public TutorialXlinePhoto m_photo;

    public bool Shot { get; set; }
    

	// Use this for initialization
	void Start () {
        Shot = false;
        m_Audio = GetComponent<AudioSource>();
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
        cursorUI.SetActive(false);
        padUIs.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {

        if (isClear)
        {
            if (!m_TextManager.IsCompleteDisplayText)
            {
                if (Input.GetButtonDown("Shutter"))
                {
                    m_TextManager.SkipScenario();
                }
            }
            else
            {
                if (Input.GetButtonDown("Shutter"))
                {
                    NextState();
                    isClear = false;
                }
            }
            return;
        }

        if (isReseting && !isFadeNow)
        {
            if (!m_TextManager.IsCompleteDisplayText)
            {
                if (Input.GetButtonDown("Shutter"))
                {
                    m_TextManager.SkipScenario();
                }
            }
            else 
            {
                if (Input.GetButtonDown("Shutter"))
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
                if (m_State == TutorialState_T.TRUEEND) return;
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
            case TutorialState_T.UI: UIUpdate();break;
            case TutorialState_T.MOVE:MoveUpdate();  break;
            case TutorialState_T.CAMERA: CameraUpdate(); break;
            case TutorialState_T.ROBOT: RobotUpdate(); break;
            case TutorialState_T.BEACON:BeaconUpdate();  break;
            case TutorialState_T.CURSOR: CursorUpdate(); break;
            case TutorialState_T.SHUTTER:ShutterUpdate(); break;
            case TutorialState_T.CURSORCHANGE: CursorChangeUpdate(); break;
            case TutorialState_T.CURSORCHANGEEND: CursorChangeEndUpdate(); break;
            case TutorialState_T.SNIPER: SniperUpdate(); break;
            case TutorialState_T.SHOTEFFECT: ShotPhaseUpdate(); break;
            case TutorialState_T.XRAYEFFECT: XrayEffectUpdate(); break;
            case TutorialState_T.XRAYCHECK: XrayCheckUpdate(); break;
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
            case TutorialState_T.SNIPER: SniperStart(); break;
            case TutorialState_T.SHOTEFFECT: ShotPhaseStart(); break;
            case TutorialState_T.TRUEEND:TrueEnd();break;
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
            case TutorialState_T.UI: UIStart(); break;
            case TutorialState_T.BEACON: BeaconStart(); break;
            case TutorialState_T.CURSOR: CursorStart(); break;
            case TutorialState_T.SHUTTER:ShutterStart(); break;
            case TutorialState_T.CURSORCHANGE:CursorChangeStart(); break;
            case TutorialState_T.CURSORCHANGEEND:CursorChangeEndStart(); break;
            case TutorialState_T.XRAYEFFECT: XrayEffectStart(); break;
            case TutorialState_T.XRAYCHECK: XrayCheckStart(); break;
            case TutorialState_T.SHOT: ShotStart(); break;
            case TutorialState_T.END: EndStart(); break;
        }
        //if (m_State == TutorialState_T.MOVE || m_State == TutorialState_T.SHUTTER || m_State == TutorialState_T.BEACON
        //    || m_State == TutorialState_T.XRAYCHECK || m_State == TutorialState_T.SHOT || m_State == TutorialState_T.END) return;
        //else Fade.FadeOut(1.0f);
        //isFadeNow = true;
        if (m_State == TutorialState_T.CAMERA || m_State == TutorialState_T.ROBOT
            || m_State == TutorialState_T.SNIPER || m_State == TutorialState_T.SHOTEFFECT
            || m_State == TutorialState_T.TRUEEND)
        {
            Fade.FadeOut(1.0f);
            isFadeNow = true;
        }
    }

    /// <summary>リセット宣言</summary>
    public void ResetState(ResetConditions condition)
    {
        isReseting = true;
        //nowCondition = condition;
        switch (condition)
        {
            case ResetConditions.PLAYERAWAY: m_TextManager.TextStart(27); break;
            case ResetConditions.PLAYERDEAD: m_TextManager.TextStart(28); break;
            case ResetConditions.MISSIONFAILD: m_TextManager.TextStart(26); break;
        }

    }

    /// <summary>リセット処理</summary>
    private void ResetTutorial()
    {

        switch (m_State)
        {
            case TutorialState_T.MOVE: PlayerReset(); break;
            case TutorialState_T.BEACON: PlayerReset();EnemyReset(); BuildingReset(); break;
            case TutorialState_T.CURSOR: PlayerReset(); EnemyReset(); BuildingReset(); break;
            case TutorialState_T.SHUTTER: ShutterReset(); break;
            case TutorialState_T.SNIPER: PlayerReset(); break;
            case TutorialState_T.XRAYCHECK: break;
            case TutorialState_T.SHOT: ShotReset(); break;
        }
        m_TextManager.HidePanel();
        isReseting = false;
        //isTextreaded = false;
    }

    private void StepClear()
    {
        m_Audio.PlayOneShot(okse);
        m_TextManager.TextStart(29);
        isClear = true;
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
            if (Input.GetButtonDown("Shutter"))
            {
                nowStateTextNum = 0;
                isTextreaded = true;
                m_TextManager.HidePanel();
                if(m_State == TutorialState_T.CURSOR)
                {
                    cursorUI.SetActive(false);
                }
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
            if (Input.GetButtonDown("Shutter"))
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
            if (Input.GetButtonDown("Shutter"))
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
            if (Input.GetButtonDown("Shutter"))
            {
                m_TextManager.SkipOpenFace(2.0f);
            }
        }
        else if (!m_TextManager.IsCompleteDisplayText)
        {
            if (Input.GetButtonDown("Shutter"))
            {
                m_TextManager.SkipScenario();
            }
        }
        else
        {
            if (Input.GetButtonDown("Shutter"))
            {
                NextState();
                m_TextManager.ClosePhDface(3.0f);
            }
        }
    }

    private void UIUpdate()
    {
        NextState();
    }

    private void MoveUpdate()
    {
        if((playerTrans.position - movePoint.transform.position).ToTopView().sqrMagnitude < 5*5)
        {
            StepClear();
        }
    }

    private void CameraUpdate()
    {
        if(look1.IsLook && look2.IsLook)
        {
            StepClear();
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
            StepClear();
        }
    }

    private void CursorUpdate()
    {
        NextState();
    }

    private void ShutterUpdate()
    {

        if(maku.activeSelf)
        {
            StepClear();
        }
    }

    private void CursorChangeUpdate()
    {
        //選んでいる射影機が変わったかをもらう
        if(playerTrans.gameObject.GetComponent<TutorialXray_SSS>().GetTarget() == xray3)
        {
            StepClear();
        }
        
    }

    private void CursorChangeEndUpdate()
    {
        NextState();
    }

    private void SniperUpdate()
    {
        if(drone == null)
        {
            StepClear();
        }
    }

    private void ShotPhaseUpdate()
    {
        if (switchCamera == null)
        {
            NextState();
        }
    }

    private void XrayEffectUpdate()
    {
        //フィルムの移動が終わったら、bool型をもらう
        if (m_photo.IsFilmEnd)
        {
            NextState();
        }
    }

    private void XrayCheckUpdate()
    {
        if (Input.GetButtonDown("WeaponChange"))
        {
            StepClear();
        }
    }

    private void ShotUpdate()
    {
        if (TutorialEnemyScripts.breakEffectManager.isEnd) StepClear();
    }

    private void EndUpdate()
    {
        NextState();
    }

    //開始時の処理達
    private void StartStart()
    {
    }

    private void UIStart()
    {
        padUIs.SetActive(true);
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

    private void CursorStart()
    {

        //カーソルを説明するUI表示
        cursorUI.SetActive(true);
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

        m_State = TutorialState_T.BEACON;
        nowTextIndex--;
    }

    private void CursorChangeStart()
    {
    }

    private void CursorChangeEndStart()
    {
    }

    private void SniperStart()
    {
        beacon1.SetActive(false);
        //beacon2.SetActive(false);

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
        lifeUI.SetActive(false);

        bgms[0].enabled = false;
        bgms[1].enabled = true;

        TutorialEnemyScripts.shootingPhaseMove.ShootingPhaseSet();
    }

    private void XrayEffectStart()
    {
        //sc.ColliderOn();
    }

    private void XrayCheckStart()
    {
        sc.ColliderOn();
    }

    private void ShotStart()
    {

    }

    private void ShotReset()
    {
        // PlayerReset(); EnemyReset(); BuildingReset();

        m_State = TutorialState_T.XRAYEFFECT;
        nowTextIndex = 21;

        Shot = false;

        m_photo.RestFilm();
    }

    private void EndStart()
    {
        Time.timeScale = 1;
        bgms[1].enabled = false;
        bgms[2].enabled = true;
        //ScecnManager.SceneChange("GameStart1");
    }

    private void TrueEnd()
    {
        ScecnManager.SceneChange("GameStart1");
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
        return (isTextreaded &&!isReseting && !isClear);
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
