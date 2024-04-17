using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
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
    Coroutine resetGameCoroutine = null;

    public void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (inPauseMenu)
            {
                Resume();
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Confined;
                EventSystem.current.SetSelectedGameObject(firstSelected);
                inPauseMenu = true;
                menuGameObject.SetActive(true);

            }
        }
    }


    public void Resume()
    {
        Cursor.lockState = CursorLockMode.Locked;
        inPauseMenu = false;
        menuGameObject.SetActive(false);
    }

    public void TweekAI(float difficulty)
    {
        handleText.text = difficultyTexts[(int)difficulty];
        //We can add more modifiers to AI, PAUL!! - Gotchu budd ;)
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
        ResetGame("Lose");
    }

    public void ReturnMainMenu()
    {
        ResetGame("Main Menu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }


    public void ResetGame(string scene)
    {
        if (resetGameCoroutine == null) resetGameCoroutine = StartCoroutine(ResetGameCoroutine(scene));
    }

    public IEnumerator ResetGameCoroutine(string scene)
    {
            Time.timeScale = 1;
            Debug.Log(NetworkManager.Singleton);
            NetworkManager.Singleton.Shutdown();
            while (NetworkManager.Singleton.ShutdownInProgress)
            {
                yield return null;
            }
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Destroy(NetworkManager.Singleton.gameObject);
            Destroy(GameManager.instance.gameObject);
            AsyncOperation asyncLoadLevel = SceneManager.LoadSceneAsync(scene);
            GameManager.instance.saveManager.Save();
            while (!asyncLoadLevel.isDone)
            {
                yield return null;
            }
       
    }
}
