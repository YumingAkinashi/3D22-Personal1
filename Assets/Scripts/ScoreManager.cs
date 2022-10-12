using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{

    public static ScoreManager instance;

    // References
    [Header("References")]
    public WaveSpawner waveSpawner;

    // Values
    [Header("Values")]
    public int score;
    public int farmLife;
    public int waveTimer;
    public int wave;

    // HUD texts
    [Header("Text References")]
    public Text HUDscore;
    public Text HUDfarmLife;
    public Text HUDwaveTimer;
    public Text HUDwave;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        UpdateHUD();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Enemy"))
        {
            collision.transform.SendMessage("TouchedFarm");
            FarmInvaded();
        }
    }
    public void FarmInvaded()
    {
        farmLife--;
        UpdateHUD();
    }
    public void UpdateHUD()
    {
        HUDscore.text = "Score: " + score.ToString();
        HUDfarmLife.text = "Farm's Life: " + farmLife.ToString();
        HUDwave.text = "Wave: " + waveSpawner.currWave.ToString();
        HUDwaveTimer.text = "Timer: " + Mathf.RoundToInt(waveSpawner.waveTimer).ToString();
    }


}
