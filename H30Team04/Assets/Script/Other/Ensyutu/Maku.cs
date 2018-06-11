using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maku : MonoBehaviour {

    public XrayMachine target;
    private GameObject xrayMaku;
    private bool equalBool = false;
    private bool isOk = true;

	// Use this for initialization
	void Start () {
        xrayMaku = transform.Find("XrayMaku").gameObject;
	}
	
	// Update is called once per frame
	void Update () {
        equalBool = (target.GetXrayOK() == isOk) ? true : false;

        if (equalBool)
        {
            isOk = false;
            xrayMaku.SetActive(true);
        }
	}
}
