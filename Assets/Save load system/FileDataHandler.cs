using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class FileDataHandler
{
    private string dataDirPath = "";
    private string dataFileName = "";

    public FileDataHandler(string dataDirPath, string dataFileName)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
    }

    public GameData Load()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);

        GameData loadedData = null;

        if(File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";

                using(FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);

            }
            catch (Exception e)
            {
                Debug.LogError("Something happened when trying to Load the data from: " + fullPath + "\n" + e);
            }
        }

        return loadedData;
    }

    public void Save(GameData data)
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);

        try
        {
            //create a directory if it doensn't already exsist.
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            string saveData = JsonUtility.ToJson(data, true);

            //using makes sure that connection to file is closed when done reading / writing to it.
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(saveData);
                }
            }
        }
        catch (Exception e)
        {

            Debug.LogError("Something happened when trying to save the data to: " + fullPath + "\n" + e);
        }
    }

    public void DeleteData()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);

        try
        {
            if(File.Exists(fullPath))
            {
                Directory.Delete(Path.GetDirectoryName(fullPath), true);
            }
            else
            {
                Debug.LogError("Tried to delete data that does not exsist: " + fullPath);
            }
        }
        catch (Exception e)
        {

            Debug.LogError("Something happened when trying to delete save data: " + fullPath + "\n" + e);
        }
    }
}
