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
    }


    private void Save()
    {
        GameManager gameManager = GameManager.instance;
        playerPosition = playerObject.transform.position;
        neutralDreams = gameManager.neutralDreamCollected;
        totalScore = gameManager.dreamEnergy;
        neutralDreamsUI = gameManager.uIManager.neutralDreams.text;
        string dir = Application.persistentDataPath + directory;
        Debug.Log(dir);
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
            playerObject.transform.position = playerPosition;
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
