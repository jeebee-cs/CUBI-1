using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using Unity.Netcode.Transports.UTP;
using TMPro;

public class NetworkManagerUI : MonoBehaviour
{
    [SerializeField] TMP_InputField _ipAdresse;
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
        AsyncOperation asyncLoadLevel = SceneManager.LoadSceneAsync("MainGame");
        while (!asyncLoadLevel.isDone)
        {
            yield return null;
        }
        NetworkManager.Singleton.StartHost();
    }

    IEnumerator JoinClientCoroutine()
    {
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(_ipAdresse.text, 7777);
        AsyncOperation asyncLoadLevel = SceneManager.LoadSceneAsync("MainGame");
        while (!asyncLoadLevel.isDone)
        {
            yield return null;
        }
        NetworkManager.Singleton.StartClient();
    }
}
