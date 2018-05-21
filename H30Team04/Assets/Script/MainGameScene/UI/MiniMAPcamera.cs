using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMAPcamera : MonoBehaviour
{
    [SerializeField]
    GameObject m_Player;
    [SerializeField]
    float cameraH = 20;
    //[SerializeField]
    //private Image MinimapIcon;
    //[SerializeField]
    //private Image MInimapArrow;
    //[SerializeField]
    //GameObject[] m_targets;
    //[SerializeField]
    //private Camera m_MinimapCamera;
    //int x_count;

    public Rect _rect;
    //private Rect _canvasRect;

    // Use this for initialization
    void Start()
    {
        _rect = new Rect(0, 0, 1, 1);
        // UIがはみ出ないようにする
        //_canvasRect = ((RectTransform)MInimapArrow.canvas.transform).rect;
        //_canvasRect.Set(
        //   _canvasRect.x + MInimapArrow.rectTransform.rect.width * 0.5f,
        //    _canvasRect.y + MInimapArrow.rectTransform.rect.height * 0.5f,
        //    _canvasRect.width - MInimapArrow.rectTransform.rect.width,
        //    _canvasRect.height - MInimapArrow.rectTransform.rect.height
        //);
    }
    private void FixedUpdate()
    {
       // m_targets = GameObject.FindGameObjectsWithTag("Xline");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(m_Player.transform.position.x, m_Player.transform.position.y + cameraH, m_Player.transform.position.z);
        //transform.rotation = Quaternion.Euler(0, m_Player.transform.localEulerAngles.y, 0);
        //for(int i=0;i<m_targets.Length;i++)
        //{
        //    print(i);
        //        var viewport = m_MinimapCamera.WorldToViewportPoint(m_targets[x_count].transform.position);
        //    if (_rect.Contains(viewport))
        //    {

        //        MinimapIcon.enabled = true;
        //        MInimapArrow.enabled = false;

        //        MinimapIcon.transform.position = m_targets[x_count].transform.position;
        //    }
        //    else
        //    {
        //        MinimapIcon.enabled = false;
        //        MInimapArrow.enabled = true;

        //        viewport.x = Mathf.Clamp01(viewport.x);
        //        viewport.y = Mathf.Clamp01(viewport.y);
        //        MInimapArrow.rectTransform.anchoredPosition = Rect.NormalizedToPoint(_canvasRect, viewport);
        //    }


        //}

    }
}
