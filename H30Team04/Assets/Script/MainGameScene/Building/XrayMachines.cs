using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XrayMachines : MonoBehaviour {


    public static List<GameObject> xrayMachineObjects;
	// Use this for initialization
	void Awake () {
        xrayMachineObjects = new List<GameObject>();
        GameObject[] machines = GameObject.FindGameObjectsWithTag("Xline");
        foreach (GameObject machine in machines)
        {
            xrayMachineObjects.Add(machine);
        }
        foreach (GameObject machine in xrayMachineObjects)
        {
            //Debug.Log(machine.transform.position);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public static void RemoveObj(GameObject obj)
    {
        //int num = 0;
        //while(num> xrayMachineObjects.Count)
        //{
        //    if(xrayMachineObjects[num].transform.position == obj.transform.position)
        //    {
        //        xrayMachineObjects.RemoveAt(num);
        //        num += 100;
        //    }
        //    num++;
        //}

        xrayMachineObjects.Remove(obj);

        foreach (GameObject machine in xrayMachineObjects)
        {
            //Debug.Log(machine);
        }
    }
}
