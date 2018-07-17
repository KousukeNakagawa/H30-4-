using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Sensitivity : MonoBehaviour {

    private Slider slider;
    public static float Sensitivitytest;
    [SerializeField]
    public GameObject gameObj;


    public static float getSensitivitytest()
    {
        return Sensitivitytest;
    }
    // Use this for initialization2
    void Start () {
        slider = GetComponent<Slider>();
        EventSystem.current.SetSelectedGameObject(gameObj);
    }
	
	// Update is called once per frame
	void Update () {
        Sensitivitytest = slider.value;
        if(Input.GetMouseButton(0))
        {
            EventSystem.current.SetSelectedGameObject(gameObj);
        }
    }
   
}
