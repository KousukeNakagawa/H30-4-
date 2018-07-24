using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPad : MonoBehaviour {

    public GameObject padA;
    public GameObject padX;
    public GameObject padY1;
    public GameObject padY2;
    public GameObject padLT;
    public GameObject padRT;
    public GameObject padRB;
    public GameObject padRight;
    public GameObject padLeft;
    public GameObject padLeft2;

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
            padX.SetActive(false);
            padY1.SetActive(false);
            padY2.SetActive(false);
            padLT.SetActive(false);
            padRT.SetActive(false);
            padRB.SetActive(false);
            padRight.SetActive(false);
            padLeft.SetActive(false);
            padLeft2.SetActive(false);
            return;
        }

        SetPadActive(tmane.GetState());
	}

    private void SetPadActive(TutorialState_T state)
    {
        padA.SetActive(PadAActive(state));
        padX.SetActive(PadXActive(state));
        padY1.SetActive(PadY1Active(state));
        padY2.SetActive(PadY2Active(state));
        padLT.SetActive(PadLTActive(state));
        padRT.SetActive(PadRTActive(state));
        padRB.SetActive(PadRBActive(state));
        padRight.SetActive(PadRightActive(state));
        padLeft.SetActive(PadLeft1Active(state));
        padLeft2.SetActive(PadLeft2Active(state));
    }

    private bool PadAActive(TutorialState_T state)
    {
        bool result = false;
        switch (state)
        {
            case TutorialState_T.UI:
            case TutorialState_T.ROBOT:
            case TutorialState_T.CURSORCHANGEEND:
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
            case TutorialState_T.SHUTTER:
                result = true;
                break;
        }
        return result;
    }

    private bool PadY1Active(TutorialState_T state)
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
    private bool PadY2Active(TutorialState_T state)
    {
        bool result = false;
        switch (state)
        {
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
    private bool PadRTActive(TutorialState_T state)
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
    private bool PadRBActive(TutorialState_T state)
    {
        bool result = false;
        switch (state)
        {
            case TutorialState_T.SNIPER:
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
            case TutorialState_T.CAMERA:
            case TutorialState_T.BEACON:
            case TutorialState_T.CURSORCHANGE:
            case TutorialState_T.SNIPER:
            case TutorialState_T.SHUTTER:
                result = true;
                break;
        }
        return result;
    }

    private bool PadLeft1Active(TutorialState_T state)
    {
        bool result = false;
        switch (state)
        {
            case TutorialState_T.MOVE:
            case TutorialState_T.BEACON:
            case TutorialState_T.SNIPER:
                result = true;
                break;
        }
        return result;
    }
    private bool PadLeft2Active(TutorialState_T state)
    {
        bool result = false;
        switch (state)
        {
            case TutorialState_T.SHOT:
                result = true;
                break;
        }
        return result;
    }
}
