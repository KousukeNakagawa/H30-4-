using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class ButtonFunction : MonoBehaviour
{
    public void StringArgFunction(string s)
    {
        ScecnManager.SceneChange(s);
    }
}