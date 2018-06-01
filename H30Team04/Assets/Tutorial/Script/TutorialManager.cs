using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    //フェード情報の取得
    [SerializeField] GameObject canvas;
    FadeObject fade;

    //デリゲート
    public delegate void executeState();

    //ステートパターンのDictionary
    public Dictionary<TutorialState, executeState> exe
    = new Dictionary<TutorialState, executeState>();

    //最初の状態の登録
    TutorialState state = TutorialState.start;

    //前のシーン
    TutorialState oldState;

    //各々のシーンの終了フラグ
    bool isEnd = false;

    void Start()
    {
        //チュートリアルのシーンの登録
        exe.Add(TutorialState.start, TutorialStart);
        exe.Add(TutorialState.move, TutorialMove);
        exe.Add(TutorialState.rotation, TutorialRotation);
        exe.Add(TutorialState.weapon, TutorialWeapon);
        exe.Add(TutorialState.xray, TutorialXray);
        exe.Add(TutorialState.shooting, TutorialShooting);
        exe.Add(TutorialState.end, TutorialEnd);

        //フェードスクリプトの取得
        fade = canvas.GetComponent<FadeObject>();
    }

    void Update()
    {
        //各々のチュートリアルの実行
        exe[state]();

        //チュートリアルの変更
        if (isEnd) ChangeTutorialState();
    }

    void TutorialStart()
    {

    }

    void TutorialMove()
    {

    }

    void TutorialRotation()
    {

    }

    void TutorialWeapon()
    {

    }

    void TutorialXray()
    {

    }

    void TutorialShooting()
    {

    }

    void TutorialEnd()
    {

    }

    /// <summary> チュートリアルシーンの変更 </summary>
    void ChangeTutorialState()
    {
        oldState = state;
        isEnd = true;
        state++;
    }

    public TutorialState GetTutorialState()
    {
        return state;
    }
}
