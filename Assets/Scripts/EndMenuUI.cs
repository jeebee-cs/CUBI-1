using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndMenuUI : MonoBehaviour
{
    public void BackMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
