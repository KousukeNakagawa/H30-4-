using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//フェード機能
[System.Serializable]
public class FadeFunction
{
    //フェード機能の取得
    public GameObject _image;

    //フェードアウトからインするまでの間隔
    [Range(0, 3)] public float fadeTime = 1;

    //テキストの取得
    public TextController textController;
}
