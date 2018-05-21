using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuilCreate : MonoBehaviour {

    public GameObject centerPrefab;
    public GameObject topPrefab;

    public int centerCount = 1;

    private BoxCollider m_Collider;
    private Transform underBuil;

	// Use this for initialization
	void Start () {
        underBuil = transform.Find("builunder");
        int builSize = 1;
        float builHeight = (transform.name.Contains("mini"))? 3.5f : MainStageDate.BuildingHeight;

        Vector3 newPos;

        //中央部分作成
        for (int i = 0; i < centerCount; i++)
        {
            GameObject center = Instantiate(centerPrefab);
            center.transform.parent = transform;
            newPos = new Vector3(0,builSize * builHeight, 0) + underBuil.localPosition;
            builSize++;
            center.transform.localPosition = newPos;
            center.transform.rotation = transform.rotation;
        }

        //屋上作成
        GameObject top = Instantiate(topPrefab);
        top.transform.parent = transform;
        newPos = new Vector3(0, builSize * builHeight, 0) + underBuil.localPosition;
        builSize++;
        top.transform.localPosition = newPos;
        top.transform.rotation = transform.rotation;

        if(GetComponent<BuilSlide>() == null)
        {
            m_Collider = GetComponent<BoxCollider>();
            //あたり判定の設定
            m_Collider.size = new Vector3(m_Collider.size.x, builSize * builHeight, m_Collider.size.z);
            m_Collider.center = new Vector3(m_Collider.center.x, (builSize * builHeight) / 2, m_Collider.center.z);

            GetComponent<BuilCrush>().Builsize = builSize;
        }

        enabled = false;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetCenterCount(string count)
    {
        int num = int.Parse(count);
        num -= 2;
        centerCount = num;
    }
}
