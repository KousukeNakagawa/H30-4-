using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialText_T : MonoBehaviour {


    [SerializeField]
    GameObject m_rawImage;
    [SerializeField]
    GameObject m_PhDcamera;
    [SerializeField]
    [Multiline]
    string[] m_Scenarios;
    public Text m_uiText;
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
    private static int _nowtext = -1;

    int m_currentLine = 0;　　// 現在の行番号
    float m_time = 0;
    float m_textEndtimer = 0;

    bool m_PhDface = false;
    /// </summary>
    bool m_scenarioi = false;
    private string[] sScenarios;

    private float yoin = 3.0f;
    private float yoinTime = 0.0f;

    public bool IsCompleteDisplayText
    {
        get { return Time.time > timeElapsed + timeUntilDisplay; }
    }

    // Use this for initialization
    void Awake()
    {
        sScenarios = m_Scenarios;
        m_crt = m_PhDcamera.GetComponent<CRT>();
        m_crt.ScanLineTail = 0;
        //FailedText(0);
    }


    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 0) return;

        //if(Fade.IsFadeEnd() && Fade.IsFadeOutOrIn())
        //{
        //    ScecnManager.SceneChange("GamePlay");
        //}

        if (m_PhDface)
        {
            //OpenPhDface(2.0f);
            if (IsCompleteDisplayText)
            {
                m_textEndtimer += Time.deltaTime;
                if (m_textEndtimer > 5)
                {
                    //m_PhDface = false;
                    m_textEndtimer = 0;
                }
            }
        }
        else
        {
            //currentText = " ";
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

    /// <summary>
    /// 特定のアクションが行われたときのはさせのセリフ処理
    /// </summary>
    /// <param name="i"></param>
    public void TextStart(int i)
    {
        Debug.Log("1.5");
        if (sScenarios[i] != null)
        {
            _nowtext = i;
            m_PhDface = true;
            m_scenarioi = true;
            m_panel.SetActive(true);
            GetNextText();
        }

    }

    //博士の現場をアナウンスするセリフ
    void GetNextText()
    {
        //m_currentLine = _nowtext;
        // 現在の行のテキストをuiTextに流し込み、現在の行番号をランダムで追加する
        currentText = sScenarios[_nowtext];

        // 想定表示時間と現在の時刻をキャッシュ
        timeUntilDisplay = currentText.Length * intervalForCharacterDisplay;
        timeElapsed = Time.time;

        // 文字カウントを初期化
        lastUpdateCharacter = -1;


    }

    //博士の枠を出す
    public void OpenPhDface(float i)
    {
        m_rawImage.SetActive(true);
        m_panel.SetActive(true);
        m_time += Time.deltaTime;
        m_crt.ScanLineTail = m_time;
        if (m_time > i)
        {
            m_time = i;

                TextStart(0);
                m_scenarioi = false;
        }

    }

    //博士の枠を消す
    public void ClosePhDface(float i)
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

    public void SkipOpenFace(float i)
    {
        m_time = i;
    }

    public void SkipScenario()
    {
        timeUntilDisplay = 0;
    }

    public bool IsFase()
    {
        return m_PhDface;
    }

    public void HidePanel()
    {
        m_panel.SetActive(false);
    }
}
