using UnityEngine;

public class UpdateRoot : MonoBehaviour, IUpdate
{
    void OnEnable()
    {
        UpdateManager.AddUpdate(this);
    }

    void OnDisable()
    {
        UpdateManager.RemoveUpdate(this);
    }

    virtual public void FixedUpdateMe()
    {
    }

    virtual public void LateUpdateMe()
    {
    }

    virtual public void UpdateMe()
    {
    }

}