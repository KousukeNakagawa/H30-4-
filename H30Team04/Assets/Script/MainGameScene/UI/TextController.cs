using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextController : MonoBehaviour {
    enum SpeakPhD
    {
        StartEndSpeak,
        GameSpeak,
    };

    [SerializeField]
    bool m_PhDChange = false;
    [SerializeField]
    GameObject m_rawImage;
    [SerializeField]
    GameObject m_PhDcamera;
    [SerializeField]
    string[] m_Scenarios;
    [SerializeField]
    string[] m_phDScenarios;
    public Text m_uiText;
    CRT m_crt;
    private SpeakPhD _speakphd;

    [SerializeField]
    [Range(0.001f, 0.3f)]
    float intervalForCharacterDisplay = 0.05f;  // 1文字の表示にかかる時間

    private string currentText = string.Empty;  // 現在の文字列
    private float timeUntilDisplay = 0;     // 表示にかかる時間
    private float timeElapsed = 1;          // 文字列の表示を開始した時間
    private int lastUpdateCharacter = -1;		// 表示中の文

    int m_currentLine = 0;　　// 現在の行番号
    int m_Random;
    float m_time = 0;
    float m_textEndtimer = 0;

    bool m_PhDface1 = false;
    bool m_scenarioi = false;

    public bool IsCompleteDisplayText
    {
        get { return Time.time > timeElapsed + timeUntilDisplay; }
    }

    // Use this for initialization
    void Start () {
        m_crt = m_PhDcamera.GetComponent<CRT>();
        m_crt.ScanLineTail = 0;
        if (!m_PhDChange) _speakphd = SpeakPhD.GameSpeak;
        else _speakphd = SpeakPhD.StartEndSpeak;
    }

    // Update is called once per frame
    void Update () {
        if (_speakphd == SpeakPhD.GameSpeak)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (!m_PhDface1 )
                {
                    m_PhDface1 = true;
                    m_scenarioi = true;
                }
            }

            if (m_PhDface1)
            {
                m_rawImage.SetActive(true);
                OpenPhDface(2f);
                if (IsCompleteDisplayText)
                {
                    m_textEndtimer += Time.deltaTime;
                        if (m_textEndtimer > 4)
                        {
                            m_PhDface1 = false;
                        }
                }
            }
            else if(!m_PhDface1)
            {
                ClosePhDface(0.0f);
                currentText = "";
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (!m_PhDface1)
                {
                    m_PhDface1 = true;
                    m_scenarioi = true;
                }
            }

            if (m_PhDface1)
            {
                m_rawImage.SetActive(true);
                OpenPhDface(2f);
                if (IsCompleteDisplayText)
                {
                    if (m_currentLine < m_Scenarios.Length && Input.GetMouseButtonDown(0))
                    {
                        SetNextSpeak();
                    }
                    else
                    {
                        m_textEndtimer += Time.deltaTime;
                        if (m_textEndtimer > 4)
                        {
                            m_PhDface1 = false;
                        }
                    }
                }
                else
                {
                    // 完了してないなら文字をすべて表示する
                    if (Input.GetMouseButtonDown(0))
                    {
                        timeUntilDisplay = 0;
                    }
                }
            }
            else if (!m_PhDface1)
            {
                ClosePhDface(0.0f);
                currentText = "";
            }
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

    //ゲーム本編の博士のセリフ
    void SetNextLine()
    {
        m_Random = Random.Range(0, m_Scenarios.Length);
        m_currentLine = m_Random;
        // 現在の行のテキストをuiTextに流し込み、現在の行番号をランダムで追加する
        currentText = m_Scenarios[m_currentLine];

        // 想定表示時間と現在の時刻をキャッシュ
        timeUntilDisplay = currentText.Length * intervalForCharacterDisplay;
        timeElapsed = Time.time;

        // 文字カウントを初期化
        lastUpdateCharacter = -1;
    }

    //開始・終了演出時の博士のセリフ
    void SetNextSpeak()
    {
        // 現在の行のテキストをuiTextに流し込み、現在の行番号をランダムで追加する
        currentText = m_Scenarios[m_currentLine];

        // 想定表示時間と現在の時刻をキャッシュ
        timeUntilDisplay = currentText.Length * intervalForCharacterDisplay;
        timeElapsed = Time.time;
        m_currentLine++;
        // 文字カウントを初期化
        lastUpdateCharacter = -1;
    }

    //博士の現場をアナウンスするセリフ
    void GetNextText(int i)
    {
            m_currentLine = i;
            // 現在の行のテキストをuiTextに流し込み、現在の行番号をランダムで追加する
            currentText = m_phDScenarios[m_currentLine];

            // 想定表示時間と現在の時刻をキャッシュ
            timeUntilDisplay = currentText.Length * intervalForCharacterDisplay;
            timeElapsed = Time.time;

            // 文字カウントを初期化
            lastUpdateCharacter = -1;
       
    }


    /// <summary>
    /// sinarioNoはm_phDScenariosの番号。timeは2.0f。speakはtrueにする。
    /// </summary>
    /// <param name="sinarioNo"></param>
    /// <param name="timer"></param>
    /// <param name="speak"></param>
    public void SetNextText(int sinarioNo, float timer,bool speak)
    {
            m_rawImage.SetActive(true);
            m_time += Time.deltaTime;
            m_crt.ScanLineTail = m_time;
        if (m_time > timer)
        {
            m_time = timer;
            if (speak)
            {
                GetNextText(sinarioNo);
                speak = false;
                ClosePhDface(0.0f);
            }
        }
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
                if (_speakphd == SpeakPhD.GameSpeak) SetNextLine();
                else SetNextSpeak();
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
            m_rawImage.SetActive(false);
            m_textEndtimer = 0;
        }
    }
}
