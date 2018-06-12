using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary> 色のタイプ </summary>
public enum ColorType
{
    RBG, Black, White, Red
}

/// <summary> フェード機能 </summary>
public class FadeObject : MonoBehaviour
{
    RectTransform rect;

    //色の変数
    float[] RBG;

    //ステートパターンで色のタイプを格納
    public Dictionary<ColorType, float[]>
        color = new Dictionary<ColorType, float[]>();

    //色のタイプの変更
    ColorType type = ColorType.RBG;

    //タイプごとの色
    float[] rbg = new float[3];
    float[] black = new float[3] { 1, 1, 1 };
    float[] white = new float[3] { 0, 0, 0 };
    float[] red = new float[3] { 1, 0, 0 };

    //フェード速度
    [SerializeField, Range(0.01f, 1)] float alphaSpeed = 0.01f;
    //最小透明度
    [SerializeField, Range(0, 1)] float limitAlpha = 1;

    //透明度
    float alfa;
    //色彩情報
    float R, G, B;

    //フェードフラグ
    public bool IsFade { get; set; }

    void Start()
    {
        rect = GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(Screen.width, Screen.height);

        //色彩情報の取得
        R = GetComponent<Image>().color.r;
        G = GetComponent<Image>().color.g;
        B = GetComponent<Image>().color.b;

        //色のタイプの登録
        color.Add(ColorType.RBG, rbg);
        color.Add(ColorType.Black, black);
        color.Add(ColorType.White, white);
        color.Add(ColorType.Red, red);
    }

    void Update()
    {
        if (Time.timeScale == 0) return;
        //自由色の登録・更新
        rbg = new float[3] { R, B, G };

        //彩色
        Color fadeColor =
            new Color(color[type][0], color[type][1], color[type][2], alfa);

        //色情報
        GetComponent<Image>().color = fadeColor;

        //透明度の上限調整
        AlfaRange();
    }

    /// <summary> 暗転 </summary>
    public void FadeOut()
    {
        IsFade = false;
        alfa += alphaSpeed;
    }

    /// <summary> 暗転の逆 </summary>
    public void FadeIn()
    {
        IsFade = true;
        if (!IsFade) return;
        alfa -= alphaSpeed;
    }

    /// <summary> フェードアウトからのフェードイン </summary>
    public void AutoFade(float time = 1)
    {
        var stayTime = time;

        FadeOut();

        if (alfa >= limitAlpha) stayTime -= Time.deltaTime;

        if (stayTime <= 0) FadeIn();
    }

    /// <summary> 最大限フェードアウトしたら true </summary>
    public bool IsMaxAlpha()
    {
        return (alfa >= limitAlpha);
    }

    /// <summary> alphaが0なら true </summary>
    public bool IsMinAlpha()
    {
        return (alfa <= 0);
    }

    /// <summary> 透明度 </summary>
    public float GetAlpha()
    {
        return alfa;
    }

    /// <summary> フェード速度の設定 </summary>
    public void SetFadeSpeed(float value)
    {
        alphaSpeed = value;
    }

    /// <summary> 透明度の上限調整 </summary>=
    void AlfaRange()
    {
        if (alfa >= limitAlpha) alfa = limitAlpha;
        if (alfa <= 0) alfa = 0;
    }

    /// <summary> 最小透明度の設定 </summary>
    public void SetMaxAlpha(float alpha)
    {
        limitAlpha = alpha;
    }

    /// <summary> フェードさせる色の設定（RBG） </summary>
    public void SetFadeColor(float r, float b, float g)
    {
        type = ColorType.RBG;

        R = r;
        B = b;
        G = g;
    }

    /// <summary> フェードさせる色のタイプの設定 </summary>
    public void SetFadeColor(ColorType color)
    {
        R = this.color[color][0];
        G = this.color[color][1];
        B = this.color[color][2];
    }
}
