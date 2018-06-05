using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonEvent : MonoBehaviour
{

    //　インフォメーションテキストに表示する文字列
    [SerializeField]
    private string informationString;
    //　インフォメーションテキスト
    [SerializeField]
    private Text informationText;
    //　自身の親のCanvasGroup
    private CanvasGroup canvasGroup;
  
    bool sw;
    [SerializeField]
    private GameObject pauseUI;
    [SerializeField]
    private Text m_text;


    void Start()
    {
        canvasGroup = GetComponentInParent<CanvasGroup>();
    }

    void OnEnable()
    {
        //　装備アイテム選択中にステータス画面を抜けた時にボタンが無効化したままの場合もあるので立ち上げ時に有効化する
        GetComponent<Button>().interactable = true;
    }

    //　ボタンの上にマウスが入った時、またはキー操作で移動してきた時
    public void OnSelected()
    {
        if (canvasGroup == null || canvasGroup.interactable)
        {
            //　イベントシステムのフォーカスが他のゲームオブジェクトにある時このゲームオブジェクトにフォーカス
            if (EventSystem.current.currentSelectedGameObject != gameObject)
            {
                EventSystem.current.SetSelectedGameObject(gameObject);
            }
            informationText.text = informationString;
            m_text.fontStyle = FontStyle.Bold;
        }
    }
    //　ボタンから移動したら情報を削除
    public void OnDeselected()
    {
        informationText.text = "";
        m_text.fontStyle = FontStyle.Normal;
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

    //　他の画面を表示する
    public void WindowOnOff(GameObject window)
    {
        if (canvasGroup == null || canvasGroup.interactable)
        {
            Camera.main.GetComponent<OperationStatusWindow>().ChangeWindow(window);
            if (sw)
            {
                //　ポーズUIのアクティブ、非アクティブを切り替え
                pauseUI.SetActive(!pauseUI.activeSelf);

                //　ポーズUIが表示されてる時は停止
                if (pauseUI.activeSelf)
                {
                    Time.timeScale = 0f;
                    //　ポーズUIが表示されてなければ通常通り進行
                }
                else
                {
                    Time.timeScale = 1f;
                }
            }
        }
    }
    public void StringArgFunction(string s)
    {
        ScecnManager.SceneChange(s);
    }
}