using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary> フェードカラー </summary>=
public enum FadeColor
{
    RBG, Black, White, Red
}

public class FadeObject : MonoBehaviour
{
    //デリゲート
    float[] RBG;

    //ステートパターンのDictionary
    public Dictionary<FadeColor, float[]> color
    = new Dictionary<FadeColor, float[]>();

    //色の状態変更
    FadeColor colorState = FadeColor.RBG;

    //兵士の情報取得用
    [SerializeField] GameObject soldier;
    SoldierMove soldierMove;

    //フェード情報
    [SerializeField, Range(0.01f, 1)] float alphaSpeed = 0.01f;
    [SerializeField, Range(0, 1)] float limitAlfa = 1;

    //透明度
    float alfa;
    //色彩情報
    float R, G, B;

    //フェードフラグ
    bool isFade;

    float[] rbg = new float[3];
    float[] black = new float[3] { 1, 1, 1 };
    float[] white = new float[3] { 0, 0, 0 };
    float[] red = new float[3] { 1, 0, 0 };

    void Start()
    {
        //兵士の情報の取得
        soldierMove = soldier.GetComponent<SoldierMove>();

        //色彩情報
        R = GetComponent<Image>().color.r;
        G = GetComponent<Image>().color.g;
        B = GetComponent<Image>().color.b;

        rbg = new float[3] { R, B, G };

        //フェードカラー
        color.Add(FadeColor.RBG, rbg);
        color.Add(FadeColor.Black, black);
        color.Add(FadeColor.White, white);
        color.Add(FadeColor.Red, red);
    }

    void Update()
    {
        Color fadeColor =
            new Color(color[colorState][0], color[colorState][1], color[colorState][2], alfa);

        GetComponent<Image>().color = fadeColor;

        //透明度の上限調整
        AlfaRange();

        //フェード処理
        if (soldierMove.IsDead()) FadeOut();
        else FadeIn();

        if (Input.GetButtonDown("Select")) isFade = true;
    }

    /// <summary> 透明度の上限調整 </summary>=
    void AlfaRange()
    {
        if (alfa >= limitAlfa) alfa = limitAlfa;
        if (alfa <= 0) alfa = 0;
    }

    /// <summary> 暗転 </summary>
    public void FadeOut()
    {
        isFade = false;
        alfa += alphaSpeed;
    }

    /// <summary> 暗転の逆 </summary>
    public void FadeIn()
    {
        if (!isFade) return;
        alfa -= alphaSpeed;
    }

    /// <summary> フェード速度の設定 </summary>
    public void SetFadeSpeed(float value)
    {
        alphaSpeed = value;
    }

    /// <summary> 最大透明度の設定 </summary>
    public void SetMaxAlpha(float alpha)
    {
        limitAlfa = alpha;
    }

    /// <summary> フェードさせる色の設定（RBG） </summary>
    public void SetFadeColor(float r, float b, float g)
    {
        colorState = FadeColor.RBG;

        R = r;
        B = b;
        G = g;
    }

    public void SetFadeColor(FadeColor color)
    {
        R = this.color[color][0];
        G = this.color[color][1];
        B = this.color[color][2];
    }
}
