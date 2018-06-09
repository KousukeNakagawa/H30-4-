using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UpdateManager : MonoBehaviour
{
    private static UpdateManager updateManager;

    void Awake()
    {  //Static関数で呼び出しが出来るようにする
       //BigEnemyUpdateManager manager = FindObjectOfType<BigEnemyUpdateManager>();
       //updateManager = manager;
        updateManager = this;
    }

    [SerializeField] private const int initialCount = 32;
    [SerializeField] private bool reduce;
    private int tail = 0;

    private static IUpdate[] updates = new IUpdate[initialCount];

    void Update()
    {
        foreach (var update in updates)
        {
            if (update == null) continue;
            update.UpdateMe();
        }
    }

    void FixedUpdate()
    {
        foreach (var update in updates)
        {
            if (update == null) continue;
            update.FixedUpdateMe();
        }
    }

    void LateUpdate()
    {
        foreach (var update in updates)
        {
            if (update == null) continue;
            update.LateUpdateMe();
        }
    }
    /// <summary>
    /// Update対象の追加
    /// </summary>
    /// <param name="iupdate"></param>
    public static void AddUpdate(IUpdate iupdate)
    {
        if (iupdate == null) return;
        updateManager.addUpdate(iupdate);
    }

    private void addUpdate(IUpdate iupdate)
    {
        if (updates.Length == tail)
        {
            Array.Resize(ref updates, checked(tail * 2));
        }
        updates[tail++] = iupdate;
    }

    /// <summary>
    /// Updateの削除
    /// </summary>
    /// <param name="iupdate"></param>
    public static void RemoveUpdate(IUpdate iupdate)
    {
        if (iupdate == null) return;
        updateManager.removeUpdate(iupdate);
    }

    private void removeUpdate(IUpdate iupdate)
    {
        for (int i = 0; i < updates.Length; i++)
        {
            if (updates[i].Equals(iupdate))
            {
                updates[i] = null;
                updateManager.refreshUpdate();
                return;
            }
        }
    }

    /// <summary>
    /// 配列の整理
    /// </summary>
    public static void RefreshUpdate()
    {
        updateManager.refreshUpdate();
    }

    private void refreshUpdate()
    {
        int j = tail - 1;
        for (int i = 0; i < updates.Length; i++)
        {
            if (updates[i] == null)
            {
                while (i < j)
                {
                    var formTail = updates[j];
                    if (formTail != null)
                    {
                        updates[i] = formTail;
                        updates[j] = null;
                        j--;
                        goto NEXTLOOP;
                    }
                }
                tail = i;
                break;
            }
            NEXTLOOP:
            continue;
        }

        if (reduce && tail < updates.Length / 2)
        {
            Array.Resize(ref updates, updates.Length / 2);
        }
    }

}
