using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformExtension {

    public static GameObject[] GetAllChildren(this GameObject obj)
    {
        List<GameObject> allChildren = new List<GameObject>();
        GetChildren(obj, ref allChildren);
        return allChildren.ToArray();
    }

    //子要素を取得してリストに追加
    private static void GetChildren(GameObject obj, ref List<GameObject> allChildren)
    {
        Transform children = obj.GetComponentInChildren<Transform>();
        //子要素がいなければ終了
        if (children.childCount == 0)
        {
            return;
        }
        foreach (Transform ob in children)
        {
            allChildren.Add(ob.gameObject);
            GetChildren(ob.gameObject, ref allChildren);
        }
    }

    public static GameObject[] GetAllGameObject(this GameObject obj)
    {
        List<GameObject> allObjects = new List<GameObject>();
        GetChildren(obj, ref allObjects);
        allObjects.Add(obj);
        return allObjects.ToArray();
    }
}
