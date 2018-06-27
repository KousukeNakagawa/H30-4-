using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeekText : MonoBehaviour {

    private Text probabilityText;
    private int num = 0;
    private int count = 0;

    public WeekPoint m_Point;

    // Use this for initialization
    void Awake () {
        probabilityText = transform.Find("Probability").GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetText(int n)
    {
        num += n;
        count++;

        probabilityText.text = (num / count) + "%";

        if (m_Point != null) m_Point.Par = (num / count);

    }

    public void NumReset()
    {
        num = 0;
        count = 0;
        probabilityText.text = "0%";
    }
}
