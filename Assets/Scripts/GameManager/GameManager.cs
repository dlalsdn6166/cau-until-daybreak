using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public PlayerCharacterController Player { get; private set; }

    public Stage[] stages;

    public event UnityEngine.Events.UnityAction<bool> Gameover;

    public int CurrentStage { get; private set; }
    public bool Paused { get; private set; }

    private void Awake()
    {
        Instance = this;
        Player = FindObjectOfType<PlayerCharacterController>();
    }

    Coroutine ee;

    public void Pause(bool value)
    {
        Paused = value;
        Cursor.visible = value;
        Cursor.lockState = value ? CursorLockMode.None: CursorLockMode.Locked;
        Time.timeScale = value ? 0 : 1;
    }

    public void Start()
    {
        Pause(false);        
        if (ee == null)
            ee = StartCoroutine(Flow());
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

        Gameover?.Invoke(true);
    }

    public void playerdead() => Gameover?.Invoke(false);
}