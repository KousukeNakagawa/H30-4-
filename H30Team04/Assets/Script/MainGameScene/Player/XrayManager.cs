using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

//射影機管理
public class XrayManager : MonoBehaviour
{
    //射影機とその距離を保持
    Dictionary<GameObject, float> _Xrays = new Dictionary<GameObject, float>();

    void Start()
    {

    }

    void Update()
    {

    }

    /// <summary>
    /// 射影機とその距離を登録
    /// </summary>
    public void XrayEntry(GameObject xray, float distance)
    {
        //射影機のタグがXlineなら登録
        if (!xray.CompareTag("Xline")) return;
        ////距離重複防止
        //if (_Xrays.ContainsValue(distance)) distance += 0.0001f;
        //登録
        _Xrays[xray] = distance;
    }

    /// <summary>
    /// 距離を元にソートしたリストを返す
    /// </summary>
    public List<KeyValuePair<GameObject, float>> GetXrays()
    {
        //リスト化
        var distance = _Xrays.ToList();

        //距離を昇順にソート
        distance.Sort((pair1, pair2) => pair1.Value.CompareTo(pair2.Value));

        return distance;
    }
}