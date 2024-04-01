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
    private bool gameOver;

   
    public void winCheck(float pointsGained)
    {
        score += pointsGained;
        if (score >= winningScore)
        {
            Win();
        }
        else if (score < 0 )
        {
            Loose();
        }
    }

    public void Loose()
    {
        if (!gameOver)
        {
            Debug.Log("You loose");
            gameOver = true;
            Reset();
        }
    }
    public void Win()
    {
        if (!gameOver)
        {
            Debug.Log("You win");
            gameOver = true;
            Reset();
        }
    }

    public void Reset()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
