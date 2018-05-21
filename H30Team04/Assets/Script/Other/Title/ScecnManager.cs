using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class ScecnManager{

    public static void SceneChange(string name)
    {
        SceneManager.LoadScene(name);
    }
}
