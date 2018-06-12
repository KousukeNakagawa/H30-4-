using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> 射影機を指定する矢印 </summary>
[System.Serializable]
public class Arrow
{
    //LineRendererを持つオブジェクト
    public GameObject _lineObject;

    //値が小さいほど近くに描画
    [Range(0, 10), Tooltip("矢印描画始点")]
    public int _startPoint = 4;

    //始点・終点の色
    public Color _startColor, _endColor;

    [Range(0.1f, 3)] public float width = 2;

    //近いほど濃くなり、遠いほど薄くなる
    [Range(1, 2), Header("[距離に比例する透明度]")]
    public float _colorDepth = 1;

    //透明度の最低値
    [Range(0, 1)]
    public float _minDepth = 0.5f;
}
