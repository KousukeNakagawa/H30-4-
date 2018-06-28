using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPad : MonoBehaviour {

    public GameObject padA;
    public GameObject padB;
    public GameObject padX;
    public GameObject padY;
    public GameObject padLT;
    public GameObject padRight;
    public GameObject padLeft;

    public TutorialManager_T tmane;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //if(tmane.GetState() == TutorialState_T.START)
        //{
        //    return;
        //}

        if (!tmane.IsReaded())
        {
            padA.SetActive(true);
            padB.SetActive(false);
            padX.SetActive(false);
            padY.SetActive(false);
            padLT.SetActive(false);
            padRight.SetActive(false);
            padLeft.SetActive(false);
            return;
        }

        SetPadActive(tmane.GetState());
	}

    private void SetPadActive(TutorialState_T state)
    {
        padA.SetActive(PadAActive(state));
        padB.SetActive(PadBActive(state));
        padX.SetActive(PadXActive(state));
        padY.SetActive(PadYActive(state));
        padLT.SetActive(PadLTActive(state));
        padRight.SetActive(PadRightActive(state));
        padLeft.SetActive(PadLeftActive(state));
    }

    private bool PadAActive(TutorialState_T state)
    {
        bool result = false;
        switch (state)
        {
            case TutorialState_T.UI:
            case TutorialState_T.ROBOT:
            case TutorialState_T.SHUTTER:
            case TutorialState_T.CURSORCHANGEEND:
                result = true;
                break;
        }
        return result;
    }

    private bool PadBActive(TutorialState_T state)
    {
        bool result = false;
        switch (state)
        {
            case TutorialState_T.CURSORCHANGE:
                result = true;
                break;
        }
        return result;
    }

    private bool PadXActive(TutorialState_T state)
    {
        bool result = false;
        switch (state)
        {
            case TutorialState_T.BEACON:
            case TutorialState_T.SNIPER:
            case TutorialState_T.SHOT:
                result = true;
                break;
        }
        return result;
    }

    private bool PadYActive(TutorialState_T state)
    {
        bool result = false;
        switch (state)
        {
            case TutorialState_T.SNIPER:
            case TutorialState_T.XRAYCHECK:
                result = true;
                break;
        }
        return result;
    }

    private bool PadLTActive(TutorialState_T state)
    {
        bool result = false;
        switch (state)
        {
            case TutorialState_T.SHUTTER:
                result = true;
                break;
        }
        return result;
    }

    private bool PadRightActive(TutorialState_T state)
    {
        bool result = false;
        switch (state)
        {
            case TutorialState_T.MOVE:
            case TutorialState_T.BEACON:
            case TutorialState_T.SHOT:
                result = true;
                break;
        }
        return result;
    }

    private bool PadLeftActive(TutorialState_T state)
    {
        bool result = false;
        switch (state)
        {
            case TutorialState_T.CAMERA:
            case TutorialState_T.BEACON:
            case TutorialState_T.SHUTTER:
            case TutorialState_T.CURSORCHANGE:
            case TutorialState_T.SNIPER:
                result = true;
                break;
        }
        return result;
    }
}
