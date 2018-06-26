using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSwitchCamera : MonoBehaviour {

    [SerializeField] private Transform target;
    [SerializeField] private float distance = 5.0f;

    public TutorialManager_T tmane;
    private FollowCamera m_script;

    // Use this for initialization
    void Start()
    {
        transform.position = target.position + (-target.right * distance);
        Vector3 angle = target.eulerAngles;
        angle.y += 90;
        transform.eulerAngles = angle;
        m_script = GetComponent<FollowCamera>();
        m_script.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (tmane.IsReaded() && !m_script.enabled) m_script.enabled = true;
        if (Mathf.Abs(transform.eulerAngles.y - target.eulerAngles.y) < 1 || transform.position.x > target.position.x + 0.5f) Destroy(transform.parent.gameObject);
        
    }

}
