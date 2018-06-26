using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialXlinePhoto : MonoBehaviour {
    
    [SerializeField]
    private Image[] m_wepons;
    [SerializeField]
    TutorialAttackPlayer m_AP;
    [SerializeField]
    private Transform[] m_imagepos;
    [SerializeField]
    private TutorialMiniMap m_miniMapCamera;
    GameObject XPhots;
    GameObject m_Sight;
    GameObject m_textBackImage;
    GameObject m_Rod;

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

    public bool test = false;
    [SerializeField]
    Text m_text;

    public TutorialManager_T tmane;

    
    private int weeknumber = 0;

    // Use this for initialization
    void Start()
    {
        keyflag_ = false;
        currentSelectStageIndex = 0;
        m_FlyerCount = 0;
        XPhots = transform.Find("XPhotos").gameObject;
        xrayDatas = new List<GameManager.WeekPointData>();
        weektextparent = transform.Find("Probabilitys").gameObject;
        weektexts = weektextparent.GetComponent<WeekTextManager>();
        m_Sight = transform.Find("attackUI").transform.Find("sight").gameObject;
        m_textBackImage = transform.Find("attackUI").transform.Find("Image").gameObject;
        m_Rod = transform.Find("attackUI").transform.Find("Rod").gameObject;
        m_Sight.SetActive(false);
        m_textBackImage.SetActive(false);
        m_Rod.SetActive(false);

        List<int> aaaa = new List<int> { 0,1,2,3,4,5};
        SetWeekPhoto("03", aaaa);
        SetWeekPhoto("04", aaaa);
        SetWeekPhoto("05", aaaa);
        SetWeekPhoto("06", aaaa);
        XPhots.SetActive(false);
        weektextparent.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        UpdateSelect();
        WeponChoiceNow();
    }

    void WeponChoiceNow()
    {
        if (tmane.GetState() < TutorialState_T.SHOTEFFECT)
        {
            m_wepons[0].enabled = true;
            m_wepons[1].enabled = true;
            m_wepons[2].enabled = true;
            if (!WeaponCtrl.WeaponBeacon)
            {
                m_wepons[0].transform.position = m_imagepos[0].position;
                m_wepons[1].transform.position = m_imagepos[1].position;
                m_wepons[0].GetComponent<RectTransform>().sizeDelta = new Vector2(150, 50);
                m_wepons[1].GetComponent<RectTransform>().sizeDelta = new Vector2(100, 40);
                m_wepons[1].transform.SetAsFirstSibling();
            }
            else
            {
                m_wepons[1].transform.position = m_imagepos[0].position;
                m_wepons[0].transform.position = m_imagepos[1].position;
                m_wepons[1].GetComponent<RectTransform>().sizeDelta = new Vector2(150, 50);
                m_wepons[0].GetComponent<RectTransform>().sizeDelta = new Vector2(100, 40);
                m_wepons[0].transform.SetAsFirstSibling();
            }
        }
        else
        {
            m_wepons[0].enabled = false;
            m_wepons[1].enabled = false;
            m_wepons[2].enabled = false;
        }
    }

    void UpdateSelect()
    {
        if (tmane.GetState() == TutorialState_T.XRAY)
        {
            m_Rod.SetActive(false);
            m_Sight.SetActive(false);
            m_textBackImage.SetActive(false);
            m_text.text = " ";
            XPhots.SetActive(true);
            weektextparent.SetActive(true);
            if (m_FlyerCount == 0) return;
            const float Margin = 0.5f;
            if (!tmane.IsReaded()) return;
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
        else if (tmane.GetState() == TutorialState_T.SHOT)
        {
            if (tmane.Shot)
            {

                XPhots.SetActive(false);
                weektextparent.SetActive(false);
                m_Sight.SetActive(false);
                m_textBackImage.SetActive(false);
                m_Rod.SetActive(false);
                m_text.text = " ";
            }
            else
            {
                m_text.text = m_AP.WeekName;
                XPhots.SetActive(false);
                weektextparent.SetActive(false);
                m_Sight.SetActive(true);
                m_Rod.SetActive(true);
                m_textBackImage.SetActive(true);
            }
            
        }
    }

    public void UIdelete()
    {
        m_Sight.SetActive(false);
    }
    public void PhotoCreate(GameManager.WeekPointData data)
    {
        xrayDatas.Add(data);
        GameObject photo = Instantiate(photoPrefab, XPhots.transform);
        //photo.transform.parent = XPhots.transform;
        photo.GetComponent<RectTransform>().anchoredPosition = new Vector2(1280 * m_FlyerCount, 0);
        photo.transform.Find("Photo").GetComponent<RawImage>().texture = Resources.Load("Texture/RenderTextures/XrayCamera" + data.name) as RenderTexture;
        if (m_FlyerCount == 0) weektexts.SetTexts(data.datas);
        m_FlyerCount++;
    }


    public void SetWeekPhoto(string texname, List<int> weeknums)
    {
        GameManager.WeekPointData result;
        result.name = texname;
        List<GameManager.WeekPointProbability> list = new List<GameManager.WeekPointProbability>();
        List<int> probabilitys = XrayProbability(weeknums);
        for (int i = 0; i < weeknums.Count; i++)
        {
            GameManager.WeekPointProbability a;
            a.num = weeknums[i];
            a.probability = probabilitys[i];
            Debug.Log("弱点「" + a.num + "」の確立は" + a.probability + "％");
            list.Add(a);
        }
        result.datas = list;

        PhotoCreate(result);
    }

    //弱点の確立の割り振り
    private List<int> XrayProbability(List<int> weeknums)
    {
        List<int> result = new List<int>();
        int count = 100;  //合計で100%になるように
        for (int i = 0; i < weeknums.Count; i++)
        {
            int probability = 0;
            if (weeknums[i] == weeknumber)
            {
                probability = 10;  //弱点部位にボーナス分％をプラス
                count -= 10; //そのぶん回す数減らす
            }
            result.Add(probability);
        }

        while (count > 0) //合計100%になるまで
        {
            count--;
            int plus = Random.Range(0, weeknums.Count);
            result[plus]++;
            //Debug.Log("今日は" + weeknums[plus]);
        }

        return result;
    }
}
