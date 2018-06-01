using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//射影機
public class XraySupport : MonoBehaviour
{
    [SerializeField] GameObject _XrayManager;
    XrayManager _manager;

    GameObject _player;

    void Start()
    {
        _manager = _XrayManager.GetComponent<XrayManager>();
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        XrayEntry();
    }

    /// <summary>
    /// 自身のタグが"Xline"なら
    /// </summary>
    void XrayEntry()
    {
        //自身とプレイヤーとの距離を取得
        float distanceToPlayer = Vector3.Distance(transform.position, _player.transform.position);
        //XrayManagerに自身とその距離を登録
        _manager.XrayEntry(gameObject, distanceToPlayer);
    }

    /// <summary>
    /// タグの変更
    /// </summary>
    public void Shutter()
    {
        transform.tag = "XlineEnd";
    }
}
