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

    private int plusminus = -1;
    private float alpha = 1.0f;
    private GameObject selectedObj;

    RectTransform m_RectTransform;

    // Use this for initialization
    void Start () {
        selectedObj = gameObj;
    }

    void Awake()
    {
        m_RectTransform = GetComponent<RectTransform>();
    }




    // Update is called once per frame
    void Update () {
    //       if (EventSystem.current.currentSelectedGameObject == gameObj)
    //       {
    //           if (selectedObj != EventSystem.current.currentSelectedGameObject)
    //           {
    //               selectedObj = EventSystem.current.currentSelectedGameObject;
    //               transform.position = gameObj.transform.position;
    //           }
    //       }
    //       else
    //       {
    //           if (selectedObj != EventSystem.current.currentSelectedGameObject)
    //           {

    //               selectedObj = EventSystem.current.currentSelectedGameObject;
    //               transform.position = gameObj2.transform.position;
    //           }
    //       }
      }
    void LateUpdate()
    {
        // EventSystemに今選択されているオブジェクトを教えてもらう
        GameObject selectedObject =
            EventSystem.current.currentSelectedGameObject;

        // 何も選択されていなかったら何もしない
        if (selectedObject == null)
        {
            return;
        }

        // 選択されているオブジェクトの場所にカーソルを表示する
        m_RectTransform.anchoredPosition =
            selectedObject.GetComponent<RectTransform>().anchoredPosition;
    }

}
