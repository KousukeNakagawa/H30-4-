using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialEnemyScripts : MonoBehaviour {

    //巨大ロボのスクリプトにアクセスしやすいようにするためのまとめ
    //staticを使っているため巨大ロボを2体以上出したらエラーが発生する
    public static TutorialBigEnemyMove bigEnemyMove;
    public static TutorialSearchObject searchObject;
    public static TutorialDestinationArrive destinationArrive;
    public static TutorialShotingPheseMove shootingPhaseMove;
    public static Transform mTransform;
    public static TutorialEnemyEffectManager bigEnemyEffectManager;
    public static TutorialShotingFailure shootingFailure;
    public static TutorialBreakEffectManager breakEffectManager;
    public static TutorialBigEnemyAnimator bigEnemyAnimatorManager;
    public static BigEnemyEffect bigEnemyEffect;
    public static TutorialEnemyAudioManager bigEnemyAudioManager;
    public static TutorialBreakRobotManager breakRobotoManager;

    public GameObject tmaneobj;
    public static TutorialManager_T tmane;

    [SerializeField] private Transform[] transforms;
    //ボディ
    [SerializeField] private Transform body_;
    //ローカルポジション固定
    private Dictionary<Transform, Vector3> localPoses = new Dictionary<Transform, Vector3>();

    private Vector3 startPosition;
    private Quaternion startRotat;

    void Awake()
    {
        mTransform = transform;
        startPosition = transform.position;
        startRotat = transform.rotation;
    }

    //// Use this for initialization
    void Start()
    {
        tmane = tmaneobj.GetComponent<TutorialManager_T>();
        bigEnemyMove = GetComponentInChildren<TutorialBigEnemyMove>();
        searchObject = GetComponentInChildren<TutorialSearchObject>();
        shootingPhaseMove = GetComponent<TutorialShotingPheseMove>();
        bigEnemyEffectManager = GetComponentInChildren<TutorialEnemyEffectManager>();
        shootingFailure = GetComponent<TutorialShotingFailure>();
        breakEffectManager = GetComponentInChildren<TutorialBreakEffectManager>();
        bigEnemyAnimatorManager = GetComponentInChildren<TutorialBigEnemyAnimator>();
        bigEnemyAudioManager = GetComponent<TutorialEnemyAudioManager>();
        bigEnemyEffect = GetComponentInChildren<BigEnemyEffect>();
        breakRobotoManager = GetComponentInChildren<TutorialBreakRobotManager>();
        foreach (Transform child in transform)
        {
            localPoses[child] = child.localPosition;
        }
    }

    void Update()
    {
        if (Time.timeScale == 0) return;
        foreach (Transform child in transform)
        {
            if (localPoses.ContainsKey(child)) child.localPosition = localPoses[child];
        }

        if (transform.position.x > 85 || transform.position.x < -85 ||
            transform.position.z > 85 || transform.position.z < -85)
        {
            if (tmane.IsReaded()) tmane.ResetState(ResetConditions.ENEMYAWAY);
        }
    }

    public void Restart()
    {
        transform.position = startPosition;
        transform.rotation = startRotat;
        bigEnemyMove.isTurn = false;
        searchObject.ResetTarget();
    }
}
