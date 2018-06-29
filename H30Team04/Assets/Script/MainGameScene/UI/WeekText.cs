using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeekText : MonoBehaviour {

    private Text probabilityText;
    private int num = 0;
    private int count = 0;

    public WeekPoint m_Point;

    private int nextnum = 0;
    private int nownum = 0;

    // Use this for initialization
    void Awake () {
        probabilityText = transform.Find("Probability").GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        if (nextnum == nownum) return;

        if (nownum > nextnum)
        {
            nownum--;
            if (nownum > nextnum + 10)
            {
                nownum -= 10;
            }
        }
        else
        {
            nownum++;
            if (nownum < nextnum - 10)
            {
                nownum += 10;
            }
        }
        TextUpdate();
    }

    private void TextUpdate()
    {
        probabilityText.text = nownum + "%";
    }

    public void SetText(int n)
    {
        num += n;
        count++;
        nextnum = (num / count);
        //probabilityText.text = (num / count) + "%";

        if (m_Point != null) m_Point.Par = nextnum;

    }

    public void NumReset()
    {
        num = 0;
        count = 0;
        nextnum = 0;
        nownum = 0;
        probabilityText.text = nownum + "%";
    }
}
