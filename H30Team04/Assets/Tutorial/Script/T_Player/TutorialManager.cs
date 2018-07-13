using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] TextController textCtrl;

    //フェード機能の実装
    [SerializeField, Tooltip("フェード機能")]
    FadeFunction fadeFunction;
    FadeObject fade;

    //デリゲートをステートパターンで管理
    delegate void Exe();
    Dictionary<TutorialState, Exe> exe = new Dictionary<TutorialState, Exe>();

    //各チュートリアルの最後のテキストの管理
    [SerializeField, Space(1), Tooltip("各チュートリアルの最後のテキスト番号")]
    LastText lastText;
    Dictionary<TutorialState, int> lastTextNum = new Dictionary<TutorialState, int>();

    //チュートリアルで使用するオブジェクトの取得
    [SerializeField, Space(1), Tooltip("使用するオブジェクト")]
    TutorialObject tutorialObject;
    GameObject player;
    Light _light;

    //プレイヤーの初期情報
    Vector3 startPos;
    Quaternion startRotate;
    Vector3 startAngles;

    //最初の状態の登録
    TutorialState tutorial = TutorialState.move;

    //前のシーン
    TutorialState oldTutorial;

    //各々のシーンの終了フラグ
    bool textStop = false;
    //テキスト変更用
    bool textInput;

    void Start()
    {
        UnlockManager.AllSet(false);

        //チュートリアルのシーンの登録
        exe.Add(TutorialState.move, TutorialMove);
        exe.Add(TutorialState.rotation, TutorialRotation);
        exe.Add(TutorialState.battle, TutorialBattle);
        exe.Add(TutorialState.shooting, TutorialShooting);

        //各チュートリアルの最後のテキストナンバーの取得
        lastTextNum.Add(TutorialState.move, lastText.T_move);
        lastTextNum.Add(TutorialState.rotation, lastText.T_rotation);
        lastTextNum.Add(TutorialState.battle, lastText.T_battle);
        lastTextNum.Add(TutorialState.shooting, lastText.T_shooting);

        //フェードスクリプトの取得
        fade = fadeFunction._image.GetComponent<FadeObject>();

        tutorialObject.goalPoint.SetActive(false);
        tutorialObject.lookPoint[0].SetActive(false);
        tutorialObject.lookPoint[1].SetActive(false);
        tutorialObject.beaconPoint.SetActive(false);

        player = tutorialObject.player;
        _light = tutorialObject.goalPoint.GetComponent<Light>();

        startPos = player.transform.position;
        startRotate = player.transform.rotation;
        startAngles = tutorialObject.playerCamera.transform.eulerAngles;
    }

    void Update()
    {
        //A or B を押した瞬間 true
        textInput = (Input.GetButtonDown("Shutter") || Input.GetButtonDown("Select")) ?
            true : false;

        //チュートリアル中に行動範囲外にでたら
        if (MoveRange.RangeExit)
        {
            Debug.Log("行動範囲を出てしまいました");
            //ReStart(tutorial);
            MoveRange.SetRange(player.transform.position, 20);
        }

        //現在のチュートリアルの実行
        exe[tutorial]();

        //現在のチュートリアルの終了判定
        //現在のテキストが現在のチュートリアルの最後のテキストなら
        if (textCtrl.CurrebtLine == lastTextNum[tutorial])
        {
            textStop = true;
            //画面を暗転させる
            fade.FadeOut();
            //真っ暗になったらチュートリアルを変更
            if (fade.IsMaxAlpha()) ChangeTutorialState();
        }
        //フェードがかかっているとき
        //if (!fade.IsMinAlpha() && !textStop)
        if (textCtrl.CurrebtLine != lastTextNum[tutorial])
        {
            fade.FadeIn();
            oldTutorial = tutorial;
        }
        //Debug.Log(ChangeText());
        //Debug.Log(tutorial);
        //Debug.Log(textCtrl.CurrebtLine);
    }

    /// <summary> 移動のチュートリアル </summary>
    void TutorialMove()
    {
        //目標地点を表示する
        tutorialObject.goalPoint.SetActive(true);

        //一旦テキストを止めたい番号
        var stopNum = 2;
        //今のテキストが止めたいテキストなら止める
        if (textCtrl.CurrebtLine == stopNum) textStop = true;

        //動き
        if (textStop)
        {
            //移動の解除
            UnlockManager.Unlock(UnlockState.move);
            //プレイヤーが目的地に到達したら
            if (GoalPoint.IsGoal)
            {
                Soldier.isStop = true;
                UnlockManager.AllSet(false);
                textStop = false;
                textInput = true;
            }
        }
    }

    /// <summary> 回転のチュートリアル </summary>
    void TutorialRotation()
    {
        //移動チュートリアル用オブジェクトの破壊
        Destroy(tutorialObject.goalPoint);

        tutorialObject.lookPoint[0].SetActive(true);
        tutorialObject.lookPoint[1].SetActive(true);
        Soldier.isStop = false;

        //一旦テキストを止めたい番号
        var stopNum = 5;
        //今のテキストが止めたいテキストなら止める
        if (textCtrl.CurrebtLine == stopNum) textStop = true;

        if (textStop)
        {
            UnlockManager.Unlock(UnlockState.move);
            //二つの LookPoint が視界に入ったらクリア
            var clear = (tutorialObject.lookPoint[0].GetComponent<LookPoint>().IsLook &&
                tutorialObject.lookPoint[1].GetComponent<LookPoint>().IsLook) ? true : false;

            if (clear)
            {
                Soldier.isStop = true;
                UnlockManager.AllSet(false);
                textStop = false;
                textInput = true;
            }
        }
    }

    /// <summary> アクションのチュートリアル </summary>
    void TutorialBattle()
    {
        //回転チュートリアル用オブジェクトの破壊
        Destroy(tutorialObject.lookPoint[0]);
        Destroy(tutorialObject.lookPoint[1]);

        //BigEnemyの出現

        //一旦テキストを止めたい番号
        var stopNum = new int[] { 9, 14, 20 };

        //今のテキストが止めたいテキストなら止める
        if (textCtrl.CurrebtLine == stopNum[0])
        {
            UnlockManager.Unlock(UnlockState.move);
            UnlockManager.Unlock(UnlockState.beacon);
            UnlockManager.Unlock(UnlockState.laserPointer);

            BeaconPhase();
        }

        if (textCtrl.CurrebtLine == stopNum[1])
        {
            UnlockManager.Unlock(UnlockState.move);
            UnlockManager.Unlock(UnlockState.laserPointer);
            UnlockManager.Unlock(UnlockState.beacon);
            UnlockManager.Unlock(UnlockState.snipe);
            SnipePhase();
        }

        if (textCtrl.CurrebtLine == stopNum[2])
        {
            UnlockManager.Unlock(UnlockState.move);
            UnlockManager.Unlock(UnlockState.xray);
            XrayPhase();
        }
    }

    void BeaconPhase()
    {
        Soldier.isStop = false;
        tutorialObject.beaconPoint.SetActive(true);
        //一旦テキストを止める
        textStop = true;

        //ビーコンが目的地に命中したらテキストを進める
        if (BeaconPoint.IsBeaconHit)
        {
            UnlockManager.AllSet(false);
            textStop = false;
            textInput = true;
        }

        //またテキストを止めたい番号
        var stopNum = 11;

        if (textCtrl.CurrebtLine == stopNum)
        {
            //テキストを止め、ビッグエネミーが誘導されるのを待つ
            textStop = true;
            if (BeaconPoint.IsBigEnemyHit || textInput)
            {
                //誘導されたらテキストを進める
                textStop = false;
                textInput = true;
            }
        }
    }

    void SnipePhase()
    {
        //ドローンを出現させる

        if (WeaponCtrl.IsWeaponBeacon /*&&
            GameObject.FindGameObjectWithTag("SmallEnemy") != null*/) textStop = true;

        if (!WeaponCtrl.IsWeaponBeacon)
        {
            textStop = false;
            textInput = true;
        }

        //またテキストを止めたい番号
        var stopNum = 15;

        if (textCtrl.CurrebtLine == stopNum)
        {
            textStop = true;
            //ドローンがいなくなったら
            if (GameObject.FindGameObjectWithTag("SmallEnemy") == null || textInput)
            {
                UnlockManager.AllSet(false);
                textStop = false;
                textInput = true;
            }
        }
    }

    void XrayPhase()
    {
        textStop = true;
        if (Input.GetButtonDown("Select"))
        {
            textStop = false;
            textInput = true;
        }

        //またテキストを止めたい番号
        var stopNum = 21;

        if (textCtrl.CurrebtLine == stopNum)
        {
            textStop = true;
            //射影機を全て使う
            if (GameObject.FindGameObjectsWithTag("Xline") == null || textInput)
            {
                Soldier.isStop = false;
                UnlockManager.AllSet(false);
                textStop = false;
                textInput = true;
            }
        }
    }

    void TutorialShooting()
    {

    }

    /// <summary> チュートリアルシーンの変更 </summary>
    void ChangeTutorialState()
    {
        PlayerReset();

        if (tutorial == oldTutorial)
            tutorial++;

        textStop = false;
    }

    /// <summary> 現在のチュートリアルのリスタート </summary>
    void ReStart(TutorialState tutorial)
    {
        exe[tutorial]();
    }

    /// <summary> プレイヤーのリセット </summary>
    void PlayerReset()
    {
        player.transform.position = startPos;
        player.transform.rotation = startRotate;
        Camera.main.transform.eulerAngles = startAngles;
    }

    /// <summary> チュートリアルシーンの取得 </summary>
    public TutorialState GetTutorialState()
    {
        return tutorial;
    }

    /// <summary> テキストを進める </summary>
    public bool ChangeText()
    {
        //A or B を押した瞬間 かつ 
        //テキストを止めていないなら
        return ((textInput && !textStop)) ? true : false;
    }
}
