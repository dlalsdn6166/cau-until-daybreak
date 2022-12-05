using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public PlayerCharacterController Player { get; private set; }

    public Stage[] stages;

    public int CurrentStage { get; private set; }
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
        for (CurrentStage = 0; CurrentStage < stages.Length; CurrentStage++)
        {
            yield return new WaitForSeconds(stages[CurrentStage].interval);
            stages[CurrentStage].Trigger();

            while (Zombie.Count > 0)
                yield return null;
        }

        // TODO Game result
        Pause(true);
        Debug.Log("win");
    }
}