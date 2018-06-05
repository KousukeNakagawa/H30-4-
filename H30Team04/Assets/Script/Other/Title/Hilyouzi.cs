using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Hilyouzi : MonoBehaviour
{
    [SerializeField]
    public GameObject gameObj;
    [SerializeField]
    public GameObject gameObj2;
    // Use this for initialization
    void Start()
    {
        gameObj.SetActive(false);
        gameObj2.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space) || Input.GetButtonDown("Shutter"))
        { 
            gameObj.SetActive(true);
            gameObj2.SetActive(true);

            //ScecnManager.SceneChange("prototype");
        }
        if(Input.GetMouseButtonDown(0))
        {
            EventSystem.current.SetSelectedGameObject(gameObj);
        }
    }
}
