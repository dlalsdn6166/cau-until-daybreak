using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject GameUI;
    public TMP_Text nowScore;
    public TMP_Text ZombieCount;
    public TMP_Text nowHP;
    public TMP_Text nowStage;

    public GameObject PauseUI;
    public TMP_Text main;
    public TMP_Text nowStage2;
    public TMP_Text nowScore2;

    public TextMeshProUGUI interactNotify;
    public bool interactive { get; private set; }
    PlayerCharacterController playerScript;
    GameManager gameManagerScript;
    // Start is called before the first frame update

    private bool gameisover = false;

    public void GameReStart()
    {
        Zombie.KillScore = 0;
        Zombie.Count = 0;
        gameisover = false;
        SceneManager.LoadScene("Map");
        main.SetText("Paused");
        GameManager.Instance.Start();
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

    private void Awake()
    {
        playerScript = GameObject.Find("Player").GetComponent<PlayerCharacterController>();
        gameManagerScript = GameObject.Find("Gamemanager").GetComponent<GameManager>();
        gameManagerScript.Gameover += GameManagerScript_Gameover;
    }

    private void GameManagerScript_Gameover(bool arg0)
    {
        gameisover = true;
        GameUI.SetActive(false);
        showPaused(true);
        if (arg0)
            main.SetText("You Win");
        else
            main.SetText("You Lose");
    }

    void Start()
    {
        PauseUI.SetActive(false);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && !gameisover)
        {
            showPaused(!PauseUI.activeSelf);
            GameUI.SetActive(!GameUI.activeSelf);
        }

        interactNotify.enabled = playerScript.Interactable;
    }

    private void showPaused(bool value)
    {
        gameManagerScript.Pause(value);
        PauseUI.SetActive(value);
        if (!value)
            return;
        main.SetText("Until Daybreak");
        nowScore2.SetText(Zombie.KillScore.ToString());
        nowStage2.SetText((gameManagerScript.CurrentStage + 1).ToString());
    }

    void LateUpdate()
    {
        nowScore.SetText(Zombie.KillScore.ToString());
        nowStage.SetText((gameManagerScript.CurrentStage + 1).ToString());
        ZombieCount.SetText(Zombie.Count.ToString());
        nowHP.SetText(Mathf.FloorToInt(playerScript.hp).ToString());
    }
}
