using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// player

public class TestSortedDictReader : MonoBehaviour {

    public TestSSS manager;

    GameObject currentCamera;
    GameObject camera1;
    GameObject camera2;

    GameObject oldCurrentCamera;
    GameObject oldCamera1;
    GameObject oldCamera2;

    GameObject lockedOnCamera;

    private void Reset()
    {
        manager = GetComponent<TestSSS>();
    }

    //TestCamera lockedOnCamera = null;

    // Use this for initialization
    void Start() {
        lockedOnCamera = null;
    }

    // Update is called once per frame
    void Update() {

        oldCamera1 = camera1;
        oldCamera2 = camera2;
        oldCurrentCamera = currentCamera;

        var sortedCameraDict = manager.sortedCameraDict();

        camera1 = sortedCameraDict[0].Key;
        camera2 = sortedCameraDict[1].Key;

        if (camera1 == oldCamera1 || camera1 == oldCamera2)
        {
            currentCamera = oldCurrentCamera;
        }

        if (camera2 != oldCamera1 || camera2 != oldCamera2)
        {
            currentCamera = camera1;
        }


        // if lockOnButton is pressed
            // lockedOnCamera = currentCamera;

        // if cameraSwitch is pressed
            // lockedOnCamera = null;
            // currentCamera = camera1 mataha camera2

        // if lockedOnCamera == null
            // arrow aim currentCamera
        // else
            // arrow aim lockedOnCamera
    }
}
