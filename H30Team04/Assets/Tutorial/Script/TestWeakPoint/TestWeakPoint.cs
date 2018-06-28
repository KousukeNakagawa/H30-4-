using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> 弱点クラス </summary>
/*

◎撮影成功する度に一定量確率を＋していく

〇本物：１つ
・最大100％になる

〇偽物：４つ
・最大90％～70％になる

*/
public class TestWeakPoint : MonoBehaviour
{
    /// <summary> 確率変動量 </summary>
    [SerializeField, Range(1, 10)] int amount = 10;
    /// <summary> 確率の最小値 </summary>
    [SerializeField, Range(0, 50)] int min = 0;
    /// <summary> 確率の最大値 </summary>
    [SerializeField, Range(50, 100)] int max = 100;
    /// <summary> 本物かどうか </summary>
    [SerializeField] bool isGenuine = false;

    /// <summary> 確率 </summary>
    int probadility;
    /// <summary> 現在と前フレームの撮影状態 </summary>
    bool currentPhoto, oldPhoto;

    /// <summary> 撮影されたか </summary>
    public bool IsInPhoto { get; set; }

    void Start()
    {
        // 初期値
        probadility = 50;
        IsInPhoto = false;

        // 自身と確率を登録
        WPMG.Add(this, probadility);
    }

    void Update()
    {
        // 常に新しい撮影状態を取得
        currentPhoto = IsInPhoto;

        // 撮影されたときの処理
        InPhotoProcessing();

        // 撮影状態を2フレーム以上連続で true にしない処理
        IsInPhotoReset();

        // 前フレームの状態を更新
        oldPhoto = currentPhoto;
    }

    /// <summary> 写真に写った際の処理 </summary>
    void InPhotoProcessing()
    {
        // 確率の最低値と最大値の調整
        Mathf.Clamp(probadility, min, max);

        // 撮影されたら (撮影された瞬間)
        if (IsInPhoto)
            // 確率をプラスさせる
            probadility += amount;
    }

    /// <summary> 撮影状態を１フレーム以上持続させない処理 </summary>
    void IsInPhotoReset()
    {
        // 現在：撮影状態 前フレーム：非撮影状態
        if (currentPhoto && !oldPhoto)
            // 非撮影状態に戻す
            IsInPhoto = false;
    }
}
