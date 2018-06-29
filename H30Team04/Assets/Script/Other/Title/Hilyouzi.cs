using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Hilyouzi : MonoBehaviour
{
    [SerializeField]
    public GameObject gameObj;
    [SerializeField]
    public GameObject gameObj2;
    [SerializeField]
    public GameObject gameObj3;

    public Text gameObjText;
    public Text gameObj2Text;
    public Text gameObj3Text;
    public Outline gameObjOutline;
    public Outline gameObj2Outline;
    public Outline gameObj3Outline;

    private int plusminus = -1;
    private float alpha = 1.0f;
    private GameObject selectedObj;

    private AudioSource m_Audio;
    public AudioClip kettei;
    public AudioClip select;

    // Use this for initialization
    void Start()
    {
        gameObj.SetActive(false);
        gameObj2.SetActive(false);
        gameObj3.SetActive(false);
        selectedObj = gameObj;
        m_Audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space) || Input.GetButtonDown("Shutter")&&!gameObj.activeSelf)
        { 
            gameObj.SetActive(true);
            gameObj2.SetActive(true);
            gameObj3.SetActive(true);
            m_Audio.PlayOneShot(kettei);

            //ScecnManager.SceneChange("prototype");
        }
        if(Input.GetMouseButtonDown(0))
        {
            EventSystem.current.SetSelectedGameObject(gameObj);
        }
        if(EventSystem.current.currentSelectedGameObject == gameObj)
        {
            if(selectedObj != EventSystem.current.currentSelectedGameObject)
            {
                m_Audio.PlayOneShot(select);
                gameObj2Text.color = new Color(gameObj2Text.color.r, gameObj2Text.color.g, gameObj2Text.color.b, 1);
                gameObj2Outline.effectColor = new Color(gameObj2Outline.effectColor.r, gameObj2Outline.effectColor.g, gameObj2Outline.effectColor.b, 0.5f);
                gameObj3Text.color = new Color(gameObj3Text.color.r, gameObj3Text.color.g, gameObj3Text.color.b, 1);
                gameObj3Outline.effectColor = new Color(gameObj3Outline.effectColor.r, gameObj3Outline.effectColor.g, gameObj3Outline.effectColor.b, 0.5f);
                selectedObj = EventSystem.current.currentSelectedGameObject;
                plusminus = -1;
                alpha = 1;
            }
            alpha += Time.deltaTime * plusminus;
            if (alpha >= 1 || alpha <= 0) plusminus *= -1;
            alpha = Mathf.Clamp(alpha, 0, 1);

            gameObjText.color = new Color(gameObjText.color.r, gameObjText.color.g, gameObjText.color.b, alpha);
            gameObjOutline.effectColor = new Color(gameObjOutline.effectColor.r, gameObjOutline.effectColor.g, gameObjOutline.effectColor.b, alpha/2);
        }
        else if(EventSystem.current.currentSelectedGameObject == gameObj2)
        {
            if (selectedObj != EventSystem.current.currentSelectedGameObject)
            {
                m_Audio.PlayOneShot(select);
                gameObjText.color = new Color(gameObjText.color.r, gameObjText.color.g, gameObjText.color.b, 1);
                gameObjOutline.effectColor = new Color(gameObjOutline.effectColor.r, gameObjOutline.effectColor.g, gameObjOutline.effectColor.b, 0.5f);
                gameObj3Text.color = new Color(gameObj3Text.color.r, gameObj3Text.color.g, gameObj3Text.color.b, 1);
                gameObj3Outline.effectColor = new Color(gameObj3Outline.effectColor.r, gameObj3Outline.effectColor.g, gameObj3Outline.effectColor.b, 0.5f);
                selectedObj = EventSystem.current.currentSelectedGameObject;
                plusminus = -1;
                alpha = 1;
            }
            alpha += Time.deltaTime * plusminus;
            if (alpha >= 1 || alpha <= 0) plusminus *= -1;
            alpha = Mathf.Clamp(alpha, 0, 1);

            gameObj2Text.color = new Color(gameObj2Text.color.r, gameObj2Text.color.g, gameObj2Text.color.b, alpha);
            gameObj2Outline.effectColor = new Color(gameObj2Outline.effectColor.r, gameObj2Outline.effectColor.g, gameObj2Outline.effectColor.b, alpha/2);
        }
        else
        {
            if (selectedObj != EventSystem.current.currentSelectedGameObject)
            {
                m_Audio.PlayOneShot(select);
                gameObjText.color = new Color(gameObjText.color.r, gameObjText.color.g, gameObjText.color.b, 1);
                gameObjOutline.effectColor = new Color(gameObjOutline.effectColor.r, gameObjOutline.effectColor.g, gameObjOutline.effectColor.b, 0.5f);
                gameObj2Text.color = new Color(gameObj2Text.color.r, gameObj2Text.color.g, gameObj2Text.color.b, 1);
                gameObj2Outline.effectColor = new Color(gameObj2Outline.effectColor.r, gameObj2Outline.effectColor.g, gameObj2Outline.effectColor.b, 0.5f);
                selectedObj = EventSystem.current.currentSelectedGameObject;
                plusminus = -1;
                alpha = 1;
            }
            alpha += Time.deltaTime * plusminus;
            if (alpha >= 1 || alpha <= 0) plusminus *= -1;
            alpha = Mathf.Clamp(alpha, 0, 1);

            gameObj3Text.color = new Color(gameObj3Text.color.r, gameObj3Text.color.g, gameObj3Text.color.b, alpha);
            gameObj3Outline.effectColor = new Color(gameObj3Outline.effectColor.r, gameObj3Outline.effectColor.g, gameObj3Outline.effectColor.b, alpha / 2);
        }

        
    }
}
