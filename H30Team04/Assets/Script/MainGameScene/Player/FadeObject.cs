using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary> 色のタイプ </summary>
public enum ColorType
{
    RBG, Black, White, Red
}

public class FadeObject : MonoBehaviour
{
    //色の変数
    float[] RBG;

    //ステートパターンで色のタイプを格納
    public Dictionary<ColorType, float[]> color
    = new Dictionary<ColorType, float[]>();

    //色のタイプの変更
    ColorType type = ColorType.RBG;

    //タイプごとの色
    float[] rbg = new float[3];
    float[] black = new float[3] { 1, 1, 1 };
    float[] white = new float[3] { 0, 0, 0 };
    float[] red = new float[3] { 1, 0, 0 };

    //兵士の情報取得用
    [SerializeField] GameObject soldier;
    SoldierMove soldierMove;

    //フェード速度
    [SerializeField, Range(0.01f, 1)] float alphaSpeed = 0.01f;
    //最小透明度
    [SerializeField, Range(0, 1)] float limitAlfa = 1;

    //透明度
    float alfa;
    //色彩情報
    float R, G, B;

    //フェードフラグ
    bool isFade;



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
        color.Add(ColorType.RBG, rbg);
        color.Add(ColorType.Black, black);
        color.Add(ColorType.White, white);
        color.Add(ColorType.Red, red);
    }

    void Update()
    {
        Color fadeColor =
            new Color(color[type][0], color[type][1], color[type][2], alfa);

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
        type = ColorType.RBG;

        R = r;
        B = b;
        G = g;
    }

    public void SetFadeColor(ColorType color)
    {
        R = this.color[color][0];
        G = this.color[color][1];
        B = this.color[color][2];
    }
}
