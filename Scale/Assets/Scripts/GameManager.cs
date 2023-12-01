using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.Timeline;
using UnityEngine.Playables;

public class GameManager : MonoBehaviour
{
    private int playerHealth = 20;
    private int enemyHealth = 20;

    public Slider playerHealthSlider;
    public Slider enemyHealthSlider;
    public static bool isSinglePlayer = true;

    public GameObject startMenu;
    public GameObject modeMenu;
    public TextMeshProUGUI countdownText;
    public AudioSource gameManagerAudio;
    public AudioSource loopableAudio;

    [SerializeField]
    public AudioClip[] gameSoundEffects = new AudioClip[1];
    public AudioClip audienceSoundTrack;

    public GameObject youWinScene;
    public PlayableDirector youWinDir;

    public GameObject gameOverScene;
    public PlayableDirector gameOverDir;

    public static bool isPlaying;


    // Start is called before the first frame update
    void Start()
    {
        playerHealth = 20;
        enemyHealth = 20;

        youWinScene.SetActive(false);
        gameOverScene.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            ChangeScene(0);
        } 

        YouWin();
        YouLose();
    }

    public void ReducePlayerHealth(int healthToReduce)
    {
        playerHealth -= healthToReduce;
        playerHealthSlider.value = playerHealth;
    }

    public void ReduceEnemyHealth(int healthToReduce)
    {
        enemyHealth -= healthToReduce;
        enemyHealthSlider.value = enemyHealth;
    }

    public void YouWin()
    {
        // If enemyHealth is 0
        if(enemyHealth <= 0)
        {
            loopableAudio.Stop();
            isPlaying = false;
            youWinScene.SetActive(true);           
        }
    }

    public void YouLose()
    {
        // if playerHealth is 0

        if(playerHealth <= 0)
        {
            loopableAudio.Stop();
            isPlaying = false;
            gameOverScene.SetActive(true);
        }
    }

    private IEnumerator WaitUntilTimelineEnds()
    {
        yield return new WaitForSeconds(2f);
    }

    public void SwitchToModeMenu()
    {
        startMenu.SetActive(false);
        modeMenu.SetActive(true);
    }

    public void ChangeScene(int sceneIndex)
    {
        switch(sceneIndex)
        {
            case 11:
                isSinglePlayer = true;
                SceneManager.LoadScene(1);
                break;
            case 1:
                isSinglePlayer = false;
                SceneManager.LoadScene(1);
                break;
            default:
                SceneManager.LoadScene(sceneIndex);
                break;
        }
    }

    public IEnumerator Countdown()
    {
        float waitTime = 0.7f;
        
        isPlaying = false;
        countdownText.gameObject.SetActive(true);
        gameManagerAudio.PlayOneShot(gameSoundEffects[0]); 

        countdownText.text = "3";
        yield return new WaitForSeconds(0.5f);

        countdownText.text = "2";
        yield return new WaitForSeconds(waitTime);

        countdownText.text = "1";
        yield return new WaitForSeconds(waitTime);

        countdownText.text = "FIGHT";
        yield return new WaitForSeconds(waitTime);

        countdownText.gameObject.SetActive(false);
        isPlaying = true;

        gameManagerAudio.PlayOneShot(gameSoundEffects[1], 1);
        loopableAudio.clip = audienceSoundTrack;
        loopableAudio.Play();
    }
}
