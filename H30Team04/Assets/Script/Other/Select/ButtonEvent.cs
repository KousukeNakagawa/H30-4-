using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class ButtonEvent : MonoBehaviour
{
    //　自身の親のCanvasGroup
    private CanvasGroup canvasGroup;

    [SerializeField]
    private GameObject firstButton;
    void Start()
    {
       
    }

    void Update()
    {
     //if(Input.GetMouseButton(0))
     //   {
     //       EventSystem.current.SetSelectedGameObject(firstButton);
     //   }
    }
    //　ステータスウインドウを非アクティブにする
    public void DisableWindow()
    {
        if (canvasGroup == null || canvasGroup.interactable)
        {
            //　ウインドウを非アクティブにする
            transform.root.gameObject.SetActive(false);
        }
    }
    public void StringArgFunction(string s)
    {
        ScecnManager.SceneChange(s);
    }


}