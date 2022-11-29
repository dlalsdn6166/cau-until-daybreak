using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public PlayerCharacterController Player { get; private set; }

    public Stage[] stages;
    public bool Paused { get; private set; }

    private void Awake()
    {
        Instance = this;
        Player = FindObjectOfType<PlayerCharacterController>();
    }

    public void Pause(bool value)
    {
        Paused = value;
        Cursor.visible = !value;
        Cursor.lockState = value ? CursorLockMode.None : CursorLockMode.Locked;
        Time.timeScale = value ? 0 : 1;
    }

    private void Start()
    {
        Pause(false);
        StartCoroutine(Flow());
    }

    private IEnumerator Flow()
    {
        for (int i = 0; i < stages.Length; i++)
        {
            yield return new WaitForSeconds(stages[i].interval);
            stages[i].Trigger();

            while (Zombie.Count > 0)
                yield return null;
        }

        // TODO Game result
        // Pause(true);
        Debug.Log("win");
    }
}