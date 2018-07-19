using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenShotMove : MonoBehaviour {

    enum ScShoState
    {
        CHANGE,
        SLIDEWAIT,
        SLIDE
    }

    private ScShoState m_State = ScShoState.CHANGE;

    private RectTransform m_RTrans;

    private RectTransform movedRTrans;
    private RectTransform slidedRTrans;

    private Vector3 startPos;
    private Vector3 startSize;
    private float startTime;
    private float kyori;

    [SerializeField] private float lerpSpeed = 10.0f;
    [SerializeField] private float slideLerpSpeed = 10.0f;
    [SerializeField] private float slideWaitTime = 5.0f;

    private string photoNum;
    private bool isHit; //写真が取れているか

    [SerializeField] private GameObject photoPrefab;

    // Use this for initialization
    void Start () {
        m_RTrans = GetComponent<RectTransform>();

        movedRTrans = transform.parent.Find("MovedBack").GetComponent<RectTransform>();
        slidedRTrans = transform.parent.Find("SlidedBack").GetComponent<RectTransform>();

        SetLerpStart(movedRTrans);
    }
	
	// Update is called once per frame
	void Update () {
        StateUpdate();

    }

    private void StateUpdate()
    {
        switch (m_State)
        {
            case ScShoState.CHANGE: UpdateChangeState(); break;
            case ScShoState.SLIDEWAIT: UpdateSlideWait(); break;
            case ScShoState.SLIDE: UpdateSlide(); break;
        }
    }

    private void NextState()
    {
        m_State++;
        switch (m_State)
        {
            case ScShoState.SLIDEWAIT: StartSlideWait();break;
            case ScShoState.SLIDE: StartSlide();break;
        }
    }

    private void SetLerpStart(RectTransform target)
    {
        startPos = m_RTrans.localPosition;
        startSize = m_RTrans.localScale;

        startTime = Time.time;
        kyori = Vector3.Distance(startPos, target.localPosition);
    } 

    private void UpdateChangeState()
    {
        //画像の移動、大きさ変更
        float nowTime = (Time.time - startTime) * lerpSpeed;
        float nowPos = nowTime / kyori;
        m_RTrans.localPosition = Vector3.Lerp(startPos, movedRTrans.localPosition, nowPos);
        m_RTrans.localScale = Vector3.Lerp(startSize, movedRTrans.localScale, nowPos);

        if (nowPos >= 1)
        {
            NextState();
        }
    }

    private void StartSlideWait()
    {
        GameObject photo = Instantiate(photoPrefab,transform.parent);
        photo.GetComponent<RectTransform>().anchoredPosition = new Vector2(340, 0);
        photo.transform.parent = transform;

        if (isHit)
        {
            photo.transform.Find("Photo").gameObject.SetActive(true);
            photo.transform.Find("Photo").GetComponent<RawImage>().texture = Resources.Load("Texture/RenderTextures/XrayCamera" + photoNum) as RenderTexture;

            photo.transform.Find("Back").gameObject.SetActive(true);
        }
        else
        {
            photo.transform.Find("Cross").gameObject.SetActive(true);
        }
    }

    private void UpdateSlideWait()
    {
        slideWaitTime -= Time.deltaTime;
        if(slideWaitTime <= 0)
        {
            NextState();
        }
    }

    private void StartSlide()
    {

        SetLerpStart(slidedRTrans);
    }

    private void UpdateSlide()
    {
        float nowTime = (Time.time - startTime) * lerpSpeed;
        float nowPos = nowTime / kyori;
        m_RTrans.localPosition = Vector3.Lerp(startPos, slidedRTrans.localPosition, nowPos);

        if (nowPos >= 1)
        {
            Destroy(gameObject);
        }
    }

    public void SetXrayPhoto(string num, bool hit)
    {
        photoNum = num;
        isHit = hit;
        //xrayImg.texture = Resources.Load("Texture/RenderTextures/XrayCamera" + num) as RenderTexture;
    }
}
