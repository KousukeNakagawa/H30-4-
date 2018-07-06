using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideLoop : MonoBehaviour {

    public TutorialManager_T tmane;

    private float nextTime =0;
    private float interval = 0.8f;

    public int textNumber = 0;

    // Use this for initialization
    void Start () {

        //nextTime = Time.time;
    }
	
	// Update is called once per frame
	void Update () {
        if(tmane.GetTextNum() > textNumber)
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(true);
            }
            this.enabled = false;
            return;
        }
        else if (nextTime == 0 && tmane.GetTextNum() == textNumber)
        {
            nextTime = Time.time;
        }
        else if (Time.time > nextTime && tmane.GetTextNum() == textNumber)
        {
            foreach(Transform child in transform)
            {
                child.gameObject.SetActive(!child.gameObject.activeSelf);
            }
            nextTime += interval;
        }
    }
}
