using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class WinLoose : MonoBehaviour
{
    [SerializeField] private UnityEvent winning;
    [SerializeField] float winningScore = 0.80f;
    [SerializeField] float score = 0.10f;
    private bool _gameFinished = false;
    bool firstBlock = true;
    [SerializeField] GameObject staticBlock;
    public bool gameOver { get => _gameFinished; set => _gameFinished = value; }

    public float scoreCount { get => score; set => score = value; }
    public void winCheck(float pointsGained)
    {
        score += pointsGained;
        if (score >= winningScore)
        {
            LevelWin();
        }
        else if (score < 0)
        {
            Lose();
        }
    }

    public void Lose()
    {
        Debug.Log("You lose");
        _gameFinished = true;
        Reset();
    }

    public void LevelWin()
    {
        Debug.Log("You win");
        Reset();
    }

    /*public void GameWin()
    {
        Debug.Log("You win");
         _gameFinished = true;
         Reset();
    }*/

    public void Reload(int reload)
    {
        if (GameManager.instance.dreamCollection.dreamsCollectionN - reload >= 0)
        {
            GameManager.instance.saveManager.saveCall("respawn");
        }
        else
        {
            Lose();
        }
    }

    public void Reset()
    {
        if (_gameFinished)
        {
            //game finished
            Time.timeScale = 1;
            SceneManager.LoadScene(0);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            
        }

        else
        {
            scoreCount = 0;
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