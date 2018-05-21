using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// manager

public class TestSSS : MonoBehaviour {

    Dictionary<GameObject, float> cameraDict = new Dictionary<GameObject, float>();

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void UpdateCameraDistance(GameObject camera, float newDist)
    {
        cameraDict[camera] = newDist;
    }

    public List<KeyValuePair<GameObject, float>> sortedCameraDict()
    {
        var list = cameraDict.ToList();
        list.Sort((pair1, pair2) => pair1.Value.CompareTo(pair2.Value));

        return list;
    }
}
