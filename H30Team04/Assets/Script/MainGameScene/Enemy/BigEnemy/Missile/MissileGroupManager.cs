using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileGroupManager : MonoBehaviour
{
    public GameObject rightGroup;
    public GameObject leftGroup;
    public GameObject missilePrefab;

    private List<GameObject> MissileList = new List<GameObject>();
    [Tooltip("ミサイルを発射する本数")] public int missileCount = 3;

    // Use this for initialization
    void Start()
    {
        Quaternion r = Quaternion.identity;
        for (int i = rightGroup.transform.childCount - 1; i >= 0; i--)
        {
            Transform t = rightGroup.transform.GetChild(i);
            if (r.Equals(Quaternion.identity)) r = t.rotation;
            GameObject im = Instantiate(missilePrefab, t.position, r, rightGroup.transform);
            Destroy(rightGroup.transform.GetChild(i).gameObject);
            MissileList.Add(im);
        }
        for (int i = leftGroup.transform.childCount - 1; i >= 0; i--)
        {
            Transform t = leftGroup.transform.GetChild(i);
            GameObject im = Instantiate(missilePrefab, t.position, r, leftGroup.transform);
            Destroy(leftGroup.transform.GetChild(i).gameObject);
            MissileList.Add(im);
        }
    }

    public void MissileSet()
    {
        StartCoroutine(MissileLaunch());
    }

    IEnumerator MissileLaunch()
    {
        for (int i = 0; i < missileCount; i++)
        {
            yield return new WaitForSeconds(0.5f);
            int num = Random.Range(0, MissileList.Count - 1);
            GameObject missile = MissileList[num];
            missile.transform.parent = null;
            missile.GetComponent<Rigidbody>().isKinematic = false;
            missile.GetComponent<MissileMove2>().enabled = true;
            missile.GetComponent<MissileCollider>().enabled = true;
            missile.GetComponent<TrailRenderer>().enabled = true;
            MissileList.RemoveAt(num);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
