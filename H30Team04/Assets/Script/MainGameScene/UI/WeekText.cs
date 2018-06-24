using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeekText : MonoBehaviour {

    private Text probabilityText;

    // Use this for initialization
    void Awake () {
        probabilityText = transform.Find("Probability").GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetText(string text)
    {
        probabilityText.text = text;

        //bool isHide = (text != "0%");
        //foreach (Transform child in transform)
        //{
        //    child.gameObject.SetActive(isHide);
        //}
    }
}
