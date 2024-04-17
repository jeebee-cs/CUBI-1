using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    Scene menuScene;

    public bool inPauseMenu;

    public AI ai;

    public GameObject menuGameObject;
    public TextMeshProUGUI handleText;

    public string[] difficultyTexts;

    public GameObject firstSelected;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (inPauseMenu)
            {
                Resume();
            }
            else
            {
                EventSystem.current.SetSelectedGameObject(firstSelected);
                inPauseMenu = true;
                menuGameObject.SetActive(true);
            }
        }
    }


    public void Resume()
    {
        inPauseMenu = false;
        menuGameObject.SetActive(false);
    }

    public void TweekAI(float difficulty)
    {
        handleText.text = difficultyTexts[(int)difficulty];
        //We can add more modifiers to AI, PAUL!!
        switch (difficulty)
        {
            case (0):
                ai.movementSpeed = 2;
                break;
            case (1):
                ai.movementSpeed = 3;
                break;
            case (2):
                ai.movementSpeed = 4;
                break;
        }
    }



    public void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnMainMenu()
    {
        SceneManager.LoadScene(menuScene.name);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
