using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Flashing : MonoBehaviour {
    [SerializeField]
    public GameObject gameObj;
    [SerializeField]
    public GameObject gameObj2;

    public Text gameObjText;
    public Text gameObj2Text;

    private int plusminus = -1;
    private float alpha = 1.0f;
    private GameObject selectedObj;

    // Use this for initialization
    void Start () {
        selectedObj = gameObj;
    }
	
	// Update is called once per frame
	void Update () {
        if (EventSystem.current.currentSelectedGameObject == gameObj)
        {
            if (selectedObj != EventSystem.current.currentSelectedGameObject)
            {            
                gameObj2Text.color = new Color(gameObj2Text.color.r, gameObj2Text.color.g, gameObj2Text.color.b, 1);
                selectedObj = EventSystem.current.currentSelectedGameObject;
                plusminus = -1;
                alpha = 1;
            }
            alpha += Time.deltaTime * plusminus;
            if (alpha >= 1 || alpha <= 0) plusminus *= -1;
            alpha = Mathf.Clamp(alpha, 0, 1);

            gameObjText.color = new Color(gameObjText.color.r, gameObjText.color.g, gameObjText.color.b, alpha);
        }
        else
        {
            if (selectedObj != EventSystem.current.currentSelectedGameObject)
            {
              
                gameObjText.color = new Color(gameObjText.color.r, gameObjText.color.g, gameObjText.color.b, 1);
               
                selectedObj = EventSystem.current.currentSelectedGameObject;
                plusminus = -1;
                alpha = 1;
            }
            alpha += Time.deltaTime * plusminus;
            if (alpha >= 1 || alpha <= 0) plusminus *= -1;
            alpha = Mathf.Clamp(alpha, 0, 1);
            gameObj2Text.color = new Color(gameObj2Text.color.r, gameObj2Text.color.g, gameObj2Text.color.b, alpha);
        }
    }
}
