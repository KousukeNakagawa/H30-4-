using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> チュートリアルでのプレイヤーの行動制限 </summary>
public class UnlockManager : MonoBehaviour
{
    /// <summary> ture = 解除 </summary>
    public static Dictionary<UnlockState, bool> Limiter
        = new Dictionary<UnlockState, bool>();

    static int unlockStateNum = 6;

    //SoldierのStartで管理
    //void Awake()
    //{
    //    //全て false にして登録
    //    //SoldierのStartで
    //    for (int i = 0; i < unlockStateNum; i++)
    //        Limiter.Add(UnlockState.move + i, false);
    //}

    /// <summary> 全行動制限の解除 or 制限 </summary>
    public static void AllSet(bool value)
    {
        for (int i = 0; i < unlockStateNum; i++)
            Limiter[UnlockState.move + i] = value;
    }

    /// <summary> プレイヤーの行動制限 </summary>
    public static void Lock(UnlockState state)
    {
        Limiter[state] = false;
    }

    /// <summary> 行動制限の解除 </summary>
    public static void Unlock(UnlockState state)
    {
        Limiter[state] = true;
    }
}
