using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class SelectSentaku : MonoBehaviour
{

    [SerializeField]
    private GameObject Map;
    [SerializeField]
    private GameObject Control;
    [SerializeField]
    private GameObject Title;
    [SerializeField]
    private Text m_maptext;
    [SerializeField]
    private Text m_controltext;
    [SerializeField]
    private Text m_titletext;

    public float m_Left_xPos;
    public float m_Center_xPos;
    public float m_Right_xPos;

    private enum SelectMove
    {
        Right,
        Center,
        Left,
    }
    private SelectMove selectMove;
    // Use this for initialization
    void Start()
    {
        selectMove = SelectMove.Left;
    }

    // Update is called once per frame
    void Update()
    {
        switch (selectMove)
        {
            case SelectMove.Left:
                transform.position = new Vector3(m_Left_xPos, 390, 0);
                transform.localScale = new Vector3(1, 1, 1);
                m_maptext.fontStyle = FontStyle.Bold;
                break;
            case SelectMove.Center:
                transform.position = new Vector3(m_Center_xPos, 390, 0);
                transform.localScale = new Vector3(1.85f, 1, 1);
                m_controltext.fontStyle = FontStyle.Bold;
                break;
            case SelectMove.Right:
                transform.position = new Vector3(m_Right_xPos, 390, 0);
                transform.localScale = new Vector3(2.7f, 1, 1);
                m_titletext.fontStyle = FontStyle.Bold;
                break;
        }
        if (Input.GetButtonDown("LockAt") && selectMove == SelectMove.Left)
        {
            selectMove = SelectMove.Center;
            Map.SetActive(true);
        }
        else if (Input.GetButtonDown("Shooting") && selectMove == SelectMove.Left)
        {
            selectMove = SelectMove.Right;
            Map.SetActive(true);
        }
        else if (Input.GetButtonDown("LockAt") && selectMove == SelectMove.Center)
        {
            selectMove = SelectMove.Right;
            Control.SetActive(true);
        }
        else if (Input.GetButtonDown("Shooting") && selectMove == SelectMove.Center)
        {
            selectMove = SelectMove.Left;
            Control.SetActive(true);
        }
        else if (Input.GetButtonDown("LockAt") && selectMove == SelectMove.Right)
        {
            selectMove = SelectMove.Left;
            Title.SetActive(true);
        }
        else if (Input.GetButtonDown("Shooting") && selectMove == SelectMove.Right)
        {
            selectMove = SelectMove.Center;
            Title.SetActive(true);
        }
        if (selectMove == SelectMove.Left)
        {
            Map.SetActive(false);
            m_controltext.fontStyle = FontStyle.Normal;
            m_titletext.fontStyle = FontStyle.Normal;
        }
        else if (selectMove == SelectMove.Center)
        {
            Control.SetActive(false);
            m_maptext.fontStyle = FontStyle.Normal;
            m_titletext.fontStyle = FontStyle.Normal;
        }
        else if (selectMove == SelectMove.Right)
        {
            Title.SetActive(false);
            m_maptext.fontStyle = FontStyle.Normal;
            m_controltext.fontStyle = FontStyle.Normal;
        }
    }
}
