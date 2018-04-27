﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeekPoint : MonoBehaviour {

    public int weeknumber = 0;

    private GameObject m_model;
    private float hideTime = 1.0f;
    private float m_time = 0.0f;

	// Use this for initialization 
	void Start () {
        m_model = transform.Find("model").gameObject;
    }
	
	// Update is called once per frame
	void Update () {

        //モデルが非表示の場合、指定時間過ぎたらモデル表示
        if (!m_model.activeSelf)
        {
            m_time += Time.deltaTime;
            if(m_time > hideTime)
            {
                m_model.SetActive(true);
            }
        }
	}

    /// <summary>射影機からのレイが通らない弱点は非表示</summary>
    public void HideObject()
    {
        //Debug.Log("やるじゃない");
        m_model.SetActive(false);
        m_time = 0.0f;
    }

    /// <summary>弱点番号(頭なら０とか)を返す関数</summary>
    //public int GetWeekNumber()
    //{
    //    return weeknumber;
    //}

    public int GetWeekNumber
    {
        get { return weeknumber; }
    }
}
