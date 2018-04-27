using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeekTextManager : MonoBehaviour {

    private List<WeekText> weekTexts;

	// Use this for initialization
	void Awake () {
        weekTexts = new List<WeekText>();

        foreach (Transform child in transform)
        {
            weekTexts.Add(child.GetComponent<WeekText>());
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetTexts(List<GameManager.WeekPointProbability> list)
    {
        for(int i = 0; i < list.Count; i++)
        {
            weekTexts[list[i].num].SetText(list[i].probability + "%");
        }
    }

    public void AllQuestion()
    {
        for (int i = 0; i < weekTexts.Count; i++)
        {
            weekTexts[i].SetText("?%");
        }
    }
}
