using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileBeaconSearch : MonoBehaviour {

    public float searchRange = 30.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (BigEnemyScripts.missileLaunch.isMissile)
        {
            if (!BigEnemyScripts.missileLaunch.isLaunch) return;
            GameObject[] bs = GameObject.FindGameObjectsWithTag("Beacon");
            SortedList<float, GameObject> beacons = new SortedList<float, GameObject>();
            foreach (GameObject m in bs)
            {
                beacons.Add((m.transform.position - BigEnemyScripts.mTransform.position).magnitude, m);
            }
            foreach (var beacon in beacons)
            {
                print(beacon.Key);
                if (beacon.Key <= searchRange)
                {
                    BigEnemyScripts.searchObject.MissileTargetChange(beacon.Value);
                }
                break;
            }
        }
	}
}
