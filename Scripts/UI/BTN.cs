using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class BTN : MonoBehaviour
{
    public void OnclickLoad()
    {
        SceneManager.LoadScene("Main");
    }
    public void OnclickOption()
    {

    }
    public void OnclickQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

}
