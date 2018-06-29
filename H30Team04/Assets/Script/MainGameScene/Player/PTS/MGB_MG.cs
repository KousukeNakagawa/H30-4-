using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary> マシンガンの弾 管理クラス </summary>
public class MGB_MG : MonoBehaviour
{
    /// <summary> マシンガンの弾を格納・管理するリスト </summary>
    public static List<MG_Bullet>
        MG_bullets = new List<MG_Bullet>();
    /// <summary> 生成するマシンガンの弾の数 </summary>
    static int instantiateNum = 10;
    /// <summary> マシンガンの弾を保管する場所 </summary>
    static Vector3 storagePlace;

    void Start()
    {
        // マシンガンの弾を生成
        InstantiateMGB();
    }

    void Update()
    {

    }

    /// <summary> マシンガンの弾 </summary>
    public static MG_Bullet Bullet()
    {
        // まだ発射されていない弾を検索
        var bullet = MG_bullets.First(x => !x.IsFire);
        // 発射された判定にする
        bullet.IsFire = true;

        return bullet;
    }

    /// <summary> マシンガンの弾生成処理 </summary>
    static void InstantiateMGB()
    {
        for (int i = 0; i < instantiateNum; i++)
        {
            // 弾の生成
            var bullet = new MG_Bullet();
            // 弾の準備処理
            bullet.GetComponent<MG_Bullet>().Init();
            // 出現させる
            Instantiate(bullet);
            // 生成した弾を保管場所へ移動
            bullet.gameObject.transform.position = storagePlace;
            // 隠す
            bullet.gameObject.SetActive(false);
            // リストへ追加
            MG_bullets.Add(bullet);
        }
    }
}
