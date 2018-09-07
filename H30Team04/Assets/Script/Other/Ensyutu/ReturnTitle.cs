using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//タイトルに戻る必要なかったけど、名前このままで
public class ReturnTitle : MonoBehaviour {

    // 操作が無い場合に自動で終了するまでの秒数   
    static readonly float AutoCloseDuration = 120;

    // 最後に入力があった時間    
    float lastInputTime; 

    

    void Awake()
    {
        // 重複防止措置。
        // 既にある場合は自身を削除する
        if (FindObjectsOfType<ReturnTitle>().Length > 1)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        Cursor.visible = false;
        lastInputTime = Time.time;
    }

    // Update is called once per frame
    void Update () {
        // XBOX コントローラーの BACK ボタンと START ボタン同時押し        
        // または      
        // キーボードのエスケープキーでゲームを強制終了する    
        if ((Input.GetKey(KeyCode.JoystickButton6) && Input.GetKey(KeyCode.JoystickButton7))        
            || Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
            return;
        }

        // 何かしらの入力があれば、最後に入力があった時間として現在の時間を記録する  
        if (InputAnyKey())
        {
            lastInputTime = Time.time;
        }

        // 最後に入力があった瞬間からの経過時間が、指定秒数を超えたら、強制終了する 
        if ((Time.time - lastInputTime) >= AutoCloseDuration)
        {
            Application.Quit(); 
            return;
        }
        
    }

    // 何らかの入力があれば true、無ければ false を返却する   
    bool InputAnyKey()
    {
        return Input.anyKey ||
            Mathf.Abs(Input.GetAxis("Horizontal")) > 0.3f ||
            Mathf.Abs(Input.GetAxis("Vertical")) > 0.3f ||
            Mathf.Abs(Input.GetAxis("Mouse X")) > 0 ||
            Mathf.Abs(Input.GetAxis("Mouse Y")) > 0 ||
            Mathf.Abs(Input.GetAxis("Mouse ScrollWheel")) > 0;
    }
}
