using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> チュートリアルでのプレイヤーの行動制限 </summary>
public class UnlockManager : MonoBehaviour
{
    public static Dictionary<TutorialState, bool> limit
        = new Dictionary<TutorialState, bool>();

    void Awake()
    {
        //全て false にして登録
        for (int i = 0; i < 4; i++)
            limit.Add(TutorialState.move + i, false);
    }

    void Update()
    {

    }

    /// <summary> 全行動制限の解除 or 制限 </summary>
    public void AllSet(bool value)
    {
        for (int i = 0; i < 4; i++)
            limit[TutorialState.move + i] = value;
    }

    /// <summary> プレイヤーの行動制限 </summary>
    public void Lock(TutorialState tutorial)
    {
        limit[tutorial] = false;
    }

    /// <summary> 行動制限の解除 </summary>
    public void Unlock(TutorialState tutorial)
    {
        limit[tutorial] = true;
    }
}
