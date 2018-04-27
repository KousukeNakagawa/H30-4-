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

    public GameObject photoPrefab;
    private List<GameManager.WeekPointData> xrayDatas;
    private GameObject weektextparent;
    private WeekTextManager weektexts;

    // Use this for initialization
    void Start () {
        m_gamemanager = gm.GetComponent<GameManager>();
        keyflag_ = false;
        currentSelectStageIndex = 0;
        m_FlyerCount = 0;
        XPhots = transform.Find("XPhotos").gameObject;
        xrayDatas = new List<GameManager.WeekPointData>();
        weektextparent = transform.Find("Probabilitys").gameObject;
        weektexts = weektextparent.GetComponent<WeekTextManager>();
        //for (int i = 0; i < XPhots.transform.childCount; ++i)
        //{
        //    if (XPhots.transform.GetChild(i).name.Split('_')[0] == "XPhoto")
        //    {
        //        ++m_FlyerCount;
        //    }
        //}
        m_Sight = transform.Find("sight").gameObject;
        XPhots.SetActive(false);
        m_Sight.SetActive(false);
        weektextparent.SetActive(false);


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
            weektextparent.SetActive(true);
            if (m_FlyerCount == 0) return;
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
                weektexts.AllQuestion();
            }
            else if (Mathf.Abs(inputHorizontal) <= Margin && m_IsChange)
            {
                m_IsChange = false;
                weektexts.SetTexts(xrayDatas[currentSelectStageIndex].datas);
            }
        }
        else if (m_gamemanager.AttackStateNow())
        {
            XPhots.SetActive(false);
            weektextparent.SetActive(false);
            m_Sight.SetActive(true);
        }
        else
        {
            if(m_gamemanager.GetWeekPointData.Count > m_FlyerCount)
            {
                PhotoCreate(m_gamemanager.GetWeekPointData[m_FlyerCount]);
            }
        }
    }

    void LoadPhotos()
    {

    }

    public void ViewPhotos()
    {

    }

    public void PhotoCreate(GameManager.WeekPointData data)
    {
        xrayDatas.Add(data);
        GameObject photo = Instantiate(photoPrefab);
        photo.transform.parent = XPhots.transform;
        photo.GetComponent<RectTransform>().anchoredPosition = new Vector2(1280 * m_FlyerCount, 0);
        photo.transform.Find("Photo").GetComponent<RawImage>().texture = Resources.Load("Texture/RenderTextures/XrayCamera" + data.name) as RenderTexture;
        if (m_FlyerCount == 0) weektexts.SetTexts(data.datas);
         m_FlyerCount++;
    }
}
