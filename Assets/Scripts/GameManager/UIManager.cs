using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public GameObject GameUI;
    public TMP_Text nowScore;
    public TMP_Text scoreAtDead;
    public TMP_Text scoreAtWin;
    public TMP_Text ZombieCount;
    public TMP_Text nowHP;
    public TMP_Text nowStage;
    public GameObject PauseUI;
    public GameObject DeadUI;
    public GameObject WinUI;

    public TextMeshProUGUI interactNotify;
    public bool interactive { get; private set; }
    PlayerCharacterController playerScript;
    GameManager gameManagerScript;
    // Start is called before the first frame update

    private void Awake()
    {
        playerScript = GameObject.Find("Player").GetComponent<PlayerCharacterController>();
        gameManagerScript = GameObject.Find("Gamemanager").GetComponent<GameManager>();
    }

    void Start()
    {
        PauseUI.SetActive(false);
        DeadUI.SetActive(false);
        WinUI.SetActive(false);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)&&!DeadUI.activeSelf&&!WinUI.activeSelf)
        {
            gameManagerScript.Pause(!PauseUI.activeSelf);
            PauseUI.SetActive(!PauseUI.activeSelf);
        }

        if (playerScript.isDead)
        {
            playerScript.isDead = false;
            scoreAtDead.SetText(Zombie.KillScore.ToString());
            gameManagerScript.Pause(!DeadUI.activeSelf);
            DeadUI.SetActive(!DeadUI.activeSelf);
        }

        if (gameManagerScript.win)
        {
            gameManagerScript.win = false;
            scoreAtWin.SetText(Zombie.KillScore.ToString());
            gameManagerScript.Pause(!WinUI.activeSelf);
            WinUI.SetActive(!WinUI.activeSelf);
        }

        if (playerScript.Interactable)
        {
            interactNotify.enabled = true;
        }
        else
        {
            interactNotify.enabled = false;

        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        nowScore.SetText(Zombie.KillScore.ToString());
        nowStage.SetText(gameManagerScript.CurrentStage.ToString());
        ZombieCount.SetText(Zombie.Count.ToString());
        nowHP.SetText(Mathf.FloorToInt(playerScript.hp).ToString());
    }
}
