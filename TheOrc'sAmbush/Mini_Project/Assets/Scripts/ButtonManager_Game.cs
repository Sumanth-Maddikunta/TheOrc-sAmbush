using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonManager_Game : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public GameObject gameOverUI;
    public Player player;
   
    public Button audioButton;
    public Sprite audioOn;
    public Sprite audioOff;
    public  bool isGamePaused = false;



   
    bool isGameOver = false;
    AudioSource gameManagerAudioSource;

    private void Start()
    {        
        gameManagerAudioSource = GameObject.FindGameObjectWithTag("GameManager").GetComponent<AudioSource>();
    }
    public void Pause()
    {
        
        if (isGameOver == false)//Game not over
        {
            isGamePaused = !isGamePaused;
            if (isGamePaused)//game is paused
            {
                Time.timeScale = 0;

                pauseMenuUI.SetActive(true);

            }
            else
            {
               
                Time.timeScale = 1;
                pauseMenuUI.SetActive(false);
            }
        }
    }

   

   public void Home()
    {
       
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu");

    }

    public void Restart()
    {
        
        isGameOver = false;
        pauseMenuUI.SetActive(false);
        gameOverUI.SetActive(false);
        Time.timeScale = 1;
        SceneManager.LoadScene("Game");
    }

    public void Audio()
    {
        
        gameManagerAudioSource.mute = !gameManagerAudioSource.mute;
        player.GetComponent<AudioSource>().mute = !player.GetComponent<AudioSource>().mute;
        if (gameManagerAudioSource.mute==true)
        {
            audioButton.image.sprite = audioOff;
        }
        else
        {
            audioButton.image.sprite = audioOn;
        }

        //Needs to be done
    }

    public void Play()
    {
        
        SceneManager.LoadScene("Game");
    }

    private void Update()
    {
        if(player.playerHealth<=0)
        {
            isGameOver = true;
            gameOverUI.SetActive(true);
                                            
        }

    }

   
}
