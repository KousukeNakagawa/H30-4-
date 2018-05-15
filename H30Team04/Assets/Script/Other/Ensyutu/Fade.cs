using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour {
    private float alpha = 1.0f;
    private bool m_Fade = false;
    private bool fadeEnd = false;
    private float fadeSpeed = 2.0f;

    // Use this for initialization
    void Start()
    {

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
            GetComponent<Image>().color = new Color(0, 0, 0, alpha);
            return;
        }
        if (m_Fade && alpha < 1.0f)
        {
            GetComponent<Image>().color = new Color(0, 0, 0, alpha);
            alpha += Time.deltaTime / fadeSpeed;
            if (alpha >= 1)
            {
                alpha = 1.0f;
                fadeEnd = true;
            }
        }
    }

    public void FadeOut(float fadespeed = 2.0f)
    {
        if (!fadeEnd || m_Fade) return;
        m_Fade = true;
        fadeEnd = false;
        fadeSpeed = fadespeed;
    }

    public void FadeIn(float fadespeed = 2.0f)
    {
        if (!fadeEnd || !m_Fade) return;
        m_Fade = false;
        fadeEnd = false;
        fadeSpeed = fadespeed;
    }

    public bool IsFadeEnd()
    {
        return fadeEnd;
    }

    /// <summary>フェードイン、フェードアウトどっちしてますか</summary>
    /// <returns>trueでフェードアウト中</returns>
    public bool IsFadeOutOrIn()
    {
        return m_Fade;
    }
}
