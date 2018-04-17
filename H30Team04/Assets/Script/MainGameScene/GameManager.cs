using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    [SerializeField]
    GameObject m_Player;
    [SerializeField]
    GameObject m_Enemy;
    [SerializeField]
    GameObject m_MiniMap;
    [SerializeField]
    float cameraH = 20;
    [HideInInspector]
    public float redLine;
    [SerializeField]
    bool test = false;  //必ず消し去るbool型


    // Use this for initialization
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        m_MiniMap.transform.position = new Vector3(m_Player.transform.position.x, m_Player.transform.position.y + cameraH, m_Player.transform.position.z);
        GameClear();
        GameOver();
        PhaseTransition();
    }

    void GameClear()
    {
        if (m_Enemy == null)
        {
            Debug.Log("ゲームクリア");
        }
    }

    void GameOver()
    {
        if (m_Player == null)
        {
            Debug.Log("ゲームオーバー");
        }
    }

    void PhaseTransition()
    {
        if (test)
        {
            Debug.Log("防衛ラインに侵入した");
        }
    }
}
