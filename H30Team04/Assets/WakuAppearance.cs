using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WakuAppearance : MonoBehaviour {

    [SerializeField]
    private GameObject[] m_WeakProbilitys;
    [SerializeField]
    float m_rightmove1, m_leftmove1, m_rightmove2, m_leftmove2;

    int m_WeakProbNum;

	// Use this for initialization
	void Start () {
        //for (int i = 0; i < m_WeakProbilitys.Length; i++) {
        //    m_WeakProbilitys[i].SetActive(false);
        //}
	}
	
	// Update is called once per frame
	void Update () {

	}

    public void WakuApper()
    {
        //for (int i = 0; i < m_WeakProbilitys.Length; i++)
        //{
        //    m_WeakProbilitys[i].SetActive(true);
        //    m_WeakProbilitys[i].transform.Find("leftWaku").gameObject.SetActive(true);
        //    m_WeakProbilitys[i].transform.Find("rightWaku").gameObject.SetActive(true);

        //    iTween.MoveTo(m_WeakProbilitys[i].transform.Find("leftWaku").gameObject, 
        //        iTween.Hash("x", m_rightmove1,
        //                    "time", 2,
        //                    "oncomplete", "LeftMove",
        //                    "oncompletetarget", gameObject));
        //}
    }

    //private void LeftMove()
    //{
    //    for (int i = 0; i < m_WeakProbilitys.Length; i++)
    //    {
    //        iTween.MoveTo(m_WeakProbilitys[i].transform.Find("leftWaku").gameObject,
    //            iTween.Hash("x", m_leftmove1,
    //                        "time", 3,
    //                        "oncomplete", "ScaleTo",
    //                        "oncompletetarget", gameObject));
    //    }
    //}

    //private void ScaleTo()
    //{
    //    for (int i = 0; i < m_WeakProbilitys.Length; i++)
    //    {
    //        iTween.ScaleTo(m_WeakProbilitys[i].transform.Find("leftWaku").gameObject,
    //            iTween.Hash("x", 0.2,"y",1.2,
    //                        "time", 4));
    //    }
    //}
}
