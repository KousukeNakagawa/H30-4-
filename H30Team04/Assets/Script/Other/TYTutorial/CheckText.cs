using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckText : MonoBehaviour {
    [SerializeField,Multiline]
    string[] m_Texts;

    public TutorialManager_T tmane;

    private Image waku;
    private Image back;

    private Text m_Text;
    // Use this for initialization
    void Start () {
        m_Text = GetComponent<Text>();
        waku = transform.parent.GetComponent<Image>();
        back = transform.parent.Find("Back").GetComponent<Image>();
        SetImageActive(false);
    }
	
	// Update is called once per frame
	void Update () {
        if(tmane.GetState() >= TutorialState_T.SHOTEFFECT) //枠の役割終了
        {
            transform.parent.gameObject.SetActive(false);
            this.enabled = false;
            return;
        }

        if (!tmane.IsReaded()) //読んでる途中
        {
            m_Text.text = " ";
            SetImageActive(false);
            return;
        }

        switch (tmane.GetState())
        {
            case TutorialState_T.MOVE:m_Text.text = m_Texts[0]; SetImageActive(true); break;
            case TutorialState_T.CAMERA: m_Text.text = m_Texts[1]; SetImageActive(true); break;
            case TutorialState_T.BEACON: m_Text.text = m_Texts[2]; SetImageActive(true); break;
            case TutorialState_T.SHUTTER: m_Text.text = m_Texts[3]; SetImageActive(true); break;
            case TutorialState_T.CURSORCHANGE: m_Text.text = m_Texts[4]; SetImageActive(true); break;
            case TutorialState_T.SNIPER: m_Text.text = m_Texts[5]; SetImageActive(true); break;
            default: SetImageActive(false);break;
        }
    }

    private void SetImageActive(bool active)
    {
        waku.enabled = active;
        back.enabled = active;
    }
}
