using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public void GameReStart()
    {
        Zombie.KillScore = 0;
        Zombie.Count = 0;
        SceneManager.LoadScene("SampleScene");
    }

    public void Exit()
    {
        Debug.Log("Game is exiting");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else  
        Application.Quit();
#endif
    }
}
