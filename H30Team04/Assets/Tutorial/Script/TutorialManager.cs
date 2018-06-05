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

    //最初の状態の登録
    TutorialState tutorial = TutorialState.move;

    //前のシーン
    TutorialState oldTutorial;

    //各々のシーンの終了フラグ
    bool textStop = false;
    //テキスト変更用
    bool textInput;

    MoveRange range;

    void Start()
    {
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

        player = tutorialObject.player;
        _light = tutorialObject.goalPoint.GetComponent<Light>();

        startPos = player.transform.position;
        startRotate = player.transform.rotation;

        range = tutorialObject.moveRange.GetComponent<MoveRange>();
    }

    void Update()
    {
        //A or B を押した瞬間 true
        textInput = (Input.GetButtonDown("Shutter") || Input.GetButtonDown("Select")) ?
            true : false;

        //現在のチュートリアルの実行
        exe[tutorial]();

        //チュートリアル中に行動範囲外にでたら
        if (range.RangeEnter)
        {
            Debug.Log("行動範囲を出てしまいました");
            //ReStart(tutorial);
            //range.SetRange(player.transform.position, 20);
        }

        //現在のチュートリアルの終了判定
        //現在のテキストが現在のチュートリアルの最後のテキストなら
        if (textCtrl.CurrebtLine == lastTextNum[tutorial])
        {
            textStop = true;
            //画面を暗転させる
            if (textInput) fade.FadeOut();
            //真っ暗になったらチュートリアルを変更
            if (fade.IsMaxAlpha()) ChangeTutorialState();
        }

        //フェードがかかっているとき
        if (!fade.IsMinAlpha() && !textStop)
        {
            fade.FadeIn();
            oldTutorial = tutorial;
        }

        //Debug.Log(ChangeText());
        //Debug.Log(tutorial);
    }

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
            UnlockManager.Unlock(TutorialState.move);
            //プレイヤーが目的地に到達したら
            if (GoalPoint.Goal)
            {
                UnlockManager.Lock(TutorialState.move);
                textInput = true;
                textStop = false;
            }
        }
    }

    void TutorialRotation()
    {
        //目標地点を表示する
        tutorialObject.goalPoint.SetActive(false);

        tutorialObject.lookPoint[0].SetActive(true);
        tutorialObject.lookPoint[1].SetActive(true);

        //一旦テキストを止めたい番号
        var stopNum = 2;
        //今のテキストが止めたいテキストなら止める
        if (textCtrl.CurrebtLine == stopNum) textStop = true;

        //二つの LookPoint が視界に入ったらクリア
        var clear = (tutorialObject.lookPoint[0].GetComponent<LookPoint>().IsLook &&
            tutorialObject.lookPoint[1].GetComponent<LookPoint>().IsLook) ? true : false;

        if(textStop)
        {
            UnlockManager.Unlock(TutorialState.rotation);
        }

        if (clear)
        {

        }
    }

    void TutorialBattle()
    {

    }

    void TutorialShooting()
    {

    }

    /// <summary> チュートリアルシーンの変更 </summary>
    void ChangeTutorialState()
    {
        if (tutorial == oldTutorial)
            tutorial++;

        textStop = false;
    }

    /// <summary> 現在のチュートリアルのリスタート </summary>
    void ReStart(TutorialState tutorial)
    {
        exe[tutorial]();
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
