using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    enum PhaseState
    {
        photoState,
        attackState
    }

    private PhaseState phaseState;
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

    GameObject Flayers;
    GameObject Arrows;
    public int m_FlyerCount;
    bool m_IsChange;
    private int currentSelectStageIndex;

    // Use this for initialization
    void Start () {
        phaseState = PhaseState.photoState;
        currentSelectStageIndex = 0;
        Flayers = transform.Find("Screen").Find("Flyers").gameObject;
        Arrows = Flayers.transform.Find("Arrows").gameObject;
    }
	
	// Update is called once per frame
	void Update () {
        m_MiniMap.transform.position = new Vector3(m_Player.transform.position.x, m_Player.transform.position.y + cameraH, m_Player.transform.position.z);
        UpdateSelect();
    }
    void FixedUpdate()
    {
        switch (phaseState)
        {
            case PhaseState.photoState: PhotoState(); break;
            case PhaseState.attackState: AttackState(); break;
        }
    }
    void UpdateSelect()
    {
        const float Margin = 0.5f;
        float inputHorizontal = (Input.GetAxisRaw("XboxLeftHorizontal") != 0) ? Input.GetAxisRaw("XboxLeftHorizontal") : Input.GetAxisRaw("Horizontal");
        if (Mathf.Abs(inputHorizontal) > Margin && !m_IsChange)
        {
            if (inputHorizontal > 0.0f)
            {
                currentSelectStageIndex += 1;
            }
            else
            {
                currentSelectStageIndex += (m_FlyerCount - 1);
            }
            currentSelectStageIndex = currentSelectStageIndex % m_FlyerCount;
            float l_positionX = currentSelectStageIndex * 1280;
            //Arrows.GetComponent<Arrows>().SetTargetLocalPositionX(l_positionX);
            //Flayers.GetComponent<Flyers>().MoveTargetPositionX(-l_positionX);
            m_IsChange = true;
        }
        else if (Mathf.Abs(inputHorizontal) <= Margin)
        {
            m_IsChange = false;
        }
    }

        private void PhotoState()
    {
        GameClear();
        GameOver();
        PhaseTransition();
    }

    private void AttackState()
    {

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
            phaseState = PhaseState.attackState;
        }
    }
}
