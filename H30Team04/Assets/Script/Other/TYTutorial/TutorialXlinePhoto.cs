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
    GameObject XPhotsBack;
    GameObject XPhotsFront;
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

    [SerializeField]
    Text m_par;

    public TutorialManager_T tmane;

    private List<GameObject> photos;

    private int weeknumber = 0;

    public bool IsFilmEnd { get;  set; }

    // Use this for initialization
    void Start()
    {
        IsFilmEnd = false;
        keyflag_ = false;
        currentSelectStageIndex = 0;
        m_FlyerCount = 0;
        photos = new List<GameObject>();
        XPhots = transform.Find("XPhotos").gameObject;
        XPhotsBack = transform.Find("XPhotosBack").gameObject;
        XPhotsFront = transform.Find("XPhotosFront").gameObject;
        xrayDatas = new List<GameManager.WeekPointData>();
        weektextparent = transform.Find("Probabilitys").gameObject;
        weektexts = weektextparent.GetComponent<WeekTextManager>();
        m_Sight = transform.Find("attackUI").transform.Find("sight").gameObject;
        m_textBackImage = transform.Find("attackUI").transform.Find("Image").gameObject;
        m_Rod = transform.Find("attackUI").transform.Find("Rod").gameObject;
        m_Sight.SetActive(false);
        m_textBackImage.SetActive(false);
        m_Rod.SetActive(false);
        XPhotsBack.SetActive(false);
        XPhotsFront.SetActive(false);

        List<int> aaaa = new List<int> { 1,3,4,5};
        List<int> iiii = new List<int> { 0, 1,  3, 4 };
        List<int> uuuu = new List<int> { 0 };
        List<int> eeee = new List<int> { 0,  2, 3,  5 };
        SetWeekPhoto("03", aaaa);
        SetWeekPhoto("04", iiii);
        SetWeekPhoto("05", uuuu);
        SetWeekPhoto("06", eeee);
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
        if (tmane.GetState() == TutorialState_T.XRAYEFFECT)
        {
            PhotoCheckUpdate();
        }
        else if (tmane.GetState() == TutorialState_T.SHOT)
        {
            if (tmane.Shot)
            {

                XPhots.SetActive(false);
                XPhotsBack.SetActive(false);
                XPhotsFront.SetActive(false);
                weektextparent.SetActive(false);
                m_Sight.SetActive(false);
                m_textBackImage.SetActive(false);
                m_Rod.SetActive(false);
                m_text.text = " ";
                m_par.text = " ";
            }
            else
            {
                m_text.text = m_AP.WeekName;
                m_par.text = m_AP.WeekPar + "%";
                XPhots.SetActive(false);
                XPhotsBack.SetActive(false);
                XPhotsFront.SetActive(false);
                weektextparent.SetActive(false);
                m_Sight.SetActive(true);
                m_Rod.SetActive(true);
                m_textBackImage.SetActive(true);
            }
            
        }
    }

    private void PhotoCheckUpdate()
    {
        m_Rod.SetActive(false);
        m_Sight.SetActive(false);
        m_textBackImage.SetActive(false);
        XPhotsFront.SetActive(true);
        m_text.text = " ";
        if (m_FlyerCount == 0)
        {
            //一個も取れてないぞいのテキスト
            //m_gamemanager.ChengeShot();
            return;
        }

        XPhots.SetActive(true);
        XPhotsBack.SetActive(true);
        weektextparent.SetActive(true);
        //const float Margin = 0.5f;
        //float inputHorizontal = /*(Input.GetAxisRaw("XboxLeftHorizontal") != 0) ? Input.GetAxisRaw("XboxLeftHorizontal") :*/ Input.GetAxisRaw("Horizontal");
        //if (!m_IsChange)
        //{
        if (currentSelectStageIndex <= m_FlyerCount)
        {
            RectTransform photoTrans = (currentSelectStageIndex == m_FlyerCount) ? XPhotsFront.GetComponent<RectTransform>() : photos[currentSelectStageIndex].GetComponent<RectTransform>();
            photoTrans.localPosition -= Vector3.up * 250 * Time.deltaTime;
            //-Vector3.up * Mathf.Lerp(photoTrans.localPosition.y, Vector3.zero.y, 0.5f);

            if (/*Mathf.Abs(*/photoTrans.localPosition.y/*)*/ < 1)
            {
                photoTrans.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                if (currentSelectStageIndex < m_FlyerCount) weektexts.SetTexts(xrayDatas[currentSelectStageIndex].datas);
                currentSelectStageIndex++;
            }
        }
        else
        {
            IsFilmEnd = true;
            //if (Input.anyKeyDown && !Input.GetButtonDown("Shutter"))
            //{
            //    GameTextController.TextStart(9);
            //    //m_gamemanager.ChengeShot();
            //}
        }
        //currentSelectStageIndex = currentSelectStageIndex % m_FlyerCount;
        //float l_positionX = currentSelectStageIndex * 1280;
        //XPhots.GetComponent<XPhotos>().MoveTargetPositionX(-l_positionX);
        //m_IsChange = true;
        //}
        //else
        //{
        //    m_IsChange = false;
        //    weektexts.SetTexts(xrayDatas[currentSelectStageIndex].datas);
        //}
    }

    public void RestFilm()
    {
        IsFilmEnd = false;
        XPhotsFront.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 500);
        for(int i = 0; i < photos.Count; i++)
        {
            photos[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 500);
        }
        currentSelectStageIndex = 0;
        weektexts.ResetText();
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
        photo.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 500);
        //photo.GetComponent<RectTransform>().anchoredPosition = new Vector2(1280 * m_FlyerCount, 0);
        photo.transform.Find("Photo").GetComponent<RawImage>().texture = Resources.Load("Texture/RenderTextures/XrayCamera" + data.name) as RenderTexture;
        //if (m_FlyerCount == 0) weektexts.SetTexts(data.datas);
        m_FlyerCount++;
        photos.Add(photo);
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
