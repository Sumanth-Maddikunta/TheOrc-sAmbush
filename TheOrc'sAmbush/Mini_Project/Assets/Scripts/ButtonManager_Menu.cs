using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonManager_Menu : MonoBehaviour
{
    public GameObject blackCircle;
    public GameObject instructionsPanel;
    public GameObject creditsPanel;
    public GameObject gameName;
    public AudioSource audioComponent;
    public Button audioButton;

    public Sprite audioOn;
    public Sprite audioOff;

    bool arePanelsActive = false;
    bool isInstructionBeingDisplayed = false;


    private void Update()
    {
        if(arePanelsActive==false)
        {
            gameName.SetActive(true);
        }
        else
        {
            gameName.SetActive(false);
        }
    }

    public void Quit()
    {
        Application.Quit();
    }
   

    public void Play()
    {

        StartCoroutine(ChangeScene());
       
    }
    public void Information()
    {
        instructionsPanel.SetActive(true);
        isInstructionBeingDisplayed = true;
        arePanelsActive = true;
    }

    public void Back()
    {
        instructionsPanel.SetActive(!true);
        isInstructionBeingDisplayed = !true;

        arePanelsActive = false;
        creditsPanel.SetActive(!true);

    }

    public void Credits()
    {
        creditsPanel.SetActive(true);
        arePanelsActive = true;
    }
    IEnumerator ChangeScene()
    {
        blackCircle.GetComponent<Animator>().SetTrigger("ZoomIn");
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("Game");

    }

    public void Audio()
    {
      audioComponent.mute = !audioComponent.mute;
        if(audioComponent.mute==true)
        {
            audioButton.image.sprite = audioOff;
        }
        else
        {
            audioButton.image.sprite = audioOn;
        }
        //Needs to be done
    }

}
