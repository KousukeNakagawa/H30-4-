using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour {
    private static float alpha = 1.0f;
    private static bool m_Fade = false;
    private static bool fadeEnd = false;
    private static float fadeSpeed = 2.0f;

    private static Color fadeColor = new Color(0, 0, 0, 0);

    // Use this for initialization
    void Start()
    {
        alpha = 1.0f;
        m_Fade = false;
        fadeEnd = false;
        fadeSpeed = 2.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (alpha > 0.0f && !m_Fade)
        {
            alpha -= Time.deltaTime / fadeSpeed;
            if (alpha <= 0)
            {
                alpha = 0.0f;
                fadeEnd = true;
            }
            GetComponent<Image>().color = fadeColor + new Color(0, 0, 0, alpha);
            return;
        }
        if (m_Fade && alpha < 1.0f)
        {
            alpha += Time.deltaTime / fadeSpeed;
            if (alpha >= 1)
            {
                alpha = 1.0f;
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
}
