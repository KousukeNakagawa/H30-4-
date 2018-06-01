using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapScript : MonoBehaviour {
    Rect _rect = new Rect(0, 0, 1, 1);
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public Rect MiniCameraRect
    {
        get { return _rect; }
        set { _rect = value; }
    }
}
