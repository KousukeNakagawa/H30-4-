using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    private static float alpha = 1.0f;
    private static bool m_Fade = false; //trueはフェードアウト中
    private static bool fadeEnd = false; //フェードしているか
    private static float fadeSpeed = 2.0f;

    private static Color fadeColor = new Color(0, 0, 0, 0);  //フェードの色
    private static float fadeAlphaLimit = 1.0f;  //フェードアウトα値の限界、どのくらいフェードアウトするか

    // Use this for initialization
    void Start()
    {
        alpha = 1.0f;
        m_Fade = false;
        fadeEnd = false;
        fadeSpeed = 2.0f;
        fadeColor = new Color(0, 0, 0, 0);
        fadeAlphaLimit = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (alpha > 0.0f && !m_Fade) //フェードイン
        {
            alpha -= Time.deltaTime / fadeSpeed * fadeAlphaLimit;
            if (alpha <= 0)
            {
                alpha = 0.0f;
                fadeAlphaLimit = 1.0f;
                fadeEnd = true;
            }
            GetComponent<Image>().color = fadeColor + new Color(0, 0, 0, alpha);
            return;
        }
        else if (m_Fade && alpha < fadeAlphaLimit) //フェードアウト
        {
            alpha += Time.deltaTime / fadeSpeed * fadeAlphaLimit;
            if (alpha >= fadeAlphaLimit)
            {
                alpha = fadeAlphaLimit;
                fadeEnd = true;
            }
            GetComponent<Image>().color = fadeColor + new Color(0, 0, 0, alpha);
        }
    }

    public static void FadeOut(float fadespeed = 2.0f)
    {
        if (!fadeEnd || m_Fade) return;
        m_Fade = true;
        fadeEnd = false;
        fadeSpeed = fadespeed;
    }

    public static void FadeIn(float fadespeed = 2.0f)
    {
        if (!fadeEnd || !m_Fade) return;
        m_Fade = false;
        fadeEnd = false;
        fadeSpeed = fadespeed;
    }

    public static bool IsFadeEnd()
    {
        return fadeEnd;
    }

    /// <summary>フェードイン、フェードアウトどっちしてますか,trueはフェードアウト中</summary>
    public static bool IsFadeOutOrIn()
    {
        return m_Fade;
    }

    /// <summary>フェードの色変更</summary>
    /// <param name="col">変えたい色</param>
    public static void ColorChenge(Color col)
    {
        fadeColor = col;
        fadeColor.a = 0;
    }

    public static void ChengeAlphaLimit(float a)
    {
        fadeAlphaLimit = a;
    }
}
