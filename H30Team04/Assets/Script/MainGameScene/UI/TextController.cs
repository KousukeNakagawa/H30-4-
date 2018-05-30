using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextController : MonoBehaviour { 

    enum tutorialState
    {
        Tutorial,
        Game,
    }

    [SerializeField, TooltipAttribute("trueならチュートリアル用。falseならゲーム開始演出用")]
    bool m_TutorialChange;
    [SerializeField]
    GameObject m_rawImage;
    [SerializeField]
    GameObject m_PhDcamera;
    [SerializeField]
    string[] m_Scenarios;
    [SerializeField]
    private Text m_uiText;
    [SerializeField]
    private GameObject m_panel;
    CRT m_crt;

    [SerializeField]
    [Range(0.001f, 0.3f)]
    float intervalForCharacterDisplay = 0.05f;  // 1文字の表示にかかる時間

    private string currentText = string.Empty;  // 現在の文字列
    private float timeUntilDisplay = 0;     // 表示にかかる時間
    private float timeElapsed = 1;          // 文字列の表示を開始した時間
    private int lastUpdateCharacter = -1;		// 表示中の文

    int m_currentLine = 0;　　// 現在の行番号
    float m_time = 0;
    float m_textEndtimer = 0;

    bool m_PhDface = false;
    bool m_scenarioi = false;

    private tutorialState m_tutorialState;

    public bool IsCompleteDisplayText
    {
        get { return Time.time > timeElapsed + timeUntilDisplay; }
    }

    // Use this for initialization
    void Start () {
        switch (m_tutorialState)
        {
            case tutorialState.Tutorial : TutorialState(); break;
            case tutorialState.Game:GameState();break;
        }
        m_crt = m_PhDcamera.GetComponent<CRT>();
        m_PhDface = true;
        if (m_TutorialChange)
        {
            m_tutorialState = tutorialState.Tutorial;
            m_crt.ScanLineTail = 2;
            SetNextSpeak();
        }
        else
        {
            m_tutorialState = tutorialState.Game;
            m_crt.ScanLineTail = 0;
            m_scenarioi = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(m_time);
        if(m_tutorialState== tutorialState.Tutorial)
        {
            TutorialState();
        }
        else
        {
            GameState();
        }
    }

    //チュートリアル用セリフ制御
    void TutorialState()
    {
        if (m_PhDface)
        {
            m_panel.SetActive(true);
            m_scenarioi = true;
            if (IsCompleteDisplayText)
            {
                if (m_currentLine < m_Scenarios.Length && Input.GetKeyDown(KeyCode.Z) || Input.GetButtonDown("Shutter"))
                {
                    SetNextSpeak();
                }
                else if (m_currentLine >= m_Scenarios.Length && Input.GetKeyDown(KeyCode.Z) || Input.GetButtonDown("Shutter"))
                {
                    currentText = " ";
                }
                else if (m_currentLine >= m_Scenarios.Length)
                {
                    m_textEndtimer += Time.deltaTime;
                    if (m_textEndtimer > 2)
                    {
                        m_PhDface = false;
                    }
                }
            }
            if (m_currentLine >= 2)
            {
                m_rawImage.SetActive(false);
            }
            else
            {
                m_rawImage.SetActive(true);
            }
        }
        else
        {
            currentText = " ";
        }

        // クリックから経過した時間が想定表示時間の何%か確認し、表示文字数を出す
        int displayCharacterCount = (int)(Mathf.Clamp01((Time.time - timeElapsed) / timeUntilDisplay) * currentText.Length);

        // 表示文字数が前回の表示文字数と異なるならテキストを更新する
        if (displayCharacterCount != lastUpdateCharacter)
        {
            m_uiText.text = currentText.Substring(0, displayCharacterCount);
            lastUpdateCharacter = displayCharacterCount;
        }
    }

    //ゲーム本編＆ゲーム開始演出用セリフ演出
    void GameState()
    {
        if (m_PhDface)
        {
            m_panel.SetActive(true);
            m_rawImage.SetActive(true);
            OpenPhDface(2f);
            if (IsCompleteDisplayText)
            {
                if (m_currentLine < m_Scenarios.Length && Input.GetKeyDown(KeyCode.Z) || Input.GetButtonDown("Shutter"))
                {
                    SetNextSpeak();
                }
                else if(m_currentLine >= m_Scenarios.Length && Input.GetKeyDown(KeyCode.Z) || Input.GetButtonDown("Shutter"))
                {
                    currentText = " ";
                }
                if (m_currentLine >= m_Scenarios.Length)
                {
                    m_textEndtimer += Time.deltaTime;
                    if (m_textEndtimer > 2)
                    {
                        m_PhDface = false;
                    }
                }
            }
        }
        else
        {
            currentText = " ";
            ClosePhDface(0.0f);
        }

        // クリックから経過した時間が想定表示時間の何%か確認し、表示文字数を出す
        int displayCharacterCount = (int)(Mathf.Clamp01((Time.time - timeElapsed) / timeUntilDisplay) * currentText.Length);

        // 表示文字数が前回の表示文字数と異なるならテキストを更新する
        if (displayCharacterCount != lastUpdateCharacter)
        {
            m_uiText.text = currentText.Substring(0, displayCharacterCount);
            lastUpdateCharacter = displayCharacterCount;
        }
    }

    //開始・終了演出時の博士のセリフ
    void SetNextSpeak()
    {
        // 現在の行のテキストをuiTextに流し込み、現在の行番号を追加する
        currentText = m_Scenarios[m_currentLine];

        // 想定表示時間と現在の時刻をキャッシュ
        timeUntilDisplay = currentText.Length * intervalForCharacterDisplay;
        timeElapsed = Time.time;
        m_currentLine++;
        // 文字カウントを初期化
        lastUpdateCharacter = -1;
    }


    //博士の枠を出す
    void OpenPhDface(float i)
    {
        m_time += Time.deltaTime;
        m_crt.ScanLineTail = m_time;
        if (m_time > i)
        {
            m_time = i;
            if (m_scenarioi)
            {
                SetNextSpeak();
                m_scenarioi = false;
            }
        }
    }

    //博士の枠を消す
    void ClosePhDface(float i)
    {
        m_time -= Time.deltaTime;
        m_crt.ScanLineTail = m_time;
        if (m_time < i)
        {
            m_time = i;
            m_panel.SetActive(false);
            m_rawImage.SetActive(false);
            m_textEndtimer = 0;
        }
    }
}
