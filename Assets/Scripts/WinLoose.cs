using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class WinLoose : MonoBehaviour
{
    [SerializeField] private UnityEvent winning;
    [SerializeField] float winningScore = 1;
    Coroutine resetGameCoroutine = null;
    private bool _gameFinished = false;
    bool firstBlock = true;
    [SerializeField] GameObject staticBlock;
    public bool gameOver { get => _gameFinished; set => _gameFinished = value; }

    private void Start()
    {
        AkSoundEngine.PostEvent("Music_Start", this.gameObject);
    }

    public void winCheck(float dreamEnergy, int neutralDreamCollected)
    {
        if (dreamEnergy >= winningScore)
        {
            LevelWin();
        }
        else if (dreamEnergy < 0 || neutralDreamCollected < 0)
        {
            Lose();
        }
    }

    public void Lose()
    {
        AkSoundEngine.PostEvent("Stop_All_Audio", this.gameObject);
        Debug.Log("You lose");
        _gameFinished = true;
        ResetGame("Lose");
    }

    public void LevelWin()
    {
        AkSoundEngine.PostEvent("Stop_All_Audio", this.gameObject);
        Debug.Log("You win");
        _gameFinished = true;
        ResetGame("Win");
    }

    /*public void GameWin()
    {
        Debug.Log("You win");
         _gameFinished = true;
         Reset();
    }*/

    public void Reload(int reload)
    {
        if (GameManager.instance.neutralDreamCollected - reload >= 0)
        {
            GameManager.instance.saveManager.saveCall("respawn");
        }
        else
        {
            Lose();
        }
    }

    public void ResetGame(string scene)
    {
        if (resetGameCoroutine == null) resetGameCoroutine = StartCoroutine(ResetGameCoroutine(scene));
    }
    public IEnumerator ResetGameCoroutine(string scene)
    {
        if (_gameFinished)
        {
            Time.timeScale = 1;
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
            while (!asyncLoadLevel.isDone)
            {
                yield return null;
            }
        }

        else
        {
            GameManager.instance.SetDreamEnergyServerRpc(0);
        }
    }
    public void firstBlockChange(GameObject block)
    {
        if (firstBlock)
        {
            Debug.Log("first block ");
            firstBlock = false;
            Vector3 blockPosition = block.transform.position;
            Destroy(block);
            Debug.Log("Here " + blockPosition);
            Instantiate(staticBlock, block.transform.position, Quaternion.identity);
        }
    }
}