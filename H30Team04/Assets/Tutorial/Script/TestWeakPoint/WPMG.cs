using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> 弱点管理クラス </summary>
public class WPMG : MonoBehaviour
{
    /// <summary> 弱点とその確率を格納 </summary>
    public static Dictionary<TestWeakPoint, int>
        weakProbadility = new Dictionary<TestWeakPoint, int>();

    /// <summary> 弱点と確率を追加・更新 </summary>
    public static void Add(TestWeakPoint point, int probability)
    {
        // 既に登録されているなら
        if (weakProbadility.ContainsKey(point))
            // 既存の弱点を削除
            weakProbadility.Remove(point);

        // 弱点と確立を追加
        weakProbadility.Add(point, probability);
    }

    /// <summary> 指定した弱点の確率を返す </summary>
    public static int GetProbadility(TestWeakPoint point)
    {
        return weakProbadility[point];
    }

    /// <summary> 指定した弱点の確率を文字で返す (一度も撮影されなかったら ？ を返す) </summary>
    public static string GetProbadilityStr(TestWeakPoint point)
    {
        // 確率が50％ (一度も撮影されなかったら) ？ を返す
        return (weakProbadility[point] == 50) ?
            "?" : weakProbadility[point].ToString();
    }
}