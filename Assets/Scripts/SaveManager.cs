using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    
    public static string directory = "/SaveData/";
    public static string fileName = "GameData.txt";
    public GameObject playerObject;
    int neutralDreams;
    float totalScore;
    string neutralDreamsUI;
    Vector3 playerPosition;



    void Awake()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
        SaveSystem.init();
        SaveObject saveObject = new SaveObject()
        {
            neutralDreams = 0,
        };
        string json = JsonUtility.ToJson(saveObject);
        //Debug.Log(json);

        SaveObject saveLoaded = JsonUtility.FromJson<SaveObject>(json);
    }


    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("trying to save...");
            Save();
            Debug.Log("Saved!");
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            Debug.Log("Loading...");
            Load();
            Debug.Log("Loaded");
        }
    }

    public void saveCall(string option)
    {
        if (option == "Save")
        {
            Save();
        }
        if (option == "Load")
        {
            Load();
        }
        if (option == "respawn")
        {
            respawn(playerObject.transform);
        }
    }


    private void Save()
    {
        GameManager gameManager = GameManager.instance;
        playerPosition = playerObject.transform.position;
        neutralDreams = gameManager.dreamCollection.dreamsCollectionN;
        totalScore = gameManager.winLoose.scoreCount;
        neutralDreamsUI = gameManager.uIManager.neutralDreams.text;
        string dir = Application.persistentDataPath + directory;

        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }

        SaveObject saveObject = new SaveObject
        {
            neutralDreams = neutralDreams,
            totalScore = totalScore,
            neutralDreamsUI = neutralDreamsUI,
            playerPosition = playerPosition
        };

        string json = JsonUtility.ToJson(saveObject, true);
        SaveSystem.Save(json);
    }

    private void Load()
    {
        GameManager gameManager = GameManager.instance;
        string saveString = SaveSystem.Load();
        if (saveString != null)
        {

            SaveObject saveObject = JsonUtility.FromJson<SaveObject>(saveString);

            gameManager.uIManager.neutralDreams.text = saveObject.neutralDreams.ToString();
            gameManager.dreamCollection.dreamsCollectionN = saveObject.neutralDreams;
            gameManager.winLoose.scoreCount = saveObject.totalScore;
            playerObject.transform.position = playerPosition;
        }

    }

    private void respawn(Transform player)
    {
        GameManager gameManager = GameManager.instance;
        string saveString = SaveSystem.Load();
        if (saveString != null)
        {

            SaveObject saveObject = JsonUtility.FromJson<SaveObject>(saveString);
            gameManager.dreamCollection.dreamsCollectionN = saveObject.neutralDreams - 1;
            gameManager.winLoose.scoreCount = saveObject.totalScore - 0.05f;
            playerObject.transform.position = player.position;
            gameManager.uIManager.neutralDreams.text = saveObject.neutralDreams.ToString();
        }

    }
    private class SaveObject
    {
        public int neutralDreams;
        public float totalScore;
        public string neutralDreamsUI;
        public Vector3 playerPosition;
    }

}
