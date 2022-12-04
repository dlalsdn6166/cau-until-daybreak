using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public GameObject GameUI;
    public TMP_Text nowScore;
    public TMP_Text ZombieCount;
    public TMP_Text nowHP;
    public TMP_Text StageText;

    PlayerCharacterController playerScript;
    // Start is called before the first frame update

    private void Awake()
    {
        playerScript = GameObject.Find("Player").GetComponent<PlayerCharacterController>();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        nowScore.SetText("now Score");
        ZombieCount.SetText(Zombie.Count.ToString());
        nowHP.SetText(playerScript.hp.ToString());
    }
}
