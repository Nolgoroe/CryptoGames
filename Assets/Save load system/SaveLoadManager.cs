using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class SaveLoadManager : MonoBehaviour
{
    [Header("Save load data needed")]
    [SerializeField] private string fileName;

    private GameData gameData;
    private List<ISaveLoadable> saveLoadObjects;
    private FileDataHandler dataHandler;

    public static SaveLoadManager instance;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(this);

        instance = this;
    }

    private void Start()
    {
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        saveLoadObjects = FindAllSaveLoadObjects();
        LoadGame();
    }

    public void NewGame()
    {
        gameData = new GameData();
    }

    public void LoadGame()
    {
        gameData = dataHandler.Load();

        if(gameData == null)
        {
            Debug.Log("No save data found - creating default data");
            NewGame();
            return;
        }

        foreach (ISaveLoadable saveLoadObject in saveLoadObjects)
        {
            saveLoadObject.LoadData(gameData);
        }


        DeleteData();
    }

    [ContextMenu("Override save?")]
    public void SaveGame()
    {
        foreach (ISaveLoadable saveLoadObject in saveLoadObjects)
        {
            saveLoadObject.SaveData(gameData);
        }

        dataHandler.Save(gameData);
    }

    [ContextMenu("Delete data")]
    public void DeleteData()
    {
        dataHandler.DeleteData();
    }

    private void OnApplicationQuit()
    {
        SaveGame();

    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
            SaveGame();

    }

    private void OnApplicationFocus(bool focus)
    {
        if(!focus)
            SaveGame();
    }

    private List<ISaveLoadable> FindAllSaveLoadObjects()
    {
        //find all monobehaviours that inherit from ISaveLoadable - all of the scripts we want to save.
        IEnumerable<ISaveLoadable> ISaveLoadObjects = FindObjectsOfType<MonoBehaviour>().OfType<ISaveLoadable>();

        //initialize a new list with the data of all the objects we just found.
        return new List<ISaveLoadable>(ISaveLoadObjects);
    }
}
