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
        int[] isSets = { 0, 0, 0, 0, 0, 0 };
        for (int i = 0; i < list.Count; i++)
        {
            isSets[list[i].num] = list[i].probability;
        }

        for (int i = 0; i < weekTexts.Count; i++)
        {
            weekTexts[i].SetText(isSets[i]);
        }
    }

    //public void AllQuestion()
    //{
    //    for (int i = 0; i < weekTexts.Count; i++)
    //    {
    //        weekTexts[i].SetText("0%");
    //    }
    //}
}
