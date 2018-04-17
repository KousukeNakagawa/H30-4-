using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuilCreate : MonoBehaviour {

    public GameObject centerPrefab;
    public GameObject topPrefab;

    public int centerCount = 1;
    private int builHeightCount = 1;  //ビルを作るときの今何階建てか

    private BoxCollider m_Collider;
    private Transform underBuil;

	// Use this for initialization
	void Start () {
        m_Collider = GetComponent<BoxCollider>();
        underBuil = transform.Find("builunder");
        int builSize = 1;

        Vector3 newPos;

        //中央部分作成
        for (int i = 0; i < centerCount; i++)
        {
            GameObject center = Instantiate(centerPrefab);
            center.transform.parent = transform;
            newPos = new Vector3(0,builSize * MainStageDate.BuildingHeight,0) + underBuil.localPosition;
            builSize++;
            center.transform.localPosition = newPos;
            center.transform.rotation = transform.rotation;
        }

        //屋上作成
        GameObject top = Instantiate(topPrefab);
        top.transform.parent = transform;
        newPos = new Vector3(0, builSize * MainStageDate.BuildingHeight, 0) + underBuil.localPosition;
        builSize++;
        top.transform.localPosition = newPos;
        top.transform.rotation = transform.rotation;

        //あたり判定の設定
        m_Collider.size = new Vector3(m_Collider.size.x, builSize * MainStageDate.BuildingHeight, m_Collider.size.z);
        m_Collider.center = new Vector3(m_Collider.center.x, (builSize * MainStageDate.BuildingHeight)/2, m_Collider.center.z);
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
