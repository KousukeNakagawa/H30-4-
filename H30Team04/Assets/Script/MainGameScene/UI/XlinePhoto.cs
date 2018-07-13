using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class XlinePhoto : MonoBehaviour {
    [SerializeField]
    private GameObject[] m_lifes;
    [SerializeField]
    private Image[] m_wepons;
    [SerializeField]
    AttackPlayer m_AP;
    [SerializeField]
    private Transform[] m_imagepos;
    [SerializeField]
    private MiniMAPcamera m_miniMapCamera;
    public GameObject gm;
    GameObject XPhots;
    GameObject XPhotsBack;
    GameObject XPhotsFront;
    GameObject m_Sight;
    GameObject m_textBackImage;
    GameObject m_Rod;
    GameManager m_gamemanager;
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
    public int _lifecount = 3;   //残機

    public bool test = false;
    [SerializeField]
    Text m_text;
    [SerializeField]
    Text m_par;

    private List<GameObject> photos;

    // Use this for initialization
    void Start()
    {
        m_gamemanager = gm.GetComponent<GameManager>();
        keyflag_ = false;
        currentSelectStageIndex = 0;
        m_FlyerCount = 0;
        XPhots = transform.Find("XPhotos").gameObject;
        XPhotsBack = transform.Find("XPhotosBack").gameObject;
        XPhotsFront = transform.Find("XPhotosFront").gameObject;
        xrayDatas = new List<GameManager.WeekPointData>();
        photos = new List<GameObject>();
        weektextparent = transform.Find("Probabilitys").gameObject;
        weektexts = weektextparent.GetComponent<WeekTextManager>();
        m_Sight = transform.Find("attackUI").transform.Find("sight").gameObject;
        m_textBackImage = transform.Find("attackUI").transform.Find("Image").gameObject;
        m_Rod = transform.Find("attackUI").transform.Find("Rod").gameObject;
        XPhots.SetActive(false);
        XPhotsBack.SetActive(false);
        XPhotsFront.SetActive(false);
        m_Sight.SetActive(false);
        m_textBackImage.SetActive(false);
        m_Rod.SetActive(false);
        weektextparent.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSelect();
        WeponChoiceNow();
        if (m_miniMapCamera._pose || !m_gamemanager.PhotoStateNow())
        {
            m_wepons[0].enabled = false;
            m_wepons[1].enabled = false;
            m_wepons[2].enabled = false;
            m_lifes[0].transform.parent.gameObject.SetActive(false);
        }
        else
        {
            m_lifes[0].transform.parent.gameObject.SetActive(true);
        }
    }

    void WeponChoiceNow()
    {
        if (m_gamemanager.PhotoStateNow())
        {
            m_wepons[0].enabled = true;
            m_wepons[1].enabled = true;
            m_wepons[2].enabled = true;
            if (!WeaponCtrl.IsWeaponBeacon)
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
        if (m_gamemanager.PhotoCheckStateNow())
        {
            PhotoCheckUpdate();
            //m_Rod.SetActive(false);
            //m_Sight.SetActive(false);
            //m_textBackImage.SetActive(false);
            //m_text.text = " ";
            //XPhots.SetActive(true);
            //weektextparent.SetActive(true);
            //if (m_FlyerCount == 0) return;
            //const float Margin = 0.5f;
            //float inputHorizontal = /*(Input.GetAxisRaw("XboxLeftHorizontal") != 0) ? Input.GetAxisRaw("XboxLeftHorizontal") :*/ Input.GetAxisRaw("Horizontal");
            //if (Mathf.Abs(inputHorizontal) > Margin && !m_IsChange)
            //{
            //    if (inputHorizontal > 0.0f)
            //    {
            //        currentSelectStageIndex += 1;
            //    }
            //    else
            //    {
            //        currentSelectStageIndex += (m_FlyerCount - 1);
            //    }
            //    currentSelectStageIndex = currentSelectStageIndex % m_FlyerCount;
            //    float l_positionX = currentSelectStageIndex * 1280;
            //    XPhots.GetComponent<XPhotos>().MoveTargetPositionX(-l_positionX);
            //    m_IsChange = true;
            //    //weektexts.AllQuestion();
            //}
            //else if (Mathf.Abs(inputHorizontal) <= Margin && m_IsChange)
            //{
            //    m_IsChange = false;
            //    weektexts.SetTexts(xrayDatas[currentSelectStageIndex].datas);
            //}
        }
        else if (m_gamemanager.AttackStateNow())
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
        else if (m_gamemanager.NowState() == GameManager.PhaseState.waitingState)
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
            if (m_gamemanager.GetWeekPointData.Count > m_FlyerCount)
            {
                PhotoCreate(m_gamemanager.GetWeekPointData[m_FlyerCount]);
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
            GameTextController.TextStart(14);
            m_gamemanager.ChengeShot();
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
            RectTransform photoTrans = (currentSelectStageIndex == m_FlyerCount)? XPhotsFront.GetComponent<RectTransform>() : photos[currentSelectStageIndex].GetComponent<RectTransform>();
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
            if (Input.anyKeyDown && !Input.GetButtonDown("Shutter"))
            {
                GameTextController.TextStart(9);
                m_gamemanager.ChengeShot();
            }
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

    public void PhotoCreate(GameManager.WeekPointData data)
    {
        xrayDatas.Add(data);
        GameObject photo = Instantiate(photoPrefab, XPhots.transform);
        //photo.transform.parent = XPhots.transform;
        //photo.GetComponent<RectTransform>().anchoredPosition = new Vector2(1280 * m_FlyerCount, 0);
        photo.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 500);
        photo.transform.Find("Photo").GetComponent<RawImage>().texture = Resources.Load("Texture/RenderTextures/XrayCamera" + data.name) as RenderTexture;
        //if (m_FlyerCount == 0) weektexts.SetTexts(data.datas);
        m_FlyerCount++;
        photos.Add(photo);
    }


    public void UIdelete()
    {
        m_Sight.SetActive(false);
    }
}
