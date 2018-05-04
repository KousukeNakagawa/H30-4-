using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideParent : MonoBehaviour {

    [SerializeField] private float startPosX = 0;
    [SerializeField] private float endPosX = 0;
    [SerializeField] private float speed = 5;

    [SerializeField] private bool isStop = false;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (isStop)
        {
            foreach(Transform child in transform)
            {
                child.GetComponent<BuilSlide>().enabled = false;
                this.enabled = false;
            }
        }
	}

    public void SlideStop()
    {
        isStop = true;
    }

    public float StartPosX
    {
        get { return startPosX; }
    }

    public float EndPosX
    {
        get { return endPosX; }
    }

    public float Speed
    {
        get { return speed; }
    }
}
