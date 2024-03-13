using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class NetworkManagerUI : MonoBehaviour
{

    public void HostGame()
    {
        GameManager.instance.StartCoroutine(HostGameCoroutine());
    }

    public void JoinClient()
    {
        GameManager.instance.StartCoroutine(JoinClientCoroutine());
    }

    IEnumerator HostGameCoroutine()
    {
        AsyncOperation asyncLoadLevel = SceneManager.LoadSceneAsync("Game");
        while (!asyncLoadLevel.isDone)
        {
            yield return null;
        }
        NetworkManager.Singleton.StartHost();
    }

    IEnumerator JoinClientCoroutine()
    {
        AsyncOperation asyncLoadLevel = SceneManager.LoadSceneAsync("Game");
        while (!asyncLoadLevel.isDone)
        {
            yield return null;
        }
        NetworkManager.Singleton.StartClient();
    }
}
