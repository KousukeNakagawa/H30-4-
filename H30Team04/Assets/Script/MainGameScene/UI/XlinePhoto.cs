using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class XlinePhoto : MonoBehaviour {
    public GameObject gm;
    GameObject XPhots;
    GameObject m_Sight;
    GameManager m_gamemanager;
    //GameObject Arrows;
    public int m_FlyerCount;
    bool m_IsChange;
    private int currentSelectStageIndex;
    AsyncOperation async;
    bool keyflag_;

    GameObject work_image;

    // Use this for initialization
    void Start () {
        m_gamemanager = gm.GetComponent<GameManager>();
        keyflag_ = false;
        currentSelectStageIndex = 0;
        m_FlyerCount = 0;
        XPhots = transform.Find("XPhotos").gameObject;
        for (int i = 0; i < XPhots.transform.childCount; ++i)
        {
            if (XPhots.transform.GetChild(i).name.Split('_')[0] == "XPhoto")
            {
                ++m_FlyerCount;
            }
        }
        m_Sight = transform.Find("sight").gameObject;
        XPhots.SetActive(false);
        m_Sight.SetActive(false);
           
    }
	
	// Update is called once per frame
	void Update () {
        UpdateSelect();
        ViewPhotos();
	}
    void UpdateSelect()
    {
        if (m_gamemanager.PhotoCheckStateNow())
        {
            XPhots.SetActive(true);
            const float Margin = 0.5f;
            float inputHorizontal = /*(Input.GetAxisRaw("XboxLeftHorizontal") != 0) ? Input.GetAxisRaw("XboxLeftHorizontal") :*/ Input.GetAxisRaw("Horizontal");
            if (Mathf.Abs(inputHorizontal) > Margin && !m_IsChange)
            {
                if (inputHorizontal > 0.0f)
                {
                    currentSelectStageIndex += 1;
                }
                else
                {
                    currentSelectStageIndex += (m_FlyerCount - 1);
                }
                currentSelectStageIndex = currentSelectStageIndex % m_FlyerCount;
                float l_positionX = currentSelectStageIndex * 1280;
                XPhots.GetComponent<XPhotos>().MoveTargetPositionX(-l_positionX);
                m_IsChange = true;
            }
            else if (Mathf.Abs(inputHorizontal) <= Margin)
            {
                m_IsChange = false;
            }
        }
        else if (m_gamemanager.AttackStateNow())
        {
            XPhots.SetActive(false);
            m_Sight.SetActive(true);
        }
    }

    void LoadPhotos()
    {

    }

    public void ViewPhotos()
    {

    }
}
