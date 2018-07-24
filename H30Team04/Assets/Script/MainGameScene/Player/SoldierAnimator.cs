using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SoldierAnimator : MonoBehaviour
{
    /// <summary> 足音SE </summary>
    [SerializeField] AudioClip footSteps_;

    /// <summary> 移動モーション時のSEピッチ１ </summary>
    [SerializeField, Range(0.1f, 3)] float firstPich_;
    /// <summary> 移動モーション時のSEピッチ２ </summary>
    [SerializeField, Range(0.1f, 3)] float nextPitch_;

    /// <summary> アニメーター </summary>
    Animator animator_;
    /// <summary> 音 </summary>
    AudioSource audio_;

    /// <summary> プレイヤーモーションの列挙型 </summary>
    enum PAType
    {
        // 待機
        Stay,
        // 移動
        Front, Back, Right, Left,
        // 構え
        SetupB, SetupS
    }

    /// <summary> 待機モーション </summary>
    bool isStay = true;

    /// <summary> 前進モーション </summary>
    bool isFront = false;
    /// <summary> 後進モーション </summary>
    bool isBack = false;
    /// <summary> 右移動モーション </summary>
    bool isRight = false;
    /// <summary> 左移動モーション </summary>
    bool isLeft = false;

    /*** 構えるモーションは「前のモーションがStayの時」「武器を切り替えた時」のみ ***/
    /// <summary> ビーコン構えるモーション </summary>
    bool isSetupBeacon = false;
    /// <summary> スナイパー構えるモーション </summary>
    bool isSetupSnipe = false;

    /// <summary> モーション名でモーションのフラグを管理 </summary>
    Dictionary<PAType, bool> motions_;
    /// <summary> 現在のアニメーション </summary>
    PAType type_ = PAType.Stay;

    void Start()
    {
        // コンポーネント
        animator_ = GetComponent<Animator>();
        audio_ = gameObject.GetComponent<AudioSource>();

        // モーションのステートパターンの構築
        motions_ = new Dictionary<PAType, bool>()
        {
            { PAType.Stay,isStay},

            { PAType.Front,isFront},
            { PAType.Back,isBack},
            { PAType.Right,isRight},
            { PAType.Left,isLeft},

            { PAType.SetupB,isSetupBeacon},
            { PAType.SetupS,isSetupSnipe},
        };
    }

    void Update()
    {
        // 一時停止中は何もしない
        if (Time.timeScale == 0) return;

        // アニメーションのセッティング
        SetAnimator();
        // アニメーションの更新
        AnimationUpdate();
        // アニメーションの変更処理
        AnimationChange(type_);
    }

    /// <summary> アニメーションのセッティング </summary>
    void SetAnimator()
    {
        animator_.SetBool("Stay", motions_[PAType.Stay]);

        animator_.SetBool("RunFront", motions_[PAType.Front]);
        animator_.SetBool("RunBack", motions_[PAType.Back]);
        animator_.SetBool("RunRight", motions_[PAType.Right]);
        animator_.SetBool("RunLeft", motions_[PAType.Left]);

        animator_.SetBool("SetupBeacon", motions_[PAType.SetupB]);
        animator_.SetBool("SetupSnipe", motions_[PAType.SetupS]);

        animator_.SetBool("WeaponIsB", WeaponCtrl.IsWeaponBeacon);
    }

    /// <summary> アニメーションの条件処理 </summary>
    void AnimationUpdate()
    {
        // 移動しているなら 前進
        if (Soldier.IsMove) type_ = PAType.Front;

        // 武器を構えていないなら 待機
        else if (!WeaponCtrl.IsSetup) type_ = PAType.Stay;

        // ビーコンを装備したなら ビーコンを構える
        else if (WeaponCtrl.IsWeaponBeacon) type_ = PAType.SetupB;

        // スナイパーを装備したなら スナイパーを構える
        else type_ = PAType.SetupS;
    }

    /// <summary> 指定したモーションタイプをtrueにする </summary>
    void AnimationChange(PAType type)
    {
        // 引数とモーションタイプが一致したらtrue それ以外はfalse
        foreach (var motionType in motions_.Keys.ToArray())
            motions_[motionType] = (motionType == type);
    }

    #region アニメーションイベント

    /// <summary> アニメーションイベント用（歩くSE） </summary>
    void FirstStep()
    {
        audio_.pitch = firstPich_;
        audio_.PlayOneShot(footSteps_);
    }

    /// <summary> アニメーションイベント用（歩くSE） </summary>
    void NextStep()
    {
        audio_.pitch = nextPitch_;
        audio_.PlayOneShot(footSteps_);
    }

    #endregion
}
