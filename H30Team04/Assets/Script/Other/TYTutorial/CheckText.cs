using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckText : MonoBehaviour {
    [SerializeField,Multiline]
    string[] m_Texts;

    public TutorialManager_T tmane;

    private Text m_Text;
    // Use this for initialization
    void Start () {
        m_Text = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        if(tmane.GetState() > TutorialState_T.SNIPER)
        {
            transform.parent.gameObject.SetActive(false);
            return;
        }

        if (!tmane.IsReaded())
        {
            m_Text.text = " ";
            return;
        }

        switch (tmane.GetState())
        {
            case TutorialState_T.MOVE:m_Text.text = m_Texts[0];break;
            case TutorialState_T.CAMERA: m_Text.text = m_Texts[1]; break;
            case TutorialState_T.BEACON: m_Text.text = m_Texts[2]; break;
            case TutorialState_T.SHUTTER: m_Text.text = m_Texts[3]; break;
            case TutorialState_T.CURSORCHANGE: m_Text.text = m_Texts[4]; break;
            case TutorialState_T.SNIPER: m_Text.text = m_Texts[5]; break;
        }
	}
}
